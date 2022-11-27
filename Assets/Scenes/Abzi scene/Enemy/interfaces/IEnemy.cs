using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
   
    public void Movment();
    public void Attack();
    public void GetDmg(float dmg);
}
