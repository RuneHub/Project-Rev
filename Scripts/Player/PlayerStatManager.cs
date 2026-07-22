using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KS { 
    public class PlayerStatManager : CharacterStatsManager
    {
        PlayerManager player;

        public float PerfectTimingBuff = 45;

        List<GameObject> charColl;

        [Header("Just Dodge")]
        public Collider justDodgeCollider;

        [SerializeField] float JDTimer;
        [SerializeField] float defaultJDTime = .2f;
        [SerializeField] private bool JDActiveTime;
        [SerializeField] LayerMask JDLayer;
        public float hitboxRadius = 1.01f;
        [SerializeField] Collider[] JDHitboxes = new Collider[10];

        [Header("Hitstop effect")]
        public float hitStopDuration = .3f;
        public float hitStopMagnitude = .25f;
        public float CamerashakeDuration = .15f;
        public float CamerashakeMagnitude = .2f;
        public float hitStopAnimationSpeedMultiplier = .1f;
        public float gamepadRumbleLowFreq = 0.25f;
        public float gamepadRumbleHighFreq = 1f;
        public float gamepadRumbleDuration = .2f;

        [Header("Healing")]
        public int smallhealingAmount = 3;
        public float smallHealingPercentage = 25f;
        public float smallHealCharge = 0;
        public bool smallHealCharging;
        [Space]
        public int LargeHealingAmount = 1;
        public float largeHealCharge = 0;
        public bool LargeHealCharging;
        [Space]
        public int regenHealingAmount = 3;
        public float regenHealingPercentage = 5f;
        public float regenHealingActiveTime = 15f;
        public bool regenTimeActive;
        public float regenHealcharge = 0;
        public bool regenHealCharging;
        [Space]
        public int ReviveAmount = 1;
        public bool ReviveUsed = false;
        public float ReviveCharge = 0;
        public bool ReviveCharging;
        [Space]
        public float healingChargeSpeed = 0.25f;
        public float maxHealingCharge = 100;
        public bool healingCharged = false;
        public bool healCharging = false;

        [Header("Passive ability texts")]
        public PassiveAbilityText passive1;
        public PassiveAbilityText passive2;

        protected override void Awake()
        {
            base.Awake();

        }

        protected override void Start()
        {
            base.Start();
            player = GetComponent<PlayerManager>();

            UIManager.instance.SetupHUD();

            GetAllHurtBoxColliders();
        }

        protected override void CheckStatus()
        {
            base.CheckStatus();
        }

        protected override void Update()
        {
            base.Update();

            if (!LargeHealCharging && largeHealCharge > 0)
            {
                largeHealCharge -= healingChargeSpeed;
                if (largeHealCharge <= 0) 
                {
                    largeHealCharge = 0;
                }
            }
                
            if (!smallHealCharging && smallHealCharge > 0)
            {
                smallHealCharge -= healingChargeSpeed;
                if (smallHealCharge <= 0)
                {
                    smallHealCharge = 0;
                }
            }

            if (!regenHealCharging && regenHealcharge > 0)
            {
                regenHealcharge -= healingChargeSpeed;
                if (regenHealcharge <= 0)
                {
                    regenHealcharge = 0;
                }
            }

            HandleJDCollision();
        }

        #region Death & Damage
        protected override void HandleDeath()
        {
            base.HandleDeath();


            player.playerAnimations.PlayTargetAnimation("Death", true, layerNum: 1);


            if (ReviveUsed)
            {
                player.inputs.DisableGameplayInput();
                UIManager.instance.popupManager.SendQuestFailedPopup();
            }
            else
            {
                UIManager.instance.hudManager.SetReviveMaskOff();
            }
            
        }

        public override void TakeDamage(float damage, bool isCrit, Color displayColor, float angledContact, DamageProperties property)
        {

            if (hasArmor)
            {
                damage = damage * 0.5f;
            }

            base.TakeDamage(damage, isCrit, displayColor, angledContact, property);

            player.cameraHandler.EffectShake(CamerashakeDuration, CamerashakeMagnitude);
            ScreenManager.instance.StartFullscreenDamageSequence();

            player.hudManager.DamageShakePortrait();

            player.inputs.GamepadRumble(gamepadRumbleLowFreq, gamepadRumbleHighFreq, gamepadRumbleDuration);

            player.UltManager.ChangeBarAmount(PlayerUltimateAttackManager.BarSource.Damage);

            Debug.Log("hit");
            Debug.Log("Player damaged for: " + damage);
            //Debug.Log("angle: " + angledContact)

            if (!player.isDead)
            {
                if (!hasArmor)
                {
                    player.animator.SetBool("isInvulnerable", true);
                    player.animator.SetBool("isDamaged", true);

                    player.effectManager.CharacterShake(hitStopDuration, hitStopMagnitude);
                    player.playerAnimations.AdjustAnimationSpeed("HitStopSpeed", hitStopAnimationSpeedMultiplier, hitStopDuration);

                    if (property == DamageProperties.Normal)
                    {

                        if (angledContact >= -90 && angledContact <= 90)
                        {
                            //front
                            player.playerAnimations.PlayTargetAnimation("Damage_Front", true, layerNum: 2);
                        }
                        else if (angledContact >= -90 && angledContact >= 90)
                        {
                            //back
                            player.playerAnimations.PlayTargetAnimation("Damage_Back", true, layerNum: 2);
                        }
                        else
                        {
                            float ran = Random.Range(0, 1);
                            if (ran == 1)
                            {
                                player.playerAnimations.PlayTargetAnimation("Damage_Front", true, layerNum: 2);
                            }
                            else
                            {
                                player.playerAnimations.PlayTargetAnimation("Damage_Back", true, layerNum: 2);
                            }
                        }

                    }
                    else if (property == DamageProperties.Launcher)
                    {
                        if (angledContact >= -90 && angledContact <= 90)
                        {
                            //front
                            Debug.Log("front");
                            player.playerAnimations.PlayTargetAnimation("Launcher_Up", true, layerNum: 2);
                        }
                        else if (angledContact >= -90 && angledContact >= 90)
                        {
                            //back
                            Debug.Log("back");
                            player.playerAnimations.PlayTargetAnimation("Launcher_Spin", true, layerNum: 2);
                        }
                        else
                        {
                            float ran = Random.Range(0, 1);
                            if (ran == 1)
                            {
                                player.playerAnimations.PlayTargetAnimation("Launcher_Up", true, layerNum: 2);
                            }
                            else
                            {
                                player.playerAnimations.PlayTargetAnimation("Launcher_Spin", true, layerNum: 2);
                            }
                        }

                    }
                    else if (property == DamageProperties.Knockback)
                    {
                        player.playerAnimations.PlayTargetAnimation("Knockback", true, layerNum: 2);

                    }
                }
            }
        }

        public void HandleRecovery()
        {
            player.playerLocomotion.ResetAdditionalInteractionMovement();

            player.animator.SetBool("DamageRecover", true);
        }
        #endregion

        #region Healing
        //handles the actual charging part of using the healing items.
        public void HandleHealingCharge()
        {
            healCharging = true;

            if (LargeHealCharging)
            {
                largeHealCharge += healingChargeSpeed;
                if (largeHealCharge > maxHealingCharge)
                {
                    largeHealCharge = maxHealingCharge;
                    healingCharged = true;
                }
            }
            else if (smallHealCharging)
            {
                smallHealCharge += healingChargeSpeed;
                if (smallHealCharge > maxHealingCharge)
                {
                    smallHealCharge = maxHealingCharge;
                    healingCharged = true;
                }
            }
            else if (regenHealCharging)
            {
                regenHealcharge += healingChargeSpeed;
                if (regenHealcharge > maxHealingCharge)
                {
                    regenHealcharge = maxHealingCharge;
                    healingCharged = true;
                }
            }
            else if (ReviveCharging)
            {
                ReviveCharge += healingChargeSpeed;
                if (ReviveCharge > maxHealingCharge)
                {
                    ReviveCharge = maxHealingCharge;
                    healingCharged = true;
                }
            }

        }    

        //performs the actual healing, has the actual math equation for the healing amount.
        //reduces the item amount.
        public void HandleHealing(float healingAmountPercentage,bool healFully)
        {
            if (currentHealth <= 0)
                return;

            if (player.isDead)
                return;

            Debug.Log("Healed!");
            healingCharged = false;
            healCharging = false;
            if (healFully)
            {
                if (LargeHealCharging && LargeHealingAmount > 0)
                {
                    LargeHealingAmount--;
                    currentHealth = maxHealth;
                    //activate Big Heal vfx
                    player.effectManager.BigHealEffect();

                    largeHealCharge = 0; 
                    UIManager.instance.hudManager.UpdateHealingText();

                    if (LargeHealingAmount == 0)
                    {
                        largeHealCharge = maxHealingCharge;
                    }
                }

                
            }
            else
            {
                if (smallHealCharging)
                { 
                    if (smallhealingAmount > 0)
                    {
                        smallhealingAmount--;
                        float healingAmount = ((maxHealth / 100) * healingAmountPercentage);
                        currentHealth += healingAmount;
                        //activate small heal vfx
                        player.effectManager.smallHealEffect();

                        smallHealCharge = 0;
                        UIManager.instance.hudManager.UpdateHealingText();

                        if (smallhealingAmount == 0)
                        {
                            smallHealCharge = maxHealingCharge;
                        }
                    }
                }

                if (regenHealCharging)
                {
                    if (regenHealingAmount > 0)
                    {
                        regenHealingAmount--;
                        startRegenHeal();
                        regenHealcharge = 0; 
                        UIManager.instance.hudManager.UpdateHealingText();

                        if (regenHealingAmount == 0)
                        {
                            regenHealcharge = maxHealingCharge;
                        }

                    }
                }
            }

            

        }

        //handles the revive, sets the use of the revive to 0 & updates the UI.
        //turns the dead bools off, enables gameplay inputs & starts the revive animation.
        public void HandleRevive()
        {

            healingCharged = false;
            healCharging = false;
            if (ReviveCharging)
            {
                if (!ReviveUsed)
                {
                    StartCoroutine(ActiveRevival());

                }
            }

        }

        private IEnumerator ActiveRevival()
        {
            player.effectManager.ReviveEffect();

            yield return new WaitForSeconds(8);

            ReviveUsed = true;
            ReviveAmount = 0;
            UIManager.instance.hudManager.UpdateHealingText();

            currentHealth = maxHealth;

            ReviveCharge = maxHealingCharge;

            Debug.Log("REVIVED!");
            player.isDead = false;
            player.isInteracting = false;

            player.inputs.EnableGameplayInput();
            HandleRecovery();
            player.playerAnimations.PlayTargetAnimation("Revive", true, true, 0, 1, 0);
        }

        //stops the regen & restarts the coroutine.
        private void startRegenHeal()
        {
            regenTimeActive = true;
            StopCoroutine(ActiveRegen());

            StartCoroutine(ActiveRegen());
        }

        //whilst the timer is on a heal will happen every tick of the regen time.
        //uses the regular healing equation.
        private IEnumerator ActiveRegen()
        {
            float tempTime = regenHealingActiveTime;
            while (regenTimeActive)
            {
                yield return new WaitForSeconds(1);
                tempTime--;
                //heal
                float healingAmount = ((maxHealth / 100) * regenHealingPercentage);
                currentHealth += healingAmount;
                //active regen heal vfx
                player.effectManager.regenHealEffect();

                if (tempTime < 0)
                { 
                    regenTimeActive = false;
                }
            }
        }

        #endregion

        #region Hurtboxes
        //get all the colliders under the model object,
        //remove the colliders that have not been tagged as "Hurtbox"
        private void GetAllHurtBoxColliders()
        {
            charColl = new List<GameObject>(GetChildrenColliders.GetAllChildrenColliders(player.combatAnimationEvents.gameObject));

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

        public void Setvulnerability(bool setOff)
        {
            if (setOff)
            {
                //for loop through all children
                for (int i = 0; i < charColl.Count; i++)
                {
                    //turn colliders off
                    charColl[i].GetComponent<Collider>().enabled = false;
                }
            }
            else if (!setOff)
            {
                for (int i = 0; i < charColl.Count; i++)
                {
                    //turn colliders on
                    charColl[i].GetComponent<Collider>().enabled = true;
                }
            }
        }
        #endregion

        #region Just Dodge
        public void ActivateJD(bool useDefaultTime = true, float otherTime = 0)
        {
            JDActiveTime = true;
            justDodgeCollider.GetComponent<Collider>().enabled = true;
            if (useDefaultTime)
            {
                JDTimer = defaultJDTime;
            }
            else
            {
                JDTimer = otherTime;
            }
        }

        //JD = Just dodge,
        private void HandleJDCollision()
        {
            if (JDActiveTime)
            {
                if (JDTimer > 0)
                {
                    JDTimer -= Time.deltaTime;
                    ProcessJD();
                }
                else
                {
                    JDActiveTime = false;
                    justDodgeCollider.GetComponent<Collider>().enabled = false;
                    JDTimer = 0;
                    JDHitboxes = null;
                }
            }
        }

        private void ProcessJD()
        {
            JDHitboxes = Physics.OverlapCapsule(transform.position, Vector3.up, hitboxRadius, JDLayer);

            for (int i = 0; i < JDHitboxes.Length; i++)
            {
                if (JDHitboxes[i].GetComponent<BaseDamageCollider>() != null)
                {
                    if (JDHitboxes[i].GetComponent<BaseDamageCollider>().Targets == TargetTypes.Player)
                    {
                        Vector3 delta = (JDHitboxes[i].transform.position - transform.position).normalized;
                        Vector3 cross = Vector3.Cross(delta, transform.forward).normalized;
                        
                        Debug.Log("just dodge");
                        JDActiveTime = false;
                        player.JustDodge = true;
                        player.playerLocomotion.HandleJustDodge(cross);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Status Effects
        public override void AddStatusEffect(StatusEffectsSO adding)
        {
            base.AddStatusEffect(adding);
            UIManager.instance.hudManager.HandleStatusEffects(adding);
        }

        public override void RemoveStatusEffect(StatusEffectsSO removing)
        {
            base.RemoveStatusEffect(removing);
            UIManager.instance.hudManager.RemoveStatusEffectIcon(removing);
        }
        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.color.WithAlpha(0);
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + .5f, transform.position.z);
            Gizmos.DrawWireSphere(pos, hitboxRadius);
        }
    }

}
