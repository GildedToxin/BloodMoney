using UnityEngine;

public class ArmCheck : MonoBehaviour, IPlayerLookTarget
{

    public bool leftArm;
    public bool rightArm;
    public bool leftLeg;
    public bool rightLeg;

    public bool isLookedAt = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLookEnter()
    {
        isLookedAt = true;
    }
    public void OnLookExit()
    {
        isLookedAt = false;
    }
}
