using UnityEngine;
using Deform;
using DG.Tweening;

public class Stick : MonoBehaviour, ICollectable
{
    [SerializeField] private float size = 1;
    [SerializeField] private GameObject model;

    private bool isTriggered = false;
    private BendDeformer bendDeformer;

    private void Start()
    {
        transform.localScale = new Vector3(1, size, 1);
        bendDeformer = GetComponentInChildren<BendDeformer>();
        var newScale = bendDeformer.gameObject.transform.localScale;
        newScale.y /= size;
        bendDeformer.Factor = 10;
        var bendFactor = bendDeformer.Factor;
        DOTween.To(() => bendFactor, x => bendFactor = x, -10, Random.Range(1f, 1.4f)).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).OnUpdate(() => 
        {
            bendDeformer.Factor = bendFactor;
        });

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            if (other.CompareTag(Tags.PLAYER) || other.CompareTag(Tags.STICK))
            {
                isTriggered = true;
                Collect();
            }
        }
    }

    public void Collect()
    {
        var ySize = transform.localScale.y;
        FindObjectOfType<PlayerStick>().AddStick(ySize * 0.35f);
        EffectsManager.Instance.PlayEffect(EffectTrigger.Collectable, transform.position, new Vector3(-90, 0, 0), Vector3.one, null, new Color(1, 0, 1));
        SoundManager.Instance.PlaySound(SoundTrigger.Stick);

        Destroy(gameObject);
    }
}
