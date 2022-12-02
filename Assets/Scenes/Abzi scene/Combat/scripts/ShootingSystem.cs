using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSystem : MonoBehaviour,IWeapon
{

    public virtual void Shoot(Transform tranShoter,float range)
    {
        raycast( tranShoter,  range);
    }
    private RaycastHit raycast(Transform tranShoter, float range)
    {
        RaycastHit hit;
        if (Physics.Raycast(tranShoter.position,tranShoter.forward, out hit, range))
        {
            Debug.Log(hit);
        }
        return hit;
    }
}
