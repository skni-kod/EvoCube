using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MultiLegWalkerCode))]
public class Spider : Enemy
{
 
    void Update()
    {
        movment();
    }
    public override void movment()
    {
        base.movment();
        if (target != null) { GetComponent<MultiLegWalkerCode>().FaceDirection = target.transform.position; }
    }


}
