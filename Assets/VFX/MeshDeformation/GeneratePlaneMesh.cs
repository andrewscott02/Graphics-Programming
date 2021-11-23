using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class GeneratePlaneMesh : MonoBehaviour
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

    float minTerrainHeight, maxTerrainHeight;

    #endregion

    #region Waves

    [Header("Waves")]
    public float sineWaveCount;
    public float waveSpeed = 1;
    public float waveHeightMin, waveHeightMax;

    #endregion

    #region Material

    [Header("Material")]
    public Gradient gradient;

    Vector2[] uvs;

    Color[] colours;


    #endregion

    #region Mesh Collision
    [Header("Mesh Collision")]
    public bool collide;

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

        ResetTransform();
    }

    private void Update()
    {
        //CreateQuad();
        UpdateMesh();

        //parentTransform.position = new Vector3();
    }

    void CreateQuad()
    {
        #region Vertices

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

        #endregion

        #region Triangles

        tris = new int[xSize * zSize * 6];

        int vertCount = 0;
        int triCount = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                tris[triCount + 0] = vertCount;
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

        #endregion

        #region Material

        uvs = new Vector2[verts.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
                i++;
            }
        }

        #endregion
    }

    void UpdateMesh()
    {
        if (mesh != null)
        {
            mesh.Clear();

            for (int i = 0; i < verts.Length; i++)
            {
                verts[i].y = Remap(Mathf.Sin(waveSpeed * (sineWaveCount * i + Time.time)), 0, 1, waveHeightMin, waveHeightMax);

                if (verts[i].y < minTerrainHeight)
                {
                    minTerrainHeight = verts[i].y;
                }
                if (verts[i].y > maxTerrainHeight)
                {
                    maxTerrainHeight = verts[i].y;
                }
            }

            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            mesh.uv = uvs;
            mesh.colors = colours;

            meshCollider.sharedMesh = mesh;
            meshCollider.sharedMesh.RecalculateBounds();
            meshCollider.sharedMesh.RecalculateNormals();

            colours = new Color[verts.Length];

            for (int i = 0, z = 0; z <= zSize; z++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    float height = Remap(verts[i].y, minTerrainHeight, maxTerrainHeight, 0, 1);
                    colours[i] = gradient.Evaluate(height);
                    i++;
                }
            }
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
        float y = this.gameObject.transform.position.y;

        parentTransform.position = new Vector3(0, y, 0);
    }

    private float Remap(float value, float inMin, float inMax, float outMin, float outMax)
    {
        //https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Remap-Node.html
        return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
    }
}
