using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSpinner : MonoBehaviour
{
    public float attackSpeed = 1f;

    public MeshRenderer trailRenderer;

    bool cancel = false;

    private void Start()
    {
        TurnRendererVisibility(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            //Vector3 newRot = transform.localRotation.eulerAngles;
            Vector3 newRot = new Vector3(0, 0, 0);

            newRot.z = Time.deltaTime * attackSpeed;

            //Debug.Log("Start spin sword | Old Rot = " + transform.rotation.z + " | New Rot = " + newRot.z + " |");

            transform.eulerAngles += newRot;

            TurnRendererVisibility(true);
            cancel = false;
        }
        else
        {
            cancel = true;
            StartCoroutine("turnFXOff", 0.3f);
        }
    }

    IEnumerator turnFXOff(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (cancel!)
        {
            TurnRendererVisibility(false);
        }
    }

    void TurnRendererVisibility(bool on)
    {
        if (trailRenderer != null)
        {
            trailRenderer.enabled = on;
        }
    }
}
