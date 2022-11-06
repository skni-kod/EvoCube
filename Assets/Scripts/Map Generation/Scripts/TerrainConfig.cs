using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainConfig : MonoBehaviour
{
    public static int seed = 0;
    public static int chunkSize = 64;
    public static int kernelNumber = 8;
    public static Perlin2dSettings p2d_to_set = new Perlin2dSettings();
}
