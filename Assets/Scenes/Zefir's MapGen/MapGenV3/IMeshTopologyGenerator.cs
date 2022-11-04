using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeshTopologyGenerator
{
    void GenerateTopology(Mesh mesh, Vector3 idd);
}
