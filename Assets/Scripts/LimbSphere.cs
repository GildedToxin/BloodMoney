using UnityEngine;
public class LimbSphere : MonoBehaviour
{
    [Header("VFX that plays when destroyed")]
    public GameObject destroyVFX;

    private void OnDestroy()
    {
        // Prevent null reference when exiting play mode
        if (!Application.isPlaying) return;

        if (destroyVFX != null)
        {
       //     Instantiate(destroyVFX, transform.position, Quaternion.identity);
        }
    }
}
