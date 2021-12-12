using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform EnemyChar;
    public GameObject RagDoll;
    public GameObject Stick;
    public GameObject Leaf;
    public float HP = 140;
    bool aliveFlag = true;
    // Start is called before the first frame update
    void Start()
    {
        EnemyChar = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.collider.tag == "Bullet")
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    public void TakeDamage(float damage)
    {
        HP -= damage;
        GetComponentInChildren<EnemyHpUI>().UpdateHp(HP);
        if (HP < 0 && aliveFlag)
        {
            HP = 0;
            aliveFlag = false;
            //CreateLoot();
            Destroy(EnemyChar.gameObject);
            Debug.Log("Killed");
            Instantiate(RagDoll.transform, gameObject.transform.position, gameObject.transform.rotation);
        }
    }

    //public void CreateLoot()
    //{
    //    for(int i = 0; i<Random.value%3; i++)
    //    {
    //        var clone = Instantiate(Leaf.transform, EnemyChar.transform.position, EnemyChar.transform.rotation);
    //        clone.tag = "Leaf";
    //    }
    //    for (int i = 0; i < Random.value % 4; i++)
    //    {
    //        var clone = Instantiate(Stick.transform, EnemyChar.transform.position, EnemyChar.transform.rotation);
    //        clone.tag = "Stick";
            
    //    }

    //}

}
