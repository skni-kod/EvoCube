using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EvoCube.EnemySystem;
public class Enemy : MonoBehaviour,IEnemy
{
    public Stats stats;
    public float detectionRadius;
    public GameObject target;

    public virtual void Movment()
    {
        if(target==null && checkIfPlayerAround()!=null)
        {
            target = checkIfPlayerAround();
        }

    }
    public virtual void Attack()
    { 
    
    }
    public virtual void GetDmg(float dmg)
    {
        stats.hp -= dmg;
        if(stats.hp<0)
        { stats.hp = 0; }
        GetComponent<EnemyHpUI>().UpdateHp(stats.hp,stats.maxHp);
    }








    private GameObject checkIfPlayerAround() //only check if player is around 
    {
        if (!GetComponent<MultiLegWalkerCode>().enabled) { GetComponent<MultiLegWalkerCode>().enabled = true; }//if spider was chilling in place
        if (GameObject.FindGameObjectWithTag("Player") && Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) <= detectionRadius)
        {
            return GameObject.FindGameObjectWithTag("Player");
        }
        return null;
    }
    public void OnDrawGizmos()
    {
        if (detectionRadius <= 0) { return; }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}





