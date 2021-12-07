using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshAPI : MonoBehaviour
{
    private static MeshAPI instance = null;
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static Mesh CreateMesh(int size, float resolution)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = CreateVertices(size + 1, resolution);
        mesh.triangles = CalculateTriangles(size);
        mesh.RecalculateNormals();
        return mesh;
    }

    public static void RebuildMeshAsync(Mesh mesh, float time)
    {
        CalculateTrianglesAsync(mesh, (int)Mathf.Sqrt(mesh.triangles.Length/6), time);
    }

    public static void ResizeMesh(Mesh mesh, int size, float resolution)
    {
        mesh.vertices = CreateVertices(size, resolution);
        mesh.triangles = CalculateTriangles(size);
        mesh.RecalculateNormals();
    }

    public static void RegenerateMesh(Mesh mesh, int size, float resolution, float[] data)
    {
        mesh.vertices = CreateVertices(size + 1, resolution, data);
        mesh.triangles = CalculateTriangles(size);
        mesh.RecalculateNormals();
    }

    public static void EditVerticesValues(Mesh mesh, float[] data)
    {
        mesh.vertices = CreateVertices((int)Mathf.Sqrt(mesh.vertices.Length), mesh.vertices[1].x, data);
        mesh.RecalculateNormals();
    }

    public static Vector3[] CreateVertices(int size)
    {
        return CreateVertices(size, 1);
    }

    public static Vector3[] CreateVertices(int size, float resolution)
    {
        Vector3[] vertices = new Vector3[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                vertices[x + (y * size)] = new Vector3(x*resolution, 0, y*resolution);
            }
        }
        return vertices;
    }

    public static Vector3[] CreateVertices(int size, float resolution, float[] data)
    {
        Vector3[] vertices = new Vector3[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                vertices[x + (y * size)] = new Vector3(x * resolution, data[x + y * size], y * resolution);
            }
        }
        return vertices;
    }

    public static int[] CalculateTriangles(int size)
    {
        int idx;
        int tris = 0;
        int[] triangles = new int[size * size * 6];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                idx = x + (y * (size + 1));

                triangles[tris++] = idx + 1;
                triangles[tris++] = idx;
                triangles[tris++] = idx + (size + 1);
                triangles[tris++] = idx + 1 + (size + 1);
                triangles[tris++] = idx + 1;
                triangles[tris++] = idx + (size + 1);
            }
        }
        return triangles;
    }

    public static int[] CalculateTrianglesFlat(int size)
    {
        int idx;
        int tris = 0;
        int[] triangles = new int[size * size * 6];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                idx = (x * 6) + (y * (size + 1)) * 6;
                triangles[tris++] = idx + 6 + 2;
                triangles[tris++] = idx + 0;
                triangles[tris++] = idx + (size + 1) * 6 + 4;
                triangles[tris++] = idx + 6 + (size + 1) * 6 + 1;
                triangles[tris++] = idx + 6 + 3;
                triangles[tris++] = idx + (size + 1) * 6 + 5;
            }
        }
        return triangles;
    }

    private static IEnumerator CalculateTrianglesAsyncIE(Mesh mesh, int size, float time)
    {
        int idx;
        int tris = 0;
        int[] triangles = new int[size * size * 6];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                idx = x + (y * (size + 1));

                triangles[tris++] = idx + 1;
                triangles[tris++] = idx;
                triangles[tris++] = idx + (size + 1);

                mesh.triangles = triangles;
                mesh.RecalculateNormals();
                yield return new WaitForSeconds(time);

                triangles[tris++] = idx + 1 + (size + 1);
                triangles[tris++] = idx + 1;
                triangles[tris++] = idx + (size + 1);

                mesh.triangles = triangles;
                mesh.RecalculateNormals();
                yield return new WaitForSeconds(time);

            }
        }
    }

    public static void CalculateTrianglesAsync(Mesh mesh, int size, float time)
    {
        instance.StartCoroutine(CalculateTrianglesAsyncIE(mesh, size, time));
    }

    private static IEnumerator CalculateTrianglesFlatAsyncIE(Mesh mesh, int size, float time)
    {
        int idx;
        int tris = 0;
        int[] triangles = new int[size * size * 6];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                idx = (x * 6) + (y * (size + 1)) * 6;

                triangles[tris++] = idx + 6 + 2;
                triangles[tris++] = idx + 0;
                triangles[tris++] = idx + (size + 1) * 6 + 4;

                mesh.triangles = triangles;
                mesh.RecalculateNormals();
                yield return new WaitForSeconds(time);

                triangles[tris++] = idx + 6 + (size + 1) * 6 +1;
                triangles[tris++] = idx + 6 + 3;
                triangles[tris++] = idx + (size + 1) * 6 + 5;

                mesh.triangles = triangles;
                mesh.RecalculateNormals();
                yield return new WaitForSeconds(time);

            }
        }
    }

    public static void CalculateTrianglesFlatAsync(Mesh mesh, int size, float time)
    {
        instance.StartCoroutine(CalculateTrianglesFlatAsyncIE(mesh, size, time));
    }

    public static Vector3[] CreateVerticesFlat(int size, float resolution, float[] data = null)
    {
        Vector3[] vertices = new Vector3[size * size * 6];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                for (int g = 0; g < 6; g++)
                {
                    if (data == null)
                        vertices[6*(x + (y * size)) + g] = new Vector3(x * resolution, 0, y * resolution);
                    else
                        vertices[6*(x + (y * size)) + g] = new Vector3(x * resolution, data[x + y * size], y * resolution);
                }
            }
        }
        return vertices;
    }


}
