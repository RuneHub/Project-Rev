using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AIBossStatManager : CharacterStatsManager
    {
        AIBossManager boss;

        [Header("HP Trigger attack")]
        public float HPTriggerAttack;
        public float HPTriggerMultiplier = 9;


        [Space(10), SerializeField] private float ArmorPercentage = 15;
        [SerializeField] private float armorDamageDivider = 15;

        [SerializeField] private List<GameObject> charColl;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            boss = GetComponent<AIBossManager>();

            GetAllHurtBoxColliders();
        }

        protected override void CheckStatus()
        {
            base.CheckStatus();

            HPTriggerAttack = baseAttack * HPTriggerMultiplier;
        }

        protected override void Update()
        {
            base.Update();

            CheckHPTriggerStatus();

        }

        private void CheckHPTriggerStatus()
        {
            if (boss.spendHPTrigger)
                return;

            if (boss.currentMode == BossMode.BreakMode)
                return;

            if (boss.isInteracting)
                return;
           
            CheckCurrentHPPercentage();

            if (CurrentHealthPercantage < boss.HpTriggerPercentage)
            {
                boss.SwapMode(BossMode.HPTriggerMode);
                boss.spendHPTrigger = true;
            }

        }

        #region Damage/Death
        protected override void HandleDeath()
        {
            base.HandleDeath();
        }
        
        public override void TakeDamage(float damage, bool isCrit, Color displayColor, float angledContact = 0, DamageProperties property = DamageProperties.Normal)
        {
            base.TakeDamage(damage, isCrit, displayColor, angledContact, property);

            //Debug.Log("Damage in %: " + ((currentHealth / maxHealth) * 100));
            boss.animator.SetBool("isDamaged", true);

            CalculateArmorDamage(damage);

            CheckCurrentHPPercentage();


            boss.animator.SetBool("isDamaged", false);
        }
        #endregion

        #region Armor

        public void CalculateArmorDamage(float damage)
        {
            float armorInPercentage = (maxHealth / 100) * ArmorPercentage;
            float armorDamageAmount = (armorInPercentage / maxHealth) * (damage / armorDamageDivider);
            RemoveArmor(armorDamageAmount);
        }

        public void RemoveArmor(float amount)
        {
            if (amount == 0)
                return;

            currentArmor -= amount;

            if (currentArmor <= 0 && boss.currentMode != BossMode.BreakMode) 
            {
                boss.SwapMode(BossMode.BreakMode);
                ScreenManager.instance.startShatter();
            }

        }

        public void AddArmour(float amount) 
        {
            currentArmor += amount;
        }

        #endregion

        #region Invuln & Colliders
        private void GetAllHurtBoxColliders()
        {
            charColl = new List<GameObject>(GetChildrenColliders.GetAllChildrenColliders(boss.combatAnimationEvents.gameObject));

            for (int i = 0; i < charColl.Count; i++)
            {
                if (charColl[i].tag != "Hurtbox")
                {
                    charColl.Remove(charColl[i]);
                }
            }

            if (charColl.Count == 0)
            {
                Debug.LogError("Found no colliders");
            }
        }

        public void InvulnON()
        {
            for (int i = 0; i < charColl.Count; i++)
            {
                charColl[i].GetComponent<Collider>().enabled = true;
            }
        }

        public void InvulnOFF()
        {
            for (int i = 0; i < charColl.Count; i++)
            {
                charColl[i].GetComponent<Collider>().enabled = false;
            }
        }
        #endregion

        #region Status Effects
        public override void AddStatusEffect(StatusEffectsSO adding)
        {
            base.AddStatusEffect(adding);
        }

        public override void CheckStatusEffects()
        {
            base.CheckStatusEffects();
        }

        public override void ClearAllStatusEffect()
        {
            base.ClearAllStatusEffect();
        }

        public override void RemoveStatusEffect(StatusEffectsSO removing)
        {
            base.RemoveStatusEffect(removing);
        }
        #endregion
    }
}