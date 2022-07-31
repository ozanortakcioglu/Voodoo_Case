using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float leftLimit = -2f;
    [SerializeField] private float rightLimit = 2f;
    [SerializeField] private float forwardSpeed = 4f;
    [SerializeField] private float leftRightSpeed = 2;

    private float swipeSensivity;
    private float maxSwipeSensivity = 20000f;
    private float targetX;

    private Rigidbody rb;
    private Animator animator;

    private bool controlsEnabled = false;
    private bool isStart = true;

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
            Movement();
    }

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

    private void OnDestroy()
    {
        TouchHandler.onTouchBegan -= TouchBegan;
        TouchHandler.onTouchMoved -= TouchMoved;
        TouchHandler.onTouchEnded -= TouchEnded;
    }
}
