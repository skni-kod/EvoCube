using EvoCube.MapGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace EvoCube.Player
{
    public class Player : MonoBehaviour
    {
        DirectorsCamera playerCamera;

        [Inject] void Construct(DirectorsCamera directorsCamera, ITerrain terrain)
        {
            playerCamera = directorsCamera;
            playerCamera.transform.SetParent(transform, false);
            playerCamera.RegisterCamera("PlayerCamera");
            playerCamera.SetActive();

            terrain.SetTargetForGeneration(transform);
        }


    }

}
