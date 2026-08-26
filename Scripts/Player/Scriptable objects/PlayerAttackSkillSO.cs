using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

        public bool useReleaseVFX;
        [DrawIf("useReleaseVFX", true)] public GameObject releaseVFX;
        [DrawIf("useReleaseVFX", true)] public float destroyTimer;

        public bool useFxEffect;
        [DrawIf("useFxEffect", true)] public GameObject fxEffect;
        [DrawIf("useFxEffect", true)] public float fxEffectDestroyTimer;


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

                if (useReleaseVFX)
                {
                    animEvents.SetReleaseVFX(releaseVFX, destroyTimer);
                }

                if (useRaycast)
                {
                    skillAttack.rawDamage = baseDamage;
                    animEvents.OnShootEventTriggered += Shoot;
                }
                else if (useHitbox)
                {
                    animEvents.OnSkillTriggered += PerformSkill;
                }

                if(useFxEffect)
                {
                    animEvents.OnFXTriggered += ActivateFX;
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

            //use SFX
            for (int i = 0; i < skillSFXList.Count; i++)
            {
                player.soundManager.AddToSkillSFXList(skillSFXList[i]);
            }

        }

        protected void Shoot(System.Object sender, Transform output)
        {
            player.combatManager.ShootRaycastHitscan(output, skillAttack);

        }

        protected void PerformSkill(System.Object sender, EventArgs e)
        {
            //Debug.Log("perform skill");


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
                    _hitbox.AddComponent<PlayerColliderProperties>().Init(player);

                }
            }
        }

        private void ActivateFX(System.Object sender, EventArgs e)
        {
            if (fxEffect != null)
            {
                GameObject vfx = Instantiate(fxEffect);
                vfx.transform.position = player.transform.position;
                vfx.transform.rotation = player.transform.rotation;
                Destroy(vfx, fxEffectDestroyTimer);
                
            }
        }

        protected void CleanSkill(System.Object sender, EventArgs e)
        {
            player.soundManager.ClearSkillSFXList();

            animEvents.OnShootEventTriggered -= Shoot;
            animEvents.OnSkillTriggered -= PerformSkill;
            animEvents.OnSkillDeactiveTriggered -= CleanSkill;
            animEvents.OnFXTriggered -= ActivateFX;
        }

        protected void DestroyHitbox(BaseDamageCollider obj)
        {
            Destroy(obj.gameObject);
        }


    }
}