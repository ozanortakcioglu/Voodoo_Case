using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailTrigger : MonoBehaviour
{
    private bool isTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            if (other.CompareTag(Tags.PLAYER))
            {
                isTriggered = true;
            }
        }

    }
}
