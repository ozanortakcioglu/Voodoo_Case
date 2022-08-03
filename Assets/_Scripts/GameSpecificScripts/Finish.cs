using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject[] multipliers;

    private bool isTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            if (other.CompareTag(Tags.PLAYER))
            {
                isTriggered = true;
                PlayerController.Instance.Win();
            }
        }
    }

    public void SetEmisionToMultiplier(float posZ)
    {
        var min = Mathf.Infinity;
        Renderer multiplierRenderer = null;

        foreach (var item in multipliers)
        {
            if(Mathf.Abs(item.transform.position.z - posZ) < min)
            {
                min = Mathf.Abs(item.transform.position.z - posZ);
                multiplierRenderer = item.GetComponent<Renderer>();
            }
        }

        multiplierRenderer.material.SetColor("_EmissionColor", multiplierRenderer.material.color);
    }
}
