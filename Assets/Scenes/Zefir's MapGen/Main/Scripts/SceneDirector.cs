using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SceneTypes
{
    Default,
    UI,
    Gameplay,
    Core,
}

public class SceneDirector : MonoBehaviour
{
    [SerializeField] public SceneTypes sceneType;
}
