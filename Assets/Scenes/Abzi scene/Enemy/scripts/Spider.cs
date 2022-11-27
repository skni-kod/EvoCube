using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
[RequireComponent(typeof(MultiLegWalkerCode))]
public class Spider : Enemy
{
    [Inject]
    public virtual void Construct()
    {

    }

    void Update()
    {
        Movment();
    }
    public override void Movment()
    {
        base.Movment();
        if (target != null) { GetComponent<MultiLegWalkerCode>().FaceDirection = target.transform.position; }
    }

    public class Factory : PlaceholderFactory<Spider>
    {

    }



}
