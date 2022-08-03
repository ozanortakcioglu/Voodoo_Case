using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag(Tags.STICK))
        {
            EffectsManager.Instance.PlayEffect(EffectTrigger.Lightning, collision.contacts[0].point, new Vector3(-140f, 0, 0), Vector3.one * 0.3f, EffectsManager.Instance.transform);
            SoundManager.Instance.PlaySound(SoundTrigger.Steel, true);
        }
    }
}
