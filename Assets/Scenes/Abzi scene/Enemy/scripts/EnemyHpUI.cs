using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpUI : MonoBehaviour
{

    private float maxHp;  
    public GameObject hpBar;

    private void Update()
    {
        maxHp = transform.parent.GetComponent<Enemy>().stats.maxHp;
        rotateToPlayer();
    }
    private void rotateToPlayer()
    {
        gameObject.transform.forward = new Vector3(-GameObject.FindGameObjectWithTag("Player").transform.position.x + transform.position.x,-GameObject.FindGameObjectWithTag("Player").transform.position.y + transform.position.y, -GameObject.FindGameObjectWithTag("Player").transform.position.z + transform.position.z) ;
    }
    public void UpdateHp(float hp,float _maxHp)
    {
        maxHp = _maxHp;
        GetComponentInChildren<Text>().text = hp.ToString() + "/" + maxHp.ToString();
        hpBar.GetComponent<RectTransform>().localScale = new Vector3(hp / maxHp, hpBar.GetComponent<RectTransform>().localScale.y, hpBar.GetComponent<RectTransform>().localScale.z);
    }

}
