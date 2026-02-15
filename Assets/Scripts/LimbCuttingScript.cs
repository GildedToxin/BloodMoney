using UnityEngine;
using System.Collections.Generic;
using System;

public class LimbCuttingScript : MonoBehaviour
{
    public int numberOfPoints = 20;
    public float radius = 1f;
    public Vector3 centerPoint;
    public GameObject pointPrefab;
    public float rotationDegrees;
    public Quaternion limbRotation;
    public int lastXPos;
    public bool wasLastShiftPositve;


    public Vector2 amplitudeRange = new Vector2(0.5f, 3f);
    public Vector2 frequencyRange = new Vector2(1f, 4f);
    public float waveFrequency;
    public float waveAmplitude;


    public GameObject limbs;
    public int currentLimb = -1;

    void Start()
    {
        limbRotation = this.transform.rotation;
    }

    public Vector3[] GeneratePointsOnCircle(int numPoints, float circleRadius, Vector3 circleCenter)
    {
        Vector3[] pointsArray = new Vector3[numPoints];
        waveAmplitude = UnityEngine.Random.Range(amplitudeRange.x, amplitudeRange.y);
        waveFrequency = UnityEngine.Random.Range(frequencyRange.x, frequencyRange.y);

        for (int i = 0; i < numPoints; i++)
        {
            // Calculate the angle for each point (in radians)
            float angle = i * Mathf.PI * 2f / numPoints;

            // Calculate the x and z coordinates using sine and cosine
            float y = Mathf.Cos(angle) * circleRadius;
            float z = Mathf.Sin(angle) * circleRadius;

            float t = i / (float)(numPoints - 1);


            
            float x = Mathf.Sin(t * Mathf.PI * 2f * waveFrequency) * waveAmplitude;

            Vector3 pointPosition = new Vector3(x, y, z) + circleCenter;

            pointsArray[i] = pointPosition;
            //Debug.Log($"Point {i}: {pointsArray[i]}");
        }

        return pointsArray;
    }

    public void CreatePoints()
    {
        this.transform.rotation = limbRotation; // Resets the limb transform rotation/position
        Vector3[] points = GeneratePointsOnCircle(numberOfPoints, radius, centerPoint);
        // print(points.Length);

        var i = 0;
        foreach (Vector3 point in points)
        {
            if (pointPrefab != null)
            {
                try
                {
                    Instantiate(pointPrefab, point, Quaternion.identity, this.transform);
                    i++;
                //    print(i + " POINTS MADE");
                }
                catch
                {
                    print("NO POINT MADE");
                }
            }
            else
            {
                Debug.LogError("Generated Point: " + point);
            }
        }
        NextLimb();
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
            print("DESTROYING");
        }
    }
    public void NextLimb()
    {
        foreach(Transform child in limbs.transform)
        {
            child.gameObject.SetActive(false);
        }
        currentLimb++;
        try
        {
            limbs.transform.GetChild(GameManager.Instance.Body.limbsHarvested).gameObject.SetActive(true);
        }
        catch
        {
            limbs.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
