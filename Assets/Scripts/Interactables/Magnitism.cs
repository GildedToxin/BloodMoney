using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Magnitism : CartScript
{
    public List<Rigidbody> rb = new List<Rigidbody>();
    private void OnTriggerEnter(Collider other)
    {
        Rigidbody newRB = other.GetComponent<Rigidbody>();

        if (newRB != null)
        {
            rb.Add(newRB);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody newRB = other.GetComponent<Rigidbody>();

        if (newRB != null)
        {
            rb.Remove(newRB);
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            foreach (Rigidbody rBodies in rb)
            {
                rBodies.AddForce(Vector3.down * 500); 
            }
        }
    }

}
