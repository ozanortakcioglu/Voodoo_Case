using UnityEngine;

public class RailTrigger : MonoBehaviour
{
    [SerializeField] private bool isEnd;
    [SerializeField] private GameObject leftRail;
    [SerializeField] private GameObject rightRail;

    private bool isTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            if (other.CompareTag(Tags.PLAYER))
            {
                isTriggered = true;
                PlayerController.Instance.RailTriggered(isEnd, leftRail.transform.position.x, rightRail.transform.position.x);
            }
        }
    }
}
