using UnityEngine;

public class CartItemMagnitism : MonoBehaviour
{
    [SerializeField]
    Rigidbody itemsRidgedBody; //need to make this a listable object in order to add new organs when they spawn in post minigame
    public CartScript cartScript;

    public float maxDistance;
    public float maxStrength;

    void Update()
    {
        if (cartScript.isMoving)
        {
            updateMagnetism();
        }
    }

    void updateMagnetism()
    {
        float distance = Vector3.Distance(itemsRidgedBody.transform.position, this.transform.position);

        if (distance < maxDistance)
        {
            float TotalDistance = Mathf.InverseLerp(maxDistance, 0f, distance);
            float strength = Mathf.Lerp(0f, maxStrength, TotalDistance);
            Vector3 cartDirection = (this.transform.position - itemsRidgedBody.transform.position).normalized;

            itemsRidgedBody.AddForce(cartDirection * strength, ForceMode.Force);
        }
    }
}
