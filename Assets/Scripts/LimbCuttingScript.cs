using UnityEngine;
using System.Collections.Generic;

public class LimbCuttingScript : MonoBehaviour
{
    private List<Vector3> referencePoints;
    private List<Vector3> playerPoints;
    public Transform p0, p1, p2, p3;
    private int segments = 50;
    public LineRenderer lineRenderer;

    void Start()
    {
        DrawCurve();
    }

    void DrawCurve()
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
    }

    void Update()
    {
        
    }
}
