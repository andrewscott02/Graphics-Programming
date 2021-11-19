using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Spawn particle effect
        //Ripple around the object
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered");

            //Slow player move speed
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Object left");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player left");
        }
    }
}