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
        //Assigns all of the mesh components
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

        ResetTransform();
    }

    private void Update()
    {
        UpdateMesh();
    }

    void CreateQuad()
    {
        /*
        Sources:
        Mesh Generation in Unity - https://www.youtube.com/watch?v=eJEpeUH1EMg
        Procedural Terrain in Unity - https://www.youtube.com/watch?v=64NblGkAabk
        */

        //Generates the vertices based on the z and x size of plane
        //Loops through all of the vetices and creates a vertex for each point, offsetting them to the transform of the parent object
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

        //Loops through all of the vertices and draws triangles based on the vertices
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

        //Assigns the UV coordinates for the surface, enabling the shader animations
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

            //Adjusts the y position of the vertices and maps them onto a sin wave, which creates a liquid wave effect
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

            //Sets the values for the mesh to the vertices and triangles generated here
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            mesh.uv = uvs;

            meshCollider.sharedMesh = mesh;
            meshCollider.sharedMesh.RecalculateBounds();
            meshCollider.sharedMesh.RecalculateNormals();

            //Loops through the vertices and dynamically adjusts the colours based on the height of the vertex
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

            mesh.colors = colours;
        }
    }

    #region Helper Functions

    private void OnDrawGizmosSelected()
    {
        if (verts != null)
        {
            for (int i = 0; i < verts.Length; i++)
            {
                Gizmos.DrawSphere(verts[i], 0.1f);
            }
        }
    }

    //Resets the trasnform of the mesh to the parent transform
    private void ResetTransform()
    {
        float y = this.gameObject.transform.position.y;

        parentTransform.position = new Vector3(0, y, 0);
    }

    //Remaps a value from one range to another
    private float Remap(float value, float inMin, float inMax, float outMin, float outMax)
    {
        //https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Remap-Node.html
        return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
    }

    #endregion
}
