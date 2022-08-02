using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    #region Movement Parameters
    [SerializeField] private float leftLimit = -2f;
    [SerializeField] private float rightLimit = 2f;
    [SerializeField] private float forwardSpeed = 4f;
    [SerializeField] private float leftRightSpeed = 2;

    private bool controlsEnabled = false;
    private float swipeSensivity;
    private float maxSwipeSensivity = 2000f;
    private float targetX;
    #endregion


    #region OnRail Parameters
    private bool onRail = false;
    private bool onGround = true;
    private float leftRailXPos;
    private float rightRailXPos;
    private float tolerance = 0.15f;
    #endregion

    private Rigidbody rb;
    private Animator animator;

    private bool isStart = true;
    private bool isFail = false;
    private bool isWin = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        TouchHandler.onTouchBegan += TouchBegan;
        TouchHandler.onTouchMoved += TouchMoved;
        TouchHandler.onTouchEnded += TouchEnded;

        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (controlsEnabled)
        {
            Movement();
            CheckOnRail();
        }
    }

    public void Win()
    {
        isWin = true;
    }

    public void Fail(bool isFall)
    {
        if (isFail)
            return;

        SoundManager.Instance.PlaySound(SoundTrigger.Fail);
        isFail = true;
        controlsEnabled = false;
        FindObjectOfType<FollowerCamera>().enabled = false;
        if (isFall)
        {
            GetComponentInChildren<Collider>().enabled = false;
            animator.SetTrigger("Fall");
        }
        else
        {
            animator.SetTrigger("Death");
            rb.velocity = Vector3.zero;
        }
        UIManager.Instance.OpenPanel(PanelNames.LosePanel, true, 1f);
    }

    #region OnRail Functions

    private void CheckOnRail()
    {
        if (onGround || !onRail)
            return;

        var playerStick = GetComponentInChildren<PlayerStick>();
        if (playerStick == null)
            return;

        var playetStickLeftSideX = playerStick.GetSidePosition(true).x - tolerance;
        var playetStickRightSideX = playerStick.GetSidePosition(false).x + tolerance;

        var xPos = transform.position.x;

        if (xPos < leftRailXPos || xPos > rightRailXPos || 
            playetStickLeftSideX > leftRailXPos || playetStickRightSideX < rightRailXPos)
        {
            if(!isWin) // Fail
                Fail(true);
            else
            {
                animator.SetTrigger("Fall");
                controlsEnabled = false;
            }
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            playerStick.transform.SetParent(null);
            playerStick.GetComponentInChildren<Collider>().tag = "Untagged";
            playerStick.gameObject.AddComponent<Rigidbody>().angularDrag = 1;
        }
    }

    public void RailTriggered(bool isEnd, float _leftRailX, float _rightRailX)
    {        
        if (isEnd)
        {
            onGround = false;
            StartCoroutine(SetOnRailWithDelay(0, false));
            animator.ResetTrigger("Hang1");
            animator.SetTrigger("Hang2");
        }
        else
        {
            StartCoroutine(SetOnRailWithDelay(0.4f, true));
            leftRailXPos = _leftRailX;
            rightRailXPos = _rightRailX;

            animator.ResetTrigger("Hang2");
            animator.SetTrigger("Hang1");
        }
    }

    private IEnumerator SetOnRailWithDelay(float delay, bool _onRail)
    {
        yield return new WaitForSeconds(delay);
        onRail = _onRail;
        onGround = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Tags.GROUND) && !onGround)
        {
            EffectsManager.Instance.PlayEffect(EffectTrigger.Land, transform.position, Vector3.zero, Vector3.one, null);
            Taptic.Light();

            onGround = true;
            if (isWin) //Win
            {
                SoundManager.Instance.PlaySound(SoundTrigger.Win);
                animator.SetTrigger("Dance");
                controlsEnabled = false;
                UIManager.Instance.OpenPanel(PanelNames.WinPanel, true, 2f);
            }
            else
            {
                animator.ResetTrigger("Hang1");
                animator.SetTrigger("Run");
            }
        }
    }
    #endregion


    #region Movement Functions
    private void TouchBegan(TouchInput touch)
    {
        if (isStart)
        {
            isStart = false;
            controlsEnabled = true;
            UIManager.Instance.OpenPanel(PanelNames.InGame, true);
            animator.SetTrigger("Run");
        }
        targetX = transform.position.x;
    }

    private void TouchEnded(TouchInput touch)
    {
        targetX = transform.position.x;
        swipeSensivity = 0f;
    }

    private void TouchMoved(TouchInput touch)
    {
        swipeSensivity = Mathf.Abs(touch.DeltaScreenPosition.x) * leftRightSpeed;

        if (swipeSensivity > maxSwipeSensivity)
            swipeSensivity = maxSwipeSensivity;


        if (touch.DeltaScreenPosition.x < 0)
            targetX = transform.position.x - swipeSensivity / 200f;
        if (touch.DeltaScreenPosition.x > 0)
            targetX = transform.position.x + swipeSensivity / 200f;
    }

    private void Movement()
    {
        targetX = Mathf.Clamp(targetX, leftLimit, rightLimit);

        rb.velocity = new Vector3((targetX - transform.position.x) * 50, Mathf.Clamp(rb.velocity.y, -10, -0f), forwardSpeed);     
    }
    #endregion


    private void OnDestroy()
    {
        TouchHandler.onTouchBegan -= TouchBegan;
        TouchHandler.onTouchMoved -= TouchMoved;
        TouchHandler.onTouchEnded -= TouchEnded;
    }
}
