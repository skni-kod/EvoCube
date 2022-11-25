using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy 
{
   
    public void movment();
    public void attack();
    public void getDmg(float dmg);
}
