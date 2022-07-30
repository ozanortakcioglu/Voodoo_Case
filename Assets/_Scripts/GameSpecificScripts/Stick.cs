using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour, ICollectable
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
        var ySize = transform.localScale.y;
        FindObjectOfType<PlayerStick>().AddStick(ySize * 2);
        //EffectsManager.PlayEffect(EffectTrigger.Match, )
        Destroy(gameObject);
    }
}
