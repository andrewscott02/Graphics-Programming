using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRipples : MonoBehaviour
{
    public MeshFilter meshFilter;

    Mesh mesh;

    Vector3[] vertices;

    public MeshCollider meshCollider;

    public float sineWaveCount;

    public float waveHeightMin, waveHeightMax;

    public float maxRecorded, minRecorded;

    // Start is called before the first frame update
    void Start()
    {
        mesh = meshFilter.mesh;

        vertices = mesh.vertices;
    }

    private void Update()
    {
        if (mesh != null)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].y = Remap(Mathf.Sin(sineWaveCount * i + Time.time), 0, 1, waveHeightMin, waveHeightMax);

                if (vertices[i].y > maxRecorded)
                {
                    maxRecorded = vertices[i].y;
                }

                if (vertices[i].y < minRecorded)
                {
                    minRecorded = vertices[i].y;
                }
            }

            mesh.vertices = vertices;

            mesh.RecalculateBounds();

            meshCollider.sharedMesh = mesh;
        }
    }

    private float Remap(float value, float inMin, float inMax, float outMin, float outMax)
    {
        //https://docs.unity3d.com/Packages/com.unity.shadergraph@6.9/manual/Remap-Node.html
        return outMin + (value - inMin) * (outMax - outMin) / (inMax - inMin);
    }
}
