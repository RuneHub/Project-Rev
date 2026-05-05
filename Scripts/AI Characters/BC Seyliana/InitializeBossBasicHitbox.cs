using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class InitializeBossBasicHitbox : MonoBehaviour
    {

        public AIBossManager boss;
        public BossBaseSO SO;

        private void Start()
        {
            if (boss == null)
            {
                boss = FindAnyObjectByType<AIBossManager>();
            }

            this.GetComponentInChildren<BaseDamageCollider>().
                Init(boss.combatAnimationEvents.DestroyHitbox, 
                        boss,
                        StatCalculator.SkillAtkPowerCalculation(SO.baseDamage, boss.statManager.baseAttack));
        }

    }
}