using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBullet 
{
public void Construct (GameObject caster,Vector3 dir);
    public void updateLifeTime();
}
