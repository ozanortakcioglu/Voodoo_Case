using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailTrigger : MonoBehaviour
{
    [SerializeField] private bool isEnd;
    private bool isTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            if (other.CompareTag(Tags.PLAYER))
            {
                isTriggered = true;
                if (isEnd)
                {

                }
                else
                {

                }
            }
        }

    }
}
