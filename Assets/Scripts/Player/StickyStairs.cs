using UnityEngine;

public class StickyStairs : MonoBehaviour
{
    public GameObject player;
    private bool onStair = false;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Stairs")
        {
            onStair = true;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            onStair = false;
        }
    }

    private void Update()
    {
        if(onStair)
        {
            //player.
        }
    }
}
