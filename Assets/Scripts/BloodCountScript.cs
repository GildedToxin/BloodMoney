using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class BloodCountScript : MonoBehaviour
{
    private DecalProjector decal;

    void Start()
    {
        decal = GetComponent<DecalProjector>();
        if (GameManager.Instance.updateBloodSplatters == true)
        {
            GameManager.Instance.totalBloodSplatters += 1;
            GameManager.Instance.remainingBloodSplatters += 1;
        }
    }

    void FixedUpdate()
    {
        if (decal.fadeFactor <= 0.25f)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDestroy()
    {
        if (GameManager.Instance.updateBloodSplatters == true)
        {
            GameManager.Instance.remainingBloodSplatters -= 1;
        }
    }
}
