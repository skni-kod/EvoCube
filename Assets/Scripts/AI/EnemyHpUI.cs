using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpUI : MonoBehaviour
{
    private GameObject camera;
    private float maxHp;  
    [Header("Parent Need Enemy script!")]
    public GameObject hpBar;
    private void Start()
    {
        maxHp = GetComponentInParent<Enemy>().HP;
        camera = Camera.main.gameObject;
    }
    private void Update()
    {
        gameObject.transform.forward = new Vector3(-camera.transform.position.x + transform.position.x,- camera.transform.position.y + transform.position.y, -camera.transform.position.z + transform.position.z) ;
    }
    public void UpdateHp(float hp)
    {
        GetComponentInChildren<Text>().text = hp.ToString() + "/" + maxHp.ToString();
        hpBar.GetComponent<RectTransform>().localScale = new Vector3(hp / maxHp, hpBar.GetComponent<RectTransform>().localScale.y, hpBar.GetComponent<RectTransform>().localScale.z);
    }
}
