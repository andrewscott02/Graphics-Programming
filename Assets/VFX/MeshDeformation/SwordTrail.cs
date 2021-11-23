using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SwordTrail : MonoBehaviour
{
    #region Mesh Generation

    [Header("Transforms")]
    public Transform parentTransform;
    public GameObject player;
    public GameObject swordBase;
    public GameObject swordPoint;
    public GameObject trailPoint;
    public GameObject desiredTrailPoint;
    public float lerpSpeed = 0.5f;

    private Vector3 offSet = new Vector3(17.6f, 14.0f, 23.8f);

    [Header("Mesh Generation")]
    [Range(3,20)]
    public int vertexCount;

    Mesh mesh;

    Vector3[] verts;
    int[] tris;

    MeshFilter meshFilter;
    MeshCollider meshCollider;

    #endregion

    #region Material

    [Header("Material")]
    public Gradient gradient;

    Vector2[] uvs;

    Color[] colours;


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        if (meshFilter != null)
        {
            meshFilter.mesh = mesh;
        }

        if (meshCollider != null)
        {
            meshCollider.sharedMesh = mesh;
        }

        CreateQuad();
        UpdateMesh();

        //ResetTransform();
    }

    private void Update()
    {
        CreateQuad();
        UpdateMesh();
        //ResetTransform();
        //parentTransform.position = new Vector3();
    }

    void CreateQuad()
    {
        #region Trail Point

        Vector3 newPos = new Vector3();

        newPos.x = Mathf.Lerp(trailPoint.transform.position.x, desiredTrailPoint.transform.position.x, Time.deltaTime * lerpSpeed);
        newPos.y = Mathf.Lerp(trailPoint.transform.position.y, desiredTrailPoint.transform.position.y, Time.deltaTime * lerpSpeed);
        newPos.z = Mathf.Lerp(trailPoint.transform.position.z, desiredTrailPoint.transform.position.z, Time.deltaTime * lerpSpeed);

        //Debug.Log(trailPoint.transform.position + " || " + desiredTrailPoint.transform.position);

        trailPoint.transform.position = newPos;

        #endregion

        #region Vertices

        verts = new Vector3[vertexCount];

        verts[0] = swordBase.transform.position;
        verts[1] = trailPoint.transform.position;
        verts[2] = swordPoint.transform.position;

        /*
        verts[vertexCount - 2] = trailPoint.transform.position;
        verts[vertexCount - 1] = swordPoint.transform.position;

        for (int i = 1; i < vertexCount - 2; i++)
        {
            verts[i].x = Mathf.Lerp(swordBase.transform.position.x, trailPoint.transform.position.x, Remap(i, 3, vertexCount - 1, 0, 1));
            verts[i].y = Mathf.Lerp(swordBase.transform.position.y, trailPoint.transform.position.y, Remap(i, 3, vertexCount - 1, 0, 1));
            verts[i].z = Mathf.Lerp(swordBase.transform.position.z, trailPoint.transform.position.z, Remap(i, 3, vertexCount - 1, 0, 1));
        }
        */
        #endregion

        #region Triangles

        tris = new int[3] { 0, 1, 2 };

        /*
        tris = new int[vertexCount * 6];

        for (int i = 0; i < vertexCount - 2; i++)
        {
            tris[i] = vertexCount - 1;
            tris[i + 1] = i;
            tris[i + 2] = i + 1;
        }
        */

        #endregion

        #region Material
        /*
        uvs = new Vector2[verts.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }
        }
        */
        #endregion
    }

    void UpdateMesh()
    {
        if (mesh != null)
        {
            mesh.Clear();

            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
    }

    private void OnDrawGizmos()
    {
        if (verts != null)
        {
            for (int i = 0; i < verts.Length; i++)
            {
                Gizmos.DrawSphere(verts[i], 0.1f);
            }
        }
    }

    private void ResetTransform()
    {
        transform.position = new Vector3(17.6f, 14.0f, 23.8f);
        transform.eulerAngles = new Vector3(0, 0, 180);
        transform.localScale = new Vector3(0, 0, 0);
    }

    private float Remap(float value, float inMin, float inMax, float outMin, float outMax)
    {
        //https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Remap-Node.html
        return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
    }
}
