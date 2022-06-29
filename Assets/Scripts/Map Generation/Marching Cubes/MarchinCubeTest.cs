using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MarchinCubeTest : MonoBehaviour
{
    public int size = 8;
    public int offsetX = 0;
    public int offsetY = 0;
    [SerializeField]MeshFilter meshFilter;
    Mesh mesh;
    [SerializeField] public Perlin2dSettings p2d;

    void Start()
    {
        mesh = MeshAPI.CreateMesh(size, 1);
        meshFilter.mesh = mesh;
        MeshAPI.EditVerticesValues(mesh, PerlinAPI.GPUPerlin2D(size + 1, new Vector2(offsetX, offsetY)));
        //MeshAPI.RebuildMeshAsync(mesh, 0.00001f);
    }

    void Update()
    {
        Regenerate();
    }
    
    void Regenerate()
    {
        float[] noise_data = PerlinAPI.GPUPerlin2D(size + 1, new Vector2(offsetX, offsetY));
        MeshAPI.RegenerateMesh(mesh, size, p2d.resolution, noise_data);
    }

}
