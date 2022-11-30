using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnerEnemy : MonoBehaviour
{
    public float timer = 5f;
    [Inject]
readonly Spider.Factory spiderFactory;
    public SpawnerEnemy(Spider.Factory factory)
    {
        spiderFactory = factory;
    }
    private void Update()
    {
        spawnSpider();
    }
    private void spawnSpider()
    {
        timer -= Time.deltaTime;
        if(timer<0)
        {
            timer = 5f;
            var enemy = spiderFactory.Create();
        }
    }
}
