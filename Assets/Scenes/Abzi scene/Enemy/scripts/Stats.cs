using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EvoCube.EnemySystem
{
    [System.Serializable]
    public struct Stats
    {
        public float maxHp { get; set; }
        public float hp { get; set; }
        public float speed { get; set; }
        public Stats(float _maxHp, float _hp, float _speed)
        {
            maxHp = _maxHp;
            hp = _hp;
            speed = _speed;
        }
    }
}