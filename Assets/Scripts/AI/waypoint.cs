using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waypoint : MonoBehaviour
{
    public float timerDoAktywacji;
    private Vector3 oldPos;
    // Start is called before the first frame update
    void Start()
    {
        oldPos = Vector3.zero;
    }
    private void Update()
    {
                        if (timerDoAktywacji > 0) { timerDoAktywacji -= Time.deltaTime;return; }

        //  transform.position = transform.InverseTransformPoint(Vector3.zero);
        if (oldPos != transform.position) transform.position = GetFootPositionHit(transform.position, Vector3.up).point + Vector3.up * transform.localScale.y / 2;
        else
            timerDoAktywacji = 0.5f;


        //  timerDoAktywacji = 0.2f;
        oldPos = transform.position;
    }
    private RaycastHit GetFootPositionHit(Vector3 origin,Vector3 up)
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;

        RaycastHit hit;

        if (Physics.Raycast(origin, origin * -10f, out hit, Mathf.Infinity,layerMask))
        {
           
            return hit;
        }
        
        return hit;
    }

}
