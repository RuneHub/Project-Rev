using System;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Player/Skills/Attack Skill")]
    public class PlayerAttackSkillSO : PlayerSkillsSO
    {
        public float baseDamage = 1f;

        #region the skill options
        public bool useRaycast = false;
        [DrawIf("useRaycast", true)] public AttackStandardSO skillAttack;

        public bool useHitbox = false;
        public List<BaseDamageCollider> hitboxes;
        [DrawIf("useHitbox", true)] public float hitboxKillTime;
        [DrawIf("useHitbox", true)] public bool vectorPlacement;
        [DrawIf("vectorPlacement", true)] public Vector3 vectorLocation;

        public AudioClip releaseSFX;

        public bool useScreenShake;
        public float shakeDuration;
        public float shakeMagnitude;

        #endregion

        public override void HandleSkill(PlayerManager owner, string position)
        {
            if (owner != null)
            {
                player = owner.GetComponent<PlayerManager>();
                SetUp();

                //swap animation
                animatorOV["Anim_Combat_SkillPlaceHolder"] = animation;
                player.animator.runtimeAnimatorController = animatorOV;

                //resetting the animations & combo counter
                combatManager.ResetCombo();
                combatManager.ResetCombatAnimations();

                animEvents = owner.GetComponentInChildren<PlayerCombatAnimationEvents>();

                if (useRaycast)
                {
                    skillAttack.rawDamage = baseDamage;
                    animEvents.OnShootEventTriggered += Shoot;
                }
                else if (useHitbox)
                {
                    animEvents.OnSkillTriggered += PerformSkill;
                }

                animEvents.OnSkillDeactiveTriggered += CleanSkill;
                player.playerAnimations.PlayTargetAnimation(position, true, useRootmotion, layerNum: 1);

            }

        }

        //set up for data that came through
        public override void SetUp()
        {
            if (skillID == "" || skillID == null)
            {
                Debug.LogError("Skill " + skillName + " has no ID");
            }

            combatManager = player.combatManager;
            animatorOV = player.animatorOV;

            if (player == null)
            {
                Debug.LogError("No player manager found!");
            }

            if (animatorOV == null)
            {
                Debug.LogError("No animator Overide found!");
            }

        }

        protected void Shoot(System.Object sender, Transform output)
        {
            //Debug.Log("skill ray shoot");
            //player.combatManager.currentAttack = skillAttack;

            player.combatManager.ShootRaycastHitscan(output, skillAttack);

        }

        protected void PerformSkill(System.Object sender, EventArgs e)
        {
            //Debug.Log("perform skill");
            player.soundManager.PlayWeaponSound(releaseSFX);

            if (useScreenShake)
            {
                player.cameraHandler.EffectShake(shakeDuration, shakeMagnitude);
            }

            if (useHitbox)
            {
                for (int i = 0; i < hitboxes.Count; i++)
                {
                    BaseDamageCollider _hitbox = Instantiate(hitboxes[i]);

                    _hitbox.transform.position += player.transform.position;
                    _hitbox.transform.rotation = Quaternion.identity;
                    if (vectorPlacement)
                    {
                        _hitbox.transform.parent = player.transform;
                        _hitbox.transform.localPosition = vectorLocation;
                        _hitbox.transform.parent = null;
                    }

                    _hitbox.DestroyWithTime = true;
                    _hitbox.DestroyTimer = hitboxKillTime;

                    _hitbox.Init(DestroyHitbox, player, baseDamage);
                }
            }
        }

        protected void CleanSkill(System.Object sender, EventArgs e)
        {
            animEvents.OnShootEventTriggered -= Shoot;
            animEvents.OnSkillTriggered -= PerformSkill;
            animEvents.OnSkillDeactiveTriggered -= CleanSkill;
        }

        protected void DestroyHitbox(BaseDamageCollider obj)
        {
            Destroy(obj.gameObject);
        }


    }
}