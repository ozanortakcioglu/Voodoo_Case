using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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
        GetComponent<Renderer>().enabled = false;
        Vector3 worldToScreen = Camera.main.WorldToScreenPoint(transform.position);
        var iconImage = UIManager.Instance.gemIcon;
        GameObject flyingGem = Instantiate(iconImage, worldToScreen, Quaternion.identity, iconImage.transform.parent);
        
        flyingGem.transform.DOMove(iconImage.transform.position, 0.5f).OnComplete(() =>
        {
            GameManager.Instance.GemCount++;
            Destroy(flyingGem);
        });
        EffectsManager.Instance.PlayEffect(EffectTrigger.Collectable, transform.position, new Vector3(-90, 0, 0), Vector3.one, null, Color.green);
        SoundManager.Instance.PlaySound(SoundTrigger.Gem);
    }

}
