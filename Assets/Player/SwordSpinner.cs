using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSpinner : MonoBehaviour
{
    public float attackSpeed = 1f;
    public float returnSpeed = 1f;

    public float readyRot;
    public float endRot;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Quaternion newRot = transform.localRotation;

            newRot.z = Mathf.Clamp(Mathf.Lerp(newRot.z, endRot, Time.deltaTime * attackSpeed), 0, 170);

            //Debug.Log("Start spin sword | Old Rot = " + transform.rotation.z + " | New Rot = " + newRot.z + " |");

            transform.localRotation = newRot;
        }
        else
        {
            /*
            Quaternion newRot = transform.localRotation;

            newRot.z = Mathf.Clamp(Mathf.Lerp(newRot.z, readyRot, Time.deltaTime * returnSpeed), 0, 170);

            //Debug.Log("Revert spin sword | Old Rot = " + transform.rotation.z + " | New Rot = " + newRot.z + " |");

            transform.localRotation = newRot;
            */
        }

        if (transform.localRotation.z >= 170)
        {
            Debug.Log("change the rotation");
            Quaternion newRot = transform.localRotation;

            newRot.z = 170;

            transform.localRotation = newRot;
        }
    }
}
