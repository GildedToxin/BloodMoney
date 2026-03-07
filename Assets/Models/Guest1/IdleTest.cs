using UnityEngine;

public class IdleTest : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        bool holdingIdleKey = Input.GetKey(KeyCode.I);

        anim.SetBool("IsIdle", holdingIdleKey);
    }
}


