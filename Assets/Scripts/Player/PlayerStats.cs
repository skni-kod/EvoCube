using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static float hp = 100;
    public static float speed = 1;
    public static float damage = 1;
    public static float food = 100;
    public static float hydrate = 100;

    [SerializeField]
    public float s_hp = 100;
    public float s_speed = 1;
    public float s_damage = 1;
    public float s_food = 100;
    public float s_hydrate = 100;

    private static PlayerStats instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    public static void set_hp(float new_hp)
    {
        if (hp <= 0)
        {
            Debug.Log("Dyntka");
        }
        hp = new_hp;
    }
    public static void set_speed(float new_speed)
    {
        speed = new_speed;
    }
    public static void set_damage(float new_damage)
    {
        damage = new_damage;
    }
    public static void set_food(float new_food)
    {
        food = new_food;
    }
    public static void set_hydrate(float new_hydrate)
    {
        hydrate = new_hydrate;
    }
}
