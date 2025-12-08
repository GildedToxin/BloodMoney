using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Magnitism : CartScript
{
    public List<Rigidbody> gameObjects = new List<Rigidbody>();
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody newGO = other.GetComponent<Rigidbody>();
        Debug.Log(newGO);

        if (newGO != null)
        {
            Debug.Log("NewGO");
            gameObjects.Add(newGO);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody newGO = other.GetComponent<Rigidbody>();

        if (newGO != null)
        {
            gameObjects.Remove(newGO);
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            foreach (Rigidbody GO in gameObjects)
            {
                GO.isKinematic = true;
                GO.transform.position = cart.transform.position;
                GO.transform.rotation = cart.transform.rotation;    
            }

            if (!isMoving)
            {
                foreach (Rigidbody GO in gameObjects)
                {
                    GO.isKinematic = false;
                    GO.transform.position = GO.transform.position;
                    GO.transform.rotation = GO.transform.rotation;

                }
            }
        }
    }

}
