using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Gun : IWeapon
{
    private float cd;
    public float maxCd;
    private Bullet.Factory _bulletFactory;
    
    [Inject]
    public Gun(Bullet.Factory bullet)
    {
        _bulletFactory = bullet;
        maxCd = 0.2f;
    }
    public void Use(GameObject caster,Vector3 dir)
    {
        if(cd<0)
        {
            _bulletFactory.Create(caster,dir);
            cd = maxCd;
        }
    }

 
    public void updateCd()
    {
        cd -= Time.deltaTime;
        
    }
}
