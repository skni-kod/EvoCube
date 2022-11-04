using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;



public class Chunk : MonoBehaviour
{
    private IMeshTopologyGenerator _meshTopologyGenerator;

    Vector3 _idd;
    Mesh mesh;
    MeshFilter meshFilter;
    MeshRenderer meshRenderer;

    [Inject]
    public void Construct(IMeshTopologyGenerator meshTopologyGenerator)
    {
        _meshTopologyGenerator = meshTopologyGenerator;
    }

    public void Start()
    {
        Reset();
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        mesh = new Mesh();
        mesh.name = "ChunkMesh";
        meshFilter.sharedMesh = mesh;
    }

    public void GenerateMeshTopology()
    {
        _meshTopologyGenerator.GenerateTopology(mesh, _idd);
    }

    void Reset()
    {

    }

    public class Pool : MonoMemoryPool<Chunk>
    {
        protected override void Reinitialize(Chunk chunk)
        {
            chunk.Reset();
            chunk.GenerateMeshTopology();
        }
    }
}
