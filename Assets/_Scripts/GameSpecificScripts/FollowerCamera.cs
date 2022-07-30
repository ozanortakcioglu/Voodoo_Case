using UnityEngine;
using DG.Tweening;

public class FollowerCamera : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        transform.DOKill();
        var pos = target.transform.position + offset;
        pos.x = 0;
        transform.DOMove(pos, 0.1f).SetEase(Ease.Linear);
    }

}
