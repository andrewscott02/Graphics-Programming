using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFollow : MonoBehaviour
{
    public Transform swordBase;
    public Transform swordPoint;
    public GameObject trailPoint;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3();

        newPos.x = Mathf.Lerp(trailPoint.transform.position.x, swordPoint.position.x, Time.deltaTime);
        newPos.y = Mathf.Lerp(trailPoint.transform.position.y, swordPoint.position.z, Time.deltaTime);
        newPos.z = Mathf.Lerp(trailPoint.transform.position.z, swordPoint.position.z, Time.deltaTime);

        trailPoint.transform.position = newPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(trailPoint.transform.position, 0.1f);
    }
}
