using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Boss/Magic/Summon")]
    public class BossSummonSO : BossBaseSO
    {
        public GameObject buildUp;
        public GameObject projectile;
        public float ProjectileSpeed = 20f;
        public float ProjectileDelay = .5f;
    }
}