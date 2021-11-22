using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GenerateMesh : MonoBehaviour
{
    #region Mesh Generation

    [Header("Mesh Generation")]
    public Transform parentTransform;
    public int xSize = 10;
    public int zSize = 10;

    Mesh mesh;

    Vector3[] verts;
    int[] tris;

    MeshFilter meshFilter;
    MeshCollider meshCollider;

    #endregion

    #region Waves

    [Header("Waves")]
    public float sineWaveCount, waveSpeed = 1;

    public float waveHeightMin, waveHeightMax;

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
        //UpdateMesh();

        parentTransform.position = new Vector3();
    }

    void CreateQuad()
    {
        verts = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int z = 0, i = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                Vector3 offset = new Vector3();
                offset.x = this.gameObject.transform.position.x;
                offset.y = this.gameObject.transform.position.y;
                offset.z = this.gameObject.transform.position.z;

                verts[i] = new Vector3(x + offset.x, 0 + offset.y, z + offset.z);
                i++;
            }
        }

        tris = new int[xSize * zSize * 6];

        int vertCount = 0;
        int triCount = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                tris[triCount + 0] = vertCount + 0;
                tris[triCount + 1] = vertCount + 1 + xSize;
                tris[triCount + 2] = vertCount + 1;
                tris[triCount + 3] = vertCount + 1;
                tris[triCount + 4] = vertCount + 1 + xSize;
                tris[triCount + 5] = vertCount + 2 + xSize;

                vertCount++;
                triCount += 6;
            }

            vertCount++;
        }
    }

    void Update()
    {
        mesh.Clear();

        if (mesh != null)
        {
            for (int i = 0; i < verts.Length; i++)
            {
                verts[i].y = Remap(Mathf.Sin(waveSpeed * (sineWaveCount * i + Time.time)), 0, 1, waveHeightMin, waveHeightMax);
            }

            mesh.vertices = verts;

            mesh.RecalculateBounds();

            meshCollider.sharedMesh = mesh;
        }

        mesh.triangles = tris;

        mesh.RecalculateNormals();
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

    private float Remap(float value, float inMin, float inMax, float outMin, float outMax)
    {
        //https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Remap-Node.html
        return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
    }
}