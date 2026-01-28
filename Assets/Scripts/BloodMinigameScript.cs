using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BloodMinigameScript : MonoBehaviour
{
    public GameObject bloodDropStartPoint;
    public GameObject playerHitZone;
    public GameObject bloodDropPrefab;
    private float bloodDropTimer = 0.0f;
    private float bloodDropTimeLimit = 3.0f;
    public bool startGame = false;

    private float bloodDropSpeed = 200.0f;

    private List<GameObject> activeBloodDrops = new List<GameObject>();
    void Start()
    {
        
    }

    void Update()
    {
        if (startGame)
        {
            bloodDropTimer += Time.deltaTime;
            if (bloodDropTimer >= bloodDropTimeLimit)
                SpawnBloodDrop();
            MoveBloodDrops();
        }
    }

    private void SpawnBloodDrop()
    {
        GameObject newBloodDrop = Instantiate(bloodDropPrefab, new Vector3(bloodDropStartPoint.transform.position.x, bloodDropStartPoint.transform.position.y, 0), Quaternion.identity);
        activeBloodDrops.Add(newBloodDrop);
        newBloodDrop.GetComponent<RectTransform>().SetParent(this.transform);
        bloodDropTimer = 0.0f;
    }

    private void MoveBloodDrops()
    {
        foreach (GameObject bloodDrop in activeBloodDrops)
        {
            bloodDrop.GetComponent<RectTransform>().position += new Vector3(-bloodDropSpeed * Time.deltaTime, 0, 0);

            if (bloodDrop.transform.position.y <= playerHitZone.transform.position.y)
            {
                    // Handle hit logic here
                    
            }
        }
    }
}
