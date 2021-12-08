using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenTesting : MonoBehaviour
{
    public MapGenerationTester terrainReference;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;
    [SerializeField] public int size = 128;
    [SerializeField] public float offsetX;
    [SerializeField] public float offsetY;

    void Update()
    {
        meshFilter.mesh.vertices = MeshAPI.CreateVerticesFlat(size + 1, 1, PerlinAPI.GPUPerlin2D(size + 1, terrainReference.p2d.seed + 1, new Vector2(offsetX, offsetY), terrainReference.p2d.gain, terrainReference.p2d.frequency, terrainReference.p2d.lacunarity, terrainReference.p2d.idk, terrainReference.p2d.type, terrainReference.p2d.octaves));
        meshFilter.mesh.triangles = MeshAPI.CalculateTrianglesFlat(size);
        meshFilter.mesh.RecalculateNormals();
        offsetX += terrainReference.scrollingSpeed.x;
        offsetY += terrainReference.scrollingSpeed.y;
    }

    public void Init()
    {
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Mesh mesh = new Mesh();
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        meshRenderer.material = terrainReference.material;
        meshFilter.mesh = mesh;
        collider.sharedMesh = mesh;
    }
}
