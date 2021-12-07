using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBite : MonoBehaviour
{
    public PowerUpPick Pick;
    public GameObject Bite;
    public Collider attackHitboxes;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Bite.SetActive(Pick.canBite);
        if (Pick.canBite)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                LaunchBite(attackHitboxes);
                Debug.Log("Bite");
            }
        }
    }

    void LaunchBite(Collider col)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("HitBox"));
        foreach (Collider c in cols)
        {
/*            if (c.transform.parent.parent == transform) continue;
            Debug.Log(c.name);*/
            if (c.tag == "Enemy")
            {
                c.gameObject.GetComponent<Enemy>().TakeDamage(100);
            }
        }
    }
}
