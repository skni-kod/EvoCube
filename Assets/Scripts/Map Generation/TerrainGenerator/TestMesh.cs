using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMesh : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter;
    Mesh mesh;
    [SerializeField] public int size = 128;
    [SerializeField] public int offsetX;
    [SerializeField] public int offsetY;
    [SerializeField] public Perlin2dSettings p2d;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        meshFilter.mesh = mesh;

    }

    // Update is called once per frame
    void Update()
    {
        mesh.vertices = MeshAPI.CreateVerticesFlat(size + 1, 1, PerlinAPI.GPUPerlin2D(size + 1, p2d.seed + 1, new Vector2(offsetX, offsetY), p2d.gain, p2d.frequency, p2d.lacunarity, p2d.idk, p2d.type, p2d.octaves));

        mesh.triangles = MeshAPI.CalculateTrianglesFlat(size);
        mesh.RecalculateNormals();
        offsetX++;
    }
}
