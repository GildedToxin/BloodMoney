using UnityEngine;
using System.Collections.Generic;

public class LimbCuttingScript : MonoBehaviour
{
    public int numberOfPoints = 20;
    public float radius = 1f;
    public Vector3 centerPoint;
    public GameObject pointPrefab;
    public float rotationDegrees;
    public Quaternion limbRotation;


    void Start()
    {
        limbRotation = this.transform.rotation;
    }

    public Vector3[] GeneratePointsOnCircle(int numPoints, float circleRadius, Vector3 circleCenter)
    {
        Vector3[] pointsArray = new Vector3[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            // Calculate the angle for each point (in radians)
            float angle = i * Mathf.PI * 2f / numPoints;

            // Calculate the x and z coordinates using sine and cosine
            float y = Mathf.Cos(angle) * circleRadius;
            float z = Mathf.Sin(angle) * circleRadius;

            // Create the point position relative to the center
            Vector3 pointPosition = new Vector3(0, y, z) + circleCenter;

            pointsArray[i] = pointPosition;
            //Debug.Log($"Point {i}: {pointsArray[i]}");
        }

        return pointsArray;
    }

    public void CreatePoints()
    {
        this.transform.rotation = limbRotation; // Resets the limb transform rotation/position
        Vector3[] points = GeneratePointsOnCircle(numberOfPoints, radius, centerPoint);

        foreach (Vector3 point in points)
        {
            if (pointPrefab != null)
                Instantiate(pointPrefab, point, Quaternion.identity, this.transform);
            else
            {
                Debug.LogError("Generated Point: " + point);
            }
        }
    }
    
    public void DestroyPoints()
    {
        bool firstChild = true;
        foreach (Transform child in transform)
        {
            if (firstChild)
            {
                firstChild = false;
                continue; // Skip the first child (the limb itself)
            }
            Destroy(child.gameObject);
        }
    }
}
