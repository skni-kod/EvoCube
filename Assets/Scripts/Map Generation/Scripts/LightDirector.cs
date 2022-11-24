using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDirector : MonoBehaviour, ILightDirector
{
    public void Initialize()
    {
        createGlobalLigthning();
    }

    private void createGlobalLigthning()
    {
        GameObject ligthObj = new GameObject("Ligth");
        ligthObj.transform.parent = transform;
        Light ligth = ligthObj.AddComponent<Light>();
        ligth.type = LightType.Directional;
        ligth.intensity = 0.8f;
    }


}
