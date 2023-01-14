using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    public void Use(GameObject caster,Vector3 dir);
    public void updateCd();
}
