using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AIStatManager : CharacterStatsManager
    {
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void CheckStatus()
        {
            base.CheckStatus();
        }

        protected override void HandleDeath()
        {
            base.HandleDeath();
        }

        public override void TakeDamage(float damage, bool isCrit, Color displayColor, float angledContact = 0, DamageProperties property = DamageProperties.Normal)
        {
            base.TakeDamage(damage, isCrit, displayColor, angledContact, property);
        }


    }
}