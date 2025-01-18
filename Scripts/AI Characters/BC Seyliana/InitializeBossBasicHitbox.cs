using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class InitializeBossBasicHitbox : MonoBehaviour
    {

        public AIBossManager boss;

        private void Start()
        {
            boss = FindAnyObjectByType<AIBossManager>();
            this.GetComponentInChildren<BaseDamageCollider>().
                Init(boss.combatAnimationEvents.DestroyHitbox, 
                        boss, 
                        boss.statManager.baseAttack);
        }

    }
}