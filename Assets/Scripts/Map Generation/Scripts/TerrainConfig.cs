using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class TerrainConfig : ScriptableObject
{
    [SerializeField] public static int seed = 0;
    [SerializeField] public static int chunkSize = 64;
    [SerializeField] public static int kernelNumber = 8;
    //public static Perlin2dSettings p2d_to_set = ScriptableObject.CreateInstance<Perlin2dSettings>();
}
