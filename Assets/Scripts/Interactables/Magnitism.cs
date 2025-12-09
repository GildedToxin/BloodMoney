using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Magnitism : CartScript
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Teleportable")
        {
            other.gameObject.transform.SetParent(cart.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Teleportable")
            other.gameObject.transform.SetParent(null);
    }

}
