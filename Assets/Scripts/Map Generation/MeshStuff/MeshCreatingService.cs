using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IMeshCreatingService
{
    Vector3[] ResizePerlinVerticesDown(Vector3[] data);
    int[] CalculateTrianglesFlat(int size);
    Vector3[] CreateVerticesFlat(int size, float resolution, float[] data = null);
    Mesh CreateMesh(int size, float resolution);
    void EditVerticesValues(Mesh mesh, float[] data);
    void RegenerateMesh(Mesh mesh, int size, float resolution, float[] data);
}



public class MeshCreatingService : IMeshCreatingService
{
    public MeshCreatingService()
    {
        Debug.Log("lol");
    }


    public Mesh CreateMesh(int size, float resolution)
    {
        Mesh mesh = new Mesh();
        mesh.vertices = CreateVertices(size + 1, resolution);
        mesh.triangles = CalculateTriangles(size);
        mesh.RecalculateNormals();
        return mesh;
    }

    public void RebuildMeshAsync(Mesh mesh, float time)
    {
        CalculateTrianglesAsync(mesh, (int)Mathf.Sqrt(mesh.triangles.Length / 6), time);
    }

    public void ResizeMesh(Mesh mesh, int size, float resolution)
    {
        mesh.vertices = CreateVertices(size, resolution);
        mesh.triangles = CalculateTriangles(size);
        mesh.RecalculateNormals();
    }

    public void RegenerateMesh(Mesh mesh, int size, float resolution, float[] data)
    {
        mesh.vertices = CreateVertices(size + 1, resolution, data);
        mesh.triangles = CalculateTriangles(size);
        mesh.RecalculateNormals();
    }

    public void EditVerticesValues(Mesh mesh, float[] data)
    {
        mesh.vertices = CreateVertices((int)Mathf.Sqrt(mesh.vertices.Length), mesh.vertices[1].x, data);
        mesh.RecalculateNormals();
    }

    public Vector3[] CreateVertices(int size)
    {
        return CreateVertices(size, 1);
    }

    public Vector3[] CreateVertices(int size, float resolution)
    {
        Vector3[] vertices = new Vector3[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                vertices[x + (y * size)] = new Vector3(x * resolution, 0, y * resolution);
            }
        }
        return vertices;
    }

    public Vector3[] CreateVertices(int size, float resolution, float[] data)
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

    public int[] CalculateTriangles(int size)
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

    public int[] CalculateTrianglesFlat(int size)
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

    private IEnumerator CalculateTrianglesAsyncIE(Mesh mesh, int size, float time)
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

    public void CalculateTrianglesAsync(Mesh mesh, int size, float time)
    {
        //instance.StartCoroutine(CalculateTrianglesAsyncIE(mesh, size, time));
    }

    private IEnumerator CalculateTrianglesFlatAsyncIE(Mesh mesh, int size, float time)
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

                triangles[tris++] = idx + 6 + (size + 1) * 6 + 1;
                triangles[tris++] = idx + 6 + 3;
                triangles[tris++] = idx + (size + 1) * 6 + 5;

                mesh.triangles = triangles;
                mesh.RecalculateNormals();
                yield return new WaitForSeconds(time);

            }
        }
    }

    public void CalculateTrianglesFlatAsync(Mesh mesh, int size, float time)
    {
        //instance.StartCoroutine(CalculateTrianglesFlatAsyncIE(mesh, size, time));
    }

    public Vector3[] CreateVerticesFlat(int size, float resolution, float[] data = null)
    {
        Vector3[] vertices = new Vector3[size * size * 6];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                /*for (int g = 0; g < 6; g++)
                {
                    if (data == null)
                        vertices[6*(x + (y * size)) + g] = new Vector3(x * resolution, 0, y * resolution);
                    else
                        vertices[6*(x + (y * size)) + g] = new Vector3(x * resolution, data[x + y * size], y * resolution);
                    
                }*/
                vertices[x + (y * size)] = new Vector3(x * resolution, data[x + y * size], y * resolution);
            }
        }
        return vertices;
    }

    #region ThreadSafe Methods

    public Vector3[] ResizePerlinVerticesDown(Vector3[] data)
    {
        Debug.Log("Resizing...");
        int size = (int)Mathf.Sqrt(data.Length / 6);
        int small_size = size - PerlinAPI.N + 1;
        Vector3[] new_data = new Vector3[small_size * small_size * 6];
        int idx = 0;
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (x < small_size && y < small_size)
                {
                    for (int g = 0; g < 6; g++)
                    {
                        new_data[idx++] = data[(x * 6) + (y * size * 6) + g];
                    }
                }
            }
        }
        return new_data;

    }

    public float[] Resize2dArrayFlat(float[] array)
    {
        int bigger_size = (int)Mathf.Sqrt(array.Length / 6);
        int smaller_size = bigger_size - PerlinAPI.N + 1;
        float[] newArray = new float[smaller_size * smaller_size * 6];


        int idx = 0;
        for (int y = 0; y < bigger_size; y++)
        {
            for (int x = 0; x < bigger_size; x++)
            {
                if (x < smaller_size && y < smaller_size)
                {
                    for (int g = 0; g < 6; g++)
                    {
                        newArray[idx++] = array[(6 * x) + (6 * y) * (smaller_size) + g];
                    }
                }
            }
        }
        return newArray;
    }

    #endregion

}
