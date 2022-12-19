using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
[RequireComponent(typeof(Rigidbody))][RequireComponent(typeof(SphereCollider))]
public class Bullet : MonoBehaviour,IBullet
{
    public float speed;
    public float dmg;
    public float lifeTime;
    Vector3 dir;
    [Inject]
    public void Construct(GameObject caster,Vector3 dir)
    {
        Physics.IgnoreCollision(GetComponent<SphereCollider>(), caster.GetComponent<CapsuleCollider>());
        transform.position = caster.transform.position + dir * 2;
        this.dir = dir;
        lifeTime = 5f;
    }
    public void updateLifeTime()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0) { Destroy(gameObject); }
    }
    private void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(dir * speed);
    }
    private void Update()
    {
        updateLifeTime();
    }
    public class Factory:PlaceholderFactory<GameObject ,Vector3 ,Bullet>
    {

    }
}
