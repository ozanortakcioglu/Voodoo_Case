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
        FindObjectOfType<PlayerStick>().AddStick(ySize);
        EffectsManager.Instance.PlayEffect(EffectTrigger.Collectable, transform.position, new Vector3(-90, 0, 0), Vector3.one, null, new Color(1, 0, 1));
        SoundManager.Instance.PlaySound(SoundTrigger.Stick);

        Destroy(gameObject);
    }
}
