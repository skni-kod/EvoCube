using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchinCubeTest : MonoBehaviour
{
    public int size = 8;
    [SerializeField]MeshFilter meshFilter;
    Mesh mesh;

    [SerializeField] float frequency = 0.02f;
    [SerializeField] float lacunarity = 2f;
    [SerializeField] int offsetX = 0;
    [SerializeField] int offsetY = 0;
    [SerializeField] float gain = 0.5f;
    [SerializeField] int seed = 0;
    [SerializeField] float resolution = 1f;
    [SerializeField] float idk = 1;
    [SerializeField] int type = 0;
    void Start()
    {
        mesh = MeshAPI.CreateMesh(size, 1);
        meshFilter.mesh = mesh;
        MeshAPI.EditVerticesValues(mesh, PerlinAPI.GPUPerlin2D(size + 1, seed + 1, new Vector2(offsetX, offsetY), gain, frequency, lacunarity, idk, type));
        //MeshAPI.RebuildMeshAsync(mesh, 0.00001f);
    }

    void Update()
    {
        Regenerate();
    }
    
    void Regenerate()
    {
        float[] noise_data = PerlinAPI.GPUPerlin2D(size + 1, seed + 1, new Vector2(offsetX, offsetY), gain, frequency, lacunarity, idk, type);
        MeshAPI.RegenerateMesh(mesh, size, resolution, noise_data);
    }

}
