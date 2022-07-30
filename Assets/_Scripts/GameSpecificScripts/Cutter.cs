using DG.Tweening;
using UnityEngine;

public class Cutter : MonoBehaviour
{
    private bool isTriggered = false;

    private void Start()
    {
        transform.DORotate(new Vector3(0, 0, 359), 4f, RotateMode.FastBeyond360).SetRelative(true).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            if (other.CompareTag(Tags.PLAYER) || other.CompareTag(Tags.STICK))
            {
                isTriggered = true;
                FindObjectOfType<PlayerStick>().CutStick(transform.position.x);
            }
        }

    }
}
