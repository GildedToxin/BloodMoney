using UnityEngine;

public class LimbManager : MonoBehaviour
{
 public bool isHandHarvested = false;
    public GameObject hand;
    void Start()
    {
        if(isHandHarvested)
        {
            Destroy(hand);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
