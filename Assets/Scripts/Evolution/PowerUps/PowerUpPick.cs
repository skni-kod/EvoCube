using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPick : MonoBehaviour
{
    public bool canBite = false;
    public bool canSpit = false;
    public bool canShoot = false;
    public bool canSprint = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "PowerBite")
        {
            Destroy(collision.gameObject);
            canBite = true;
        }
        if (collision.gameObject.name == "PowerSpit")
        {
            Destroy(collision.gameObject);
            canSpit = true;
        }
        if (collision.gameObject.name == "PowerSprint")
        {
            Destroy(collision.gameObject);
            canSprint = true;
        }
        if (collision.gameObject.tag == "HpUp")
        {
            Destroy(collision.gameObject);
            PlayerStats.set_hp(PlayerStats.hp + 1);
        }
        if (collision.gameObject.tag == "Base")
        {
            Debug.Log("Szllaag");
        }
    }
}
