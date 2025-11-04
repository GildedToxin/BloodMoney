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
        //DrawCurve();

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

    void Update()
    {
        
    }

    /*void DrawCurve()
    {
        Vector3[] positions = new Vector3[segments + 1]; // Draws the line renderer along these positions
        for (int i = 0; i <= segments; i++) // Adds specified number of positions from previous line using segments
        {
            float t = i / (float)segments;
            positions[i] = GetBezierPoint(p0.position, p1.position, p2.position, p3.position, t);
        }
        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);        
    }
    
    Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) // I barely understand how this works :')
    {
        float u = 1 - t;
        return u*u*u*p0 + 3*u*u*t*p1 + 3*u*t*t*p2 + t*t*t*p3; // Cubic Bezier formula
    }*/
}
