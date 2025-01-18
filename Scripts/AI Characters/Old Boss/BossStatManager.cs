using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace KS
{
    public class BossStatManager : CharacterStatsManager
    {
        BossManager boss;

        protected override void Awake()
        {
            base.Awake();
            boss = GetComponent<BossManager>();
        }

        protected override void Start()
        {
            base.Start();
            boss.uiManager.SetUpBossVitality();
        }

        protected override void CheckStatus()
        {
            base.CheckStatus();
        }

        protected override void Update()
        {
            base.Update();

        }

        protected override void HandleDeath()
        {
            base.HandleDeath();

            boss.behaviourRunner.enabled = false;
            boss.animationManager.PlayTargetAnimation("Death", true, layerNum: 2);
        }

        public override void TakeDamage(float damage, bool isCrit, Color displayColor, float angledContact, DamageProperties property)
        {
            base.TakeDamage(damage, isCrit, displayColor, angledContact, property);

            if (boss.isDead)
            {
                boss.animationManager.PlayTargetAnimation("GetHit", false, layerNum: 1);
            }
        }

    }
}