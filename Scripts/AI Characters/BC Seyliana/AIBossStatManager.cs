using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

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

        [Space]
        [SerializeField] private CutsceneManager cutsceneManager;
        [SerializeField] private PlayableAsset QuestClearCS;
        [SerializeField] private float recoveryTime = 2f;

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

            //turn off behavbiour tree
            boss.behaviourRunner.enabled = false;
            //play animation
            boss.animator.SetBool("Staggered", true);
            boss.bossAnimations.PlayTargetAnimation("StaggerBreak", true, false, CrossFadeSpeed: 0, layerNum: 3, normalizedTime: 0);

            //start cutscene #4
            cutsceneManager.PlayCutscene(QuestClearCS);
        }
        
        public override void TakeDamage(float damage, bool isCrit, Color displayColor, float angledContact = 0, DamageProperties property = DamageProperties.Normal)
        {
            base.TakeDamage(damage, isCrit, displayColor, angledContact, property);

            //Debug.Log("Damage in %: " + ((currentHealth / maxHealth) * 100));
            boss.animator.SetBool("isDamaged", true);

            CalculateArmorDamage(damage);

            CheckCurrentHPPercentage();

            if (!boss.battleData.damageTrackerRunning)
            {
                boss.battleData.StartAccumuledDamage();
            }

            boss.animator.SetBool("isDamaged", false);
        }

        public override void TakeUltimateHit()
        {
            base.TakeUltimateHit();

            Debug.Log("Boss Took Ultimate Hit");

            //turn off behaviour
            boss.TurnOffBehaviour();

            //rotate to face player
            Vector3 lookTarget = new Vector3(boss.GetTarget().transform.position.x,
                                           boss.transform.position.y,
                                           boss.GetTarget().transform.position.z);
            boss.transform.LookAt(lookTarget);

            //turn off locomotion
            boss.bossLocomotion.enabled = false;
            
            // do break damage animation
            boss.animator.SetBool("Staggered", true);
            boss.bossAnimations.PlayTargetAnimation("StaggerBreak", true, layerNum: 3);
        
        }

        public override void RecoverUltimateHit()
        {
            base.RecoverUltimateHit();

            StartCoroutine(RecoveryTimer());

            Debug.Log("Boss Ultimate Hit Recover");

            boss.bossLocomotion.enabled = true;
            boss.bossAnimations.PlayTargetAnimation("Stagger Recovery", true, layerNum: 3);
            boss.TurnOnBehaviour();

        }

        private IEnumerator RecoveryTimer()
        {
            yield return new WaitForSeconds(recoveryTime);
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