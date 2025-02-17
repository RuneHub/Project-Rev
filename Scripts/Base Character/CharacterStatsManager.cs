using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS { 
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("character stats")]
        public int Level = 1;
        public Color HUDDisplayColor = Color.white;

        [Header("Character status Stats")]
        public float maxHealth;
        public float currentHealth;
        [Range(1,100)] public float CurrentHealthPercantage;

        [Header("Character Combat Stats")]
        [Header("Attack")]
        public float baseAttack;
        public float CriticalHitRate; //always in %, the chance to get a critical hit.
        public float CriticalHitBuff; //always in %, the % that increases.

        [Header("Defense")]
        public float baseDefense = 10;

        [Header("Armor")]
        public bool hasArmor = false;
        public float maxArmor;
        public float currentArmor;

        [Header("min&max level stats")]
        public float maxLevelHealth;
        public float minLevelHealth;
        public float maxLevelAttack;
        public float minLevelAttack;

        [Header("invincibility")]
        public bool invincible;

        [Header("Damage")]
        public DamageProperties dmgProperties;
        public bool usesAngledDamage = false;

        [Header("Status Effects")]
        public List<StatusEffectsSO> statusEffects;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {
            CheckStatus();
        }

        protected virtual void CheckStatus()
        {
            maxHealth = StatCalculator.CalculateStats(Level, minLevelHealth, maxLevelHealth);
            baseAttack = StatCalculator.CalculateStats(Level, minLevelAttack, maxLevelAttack);

            currentHealth = maxHealth;
             
            if (hasArmor)
            {
                currentArmor = maxArmor;
            }

        }

        protected virtual void Update()
        {
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }

            if (invincible && currentHealth < 1)
            {
                currentHealth = 1;
            }

            if (hasArmor)
            {
                if (currentArmor < 0) 
                {
                    currentArmor = 0;
                }
            }

            CheckStatusEffects();
        }

        public virtual void CheckCurrentHPPercentage()
        {
            CurrentHealthPercantage = (currentHealth / maxHealth) * 100;
        }

        protected virtual void HandleDeath()
        {
            if(currentHealth < 0)
                currentHealth = 0;
        }

        public virtual void TakeDamage(float damage, bool isCrit, Color displayColor, float angledContact = 0, DamageProperties property = DamageProperties.Normal)
        {
            if (character.isInvulnerable)
                return;

            if (character.isDead)
                return;

            character.isHit = true;

            currentHealth = currentHealth - damage;

            FloatingUIManager.instance.DamageDone((int)damage, transform.position, isCrit, displayColor);

            if (currentHealth <= 0)
            {
                character.isDead = true;
                currentHealth = 0;
                HandleDeath();
            }

        }

        #region Status Effect
        //add status effect
        public virtual void AddStatusEffect(StatusEffectsSO adding)
        {
            statusEffects = StatCalculator.HandleStatusEffectListUpdate(statusEffects, adding);
            adding.Active = true;

            if (adding.useTime)
            {
                StartCoroutine(Countdown(adding));
            }

        }

        public virtual void CheckStatusEffects()
        {
            for (int i = 0; i < statusEffects.Count; i++)
            {
                //remove them if they are not active
                if (!statusEffects[i].Active && statusEffects[i].useTime)
                {
                    RemoveStatusEffect(statusEffects[i]);
                }

            }
        }

        //removes status effects
        public virtual void RemoveStatusEffect(StatusEffectsSO removing)
        {
            for (int i = 0; i < statusEffects.Count; i++)
            {
                if (statusEffects[i].statusEffectType == removing.statusEffectType &&
                    statusEffects[i].affectedStat == removing.affectedStat)
                {
                    //Debug.Log("removing: " + statusEffects[i]);
                    statusEffects.Remove(statusEffects[i]);
                }
            }
        }

        //clear the list of status effects
        public virtual void ClearAllStatusEffect()
        {
            statusEffects.Clear();
        }

        private IEnumerator Countdown(StatusEffectsSO se)
        {
            float counter = se.ActiveTime;

            float blinkTime = (counter / 100) * 25;

            while (counter > 0)
            {
                yield return new WaitForSeconds(1);

                if (counter < blinkTime)
                {
                    se.IconBlink = true;
                }

                counter--;
            }

            se.IconBlink = false;
            se.Active = false;

        }
        #endregion
    }
}
