using UnityEngine;

public class LavaGround : MonoBehaviour
{
    private bool isTriggered;
    private float time = 1;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER))
        {
            isTriggered = true;

            if (time > 0.1f)
            {
                time = 0;
                FindObjectOfType<PlayerStick>().MeltStick();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER))
        {
            isTriggered = false;
        }
    }

    private void Update()
    {
        if (isTriggered)
        {
            time += Time.deltaTime;
        }
    }
}
