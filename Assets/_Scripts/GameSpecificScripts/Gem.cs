using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour, ICollectable
{
    private bool isTriggered = false;
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
        //Collect Gem
    }

}
