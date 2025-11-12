using UnityEngine;

public class EvelatorTeleporter : MonoBehaviour
{
    public Transform box1;
    public Transform box2;

    private bool inBox1 = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {

                Teleport(box1, box2);
     
        }
    }

    void Teleport(Transform fromBox, Transform toBox)
    {
        // Get the player's position relative to the current box
        Vector3 localPos = fromBox.InverseTransformPoint(transform.position);

        // Convert that local position to world space in the new box
        Vector3 newWorldPos = toBox.TransformPoint(localPos);

        // Move player
        transform.position = newWorldPos;
    }
}
