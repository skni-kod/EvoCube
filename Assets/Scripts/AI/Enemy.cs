using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform EnemyChar;
    public GameObject RagDoll;
    public GameObject Stick;
    public GameObject Leaf;
    [System.Serializable]
    public struct Stats
    {
        public float HP;
        public bool chill;
        public float provocationRadius;
        public List<GameObject> waypoints;
        [HideInInspector]
        public int aktualnyWaypoint;
    }
    
    public Stats stats;
    bool aliveFlag = true;
    // Start is called before the first frame update
    void Start()
    {
        EnemyChar = GetComponent<Transform>();
        if (stats.chill) { stats.provocationRadius = -1; }
        if (stats.waypoints.Count == 0) { stats.aktualnyWaypoint = -1; }
    }

    // Update is called once per frame
    void Update()
    {
        if(checkEnemyAround()!=null)
        {
            GetComponent<MultiLegWalkerCode>().FaceDirection =checkEnemyAround().transform.position;
        }
        else
        {
            if(stats.aktualnyWaypoint==-1)
            GetComponent<MultiLegWalkerCode>().FaceDirection = transform.position;
            else
            {
                GetComponent<MultiLegWalkerCode>().FaceDirection = stats.waypoints[stats.aktualnyWaypoint].transform.position;
                if(Vector3.Distance(transform.position, stats.waypoints[stats.aktualnyWaypoint].transform.position)<2f)
                {
                    Debug.Log("zmieniam checkpointa");
                    stats.aktualnyWaypoint = stats.aktualnyWaypoint + 1 >= stats.waypoints.Count ? 0 : stats.aktualnyWaypoint + 1;
                }
            }
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.collider.tag == "Bullet")
    //    {
    //        Destroy(gameObject);
    //    }
    //}



    public GameObject checkEnemyAround()
    {
        if(GameObject.Find("cookie") != null && Vector3.Distance(GameObject.Find("cookie").transform.position,transform.position)<=stats.provocationRadius )
        {
            return GameObject.Find("cookie");
        }
        return null;
    }
    public void TakeDamage(float damage)
    {
        stats.HP -= damage;
        GetComponentInChildren<EnemyHpUI>().UpdateHp(stats.HP);
        if (stats.HP < 0 && aliveFlag)
        {
            stats.HP = 0;
            aliveFlag = false;
            //CreateLoot();
            Destroy(EnemyChar.gameObject);
            Debug.Log("Killed");
            Instantiate(RagDoll.transform, gameObject.transform.position, gameObject.transform.rotation);
        }
    }
    public void OnDrawGizmos()
    {
        if (stats.provocationRadius <= 0) { return; }
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stats.provocationRadius);
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
