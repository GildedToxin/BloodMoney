using UnityEngine;
using System.Collections.Generic;

public class LimbCuttingScript : MonoBehaviour
{
    //private List<Vector3> referencePoints;
    //private List<Vector3> playerPoints;
    //public Transform p0, p1, p2, p3;
    //private int segments = 50;
    //public LineRenderer lineRenderer;

    // 2nd attempt variables
    public int numberOfPoints = 20;
    public float radius = 1f;
    public Vector3 centerPoint;
    public GameObject pointPrefab;
    public float rotationDegrees;
    //private float pointHeight = .2f; // Adjusts the height of the circle points

    void Start()
    {
        CreatePoints();
    }

    public Vector3[] GeneratePointsOnCircle(int numPoints, float circleRadius, Vector3 circleCenter)
    {
        Vector3[] pointsArray = new Vector3[numPoints];

        for (int i = 0; i < numPoints; i++)
        {
            // Calculate the angle for each point (in radians)
            float angle = i * Mathf.PI * 2f / numPoints;

            // Calculate the x and z coordinates using sine and cosine
            float y = Mathf.Cos(angle) * circleRadius;//(circleRadius * pointHeight);
            float z = Mathf.Sin(angle) * circleRadius;//(circleRadius * pointHeight);

            // Create the point position relative to the center
            Vector3 pointPosition = new Vector3(0, y, z) + circleCenter;

            pointsArray[i] = pointPosition;
            Debug.Log($"Point {i}: {pointsArray[i]}");
        }

        return pointsArray;
    }
    
    public void CreatePoints()
    {
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

    void Update()
    {
    }
}
