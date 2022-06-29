using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;


class StaticMeshService
{
    [Inject]
    private IMeshCreatingService _meshCreatingService;

    public Vector3[] ResizePerlinVerticesDown(Vector3[] data)
    {
        if (_meshCreatingService == null)
            Debug.Log("XDD");
        return _meshCreatingService.ResizePerlinVerticesDown(data);
    }
    public int[] CalculateTrianglesFlat(int size)
    {
        return _meshCreatingService.CalculateTrianglesFlat(size);
    }


}

