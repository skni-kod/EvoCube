using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class MeshCreator : MonoBehaviour
{
    public MeshFilter meshFilter;
    public int size;
    public Vector3 scalesPerlin;
    public Vector2 perlinOffset;
    public Vector3 scalesPerlin2;
    public Vector2 perlinOffset2;
    [Range(0, 1)]
    public float ratio;
    public bool perlin2Squared;
    public bool perlin2Abs;
    public int power;
    void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

    }

    void Update()
    {
        if (meshFilter != null)
        {
            if (meshFilter.sharedMesh == null)
            {
                meshFilter.sharedMesh = CreateNewMesh(new Vector2(size, size));
            }
            else
            {
                meshFilter.sharedMesh = UpdateMesh(new Vector2(size, size), meshFilter.sharedMesh);
            }
        }

    }

    private void FixedUpdate()
    {

        if (Application.isPlaying)
        {
            perlinOffset2.x += 1f;
            perlinOffset.x += 5f;
        }
    }

    Mesh CreateNewMesh(Vector2 size)
    {
        Mesh mesh = new Mesh();
        mesh.name = "TestMesh";
        mesh.vertices = CreateVertices(size);
        mesh.triangles = CreateTriangles(size);
        return mesh;
    }

    Mesh UpdateMesh(Vector2 size, Mesh mesh)
    {
        mesh.vertices = CreateVertices(size);
        mesh.triangles = CreateTriangles(size);
        return mesh;
    }

    Vector3[] CreateVertices(Vector2 size)
    {
        Vector3[] vertices = new Vector3[(int)((size.x + 1) * (size.y + 1))];
        for (int y = 0; y <= size.y; y++)
        {
            for (int x = 0; x <= size.x; x++)
            {
                float perlin1 = ((Mathf.PerlinNoise((x / scalesPerlin.x) + perlinOffset.x / 10, (y / scalesPerlin.z) + perlinOffset.y / 10) - 0.5f) * scalesPerlin.y);
                float perlin2;
                if (perlin2Squared)
                    perlin2 = (Mathf.Pow(Mathf.PerlinNoise((x / scalesPerlin2.x) + perlinOffset2.x / 10, (y / scalesPerlin2.z) + perlinOffset2.y / 10) - 0.5f, power) * scalesPerlin2.y);
                else
                    perlin2 = ((Mathf.PerlinNoise((x / scalesPerlin2.x) + perlinOffset2.x / 10, (y / scalesPerlin2.z) + perlinOffset2.y / 10) - 0.5f) * scalesPerlin2.y);

                if (perlin2Abs)
                {
                    perlin2 = Mathf.Abs(perlin2);
                }
                float final = (perlin1 * ratio) + (perlin2 * (1f - ratio));
                vertices[x + (y * ((int)size.x + 1))] = new Vector3(x, final, y);
            }
        }
        
        return vertices;
    }

    int[] CreateTriangles(Vector2 size)
    {
        int count = 0;
        int[] triangles = new int[(int)(size.x * size.y * 2) * 6];
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                if (x != size.x && y != size.y)
                {
                    triangles[count++] = (x + (int)(y * (size.x + 1))) + 1;
                    triangles[count++] = (x + (int)(y * (size.x + 1)));
                    triangles[count++] = (x + (int)(y * (size.x + 1))) + (int)size.y + 1;

                    triangles[count++] = (x + (int)(y * (size.x + 1))) + (int)size.y + 2;
                    triangles[count++] = (x + (int)(y * (size.x + 1))) + 1;
                    triangles[count++] = (x + (int)(y * (size.x + 1))) + (int)size.y + 1;
                }

            }
        }
        return triangles;
    }

}
