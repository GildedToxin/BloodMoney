using UnityEngine;
using UnityEngine.VFX;

public class AutoDestroyVFX : MonoBehaviour
{
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}