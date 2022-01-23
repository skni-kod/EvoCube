using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public static class FindChunkIdsAroundAPI
{

    public static List<Vector3> FindChunksIdsAroundSquare(Vector3 focusPoint, int range, int size)
    {
        List<Vector3> foundChunks = new List<Vector3>();
        Vector3 center = new Vector3(
                                     Mathf.FloorToInt(focusPoint.x / size),
                                     Mathf.FloorToInt(focusPoint.y / size),
                                     Mathf.FloorToInt(focusPoint.z / size)
                                     );

        for (int x = -range; x < range;x++)
        {
            for (int y = -range; y < range; y++)
            {
                for (int z = -range; z < range; z++)
                {
                    Vector3 id = new Vector3(x, y, z) + center;
                    foundChunks.Add(id);
                }
            }
        }
        //foundChunks.Sort((v1, v2) => (v1 - center).sqrMagnitude.CompareTo((v2 - center).sqrMagnitude));
        return foundChunks;
    }


}
