using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Player/Skills/Gap Closer Skill")]
    public class PlayerGapCloserSkillSO : PlayerSkillsSO
    {
        [Space(10)]
        public float baseDamage = 1f;
        public float movementSpeed = 15f;
        public float GapClosingTime = 1.5f;
        public float stopDistance = 2.5f;
        [Space(10)]
        public AnimationClip startUp;
        public bool startUpRootMotion;
        public AnimationClip Movement;
        [Space(10)]
        public BaseDamageCollider hitbox;
        public float hitboxKillTime;
        public Vector3 hitboxPlacement;
        [Space(10)]
        [Header("Effect Data")]
        public AudioClip releaseSFX;
        public bool useScreenShake;
        public float shakeDuration;
        public float shakeMagnitude;
        [Space(10)]
        public GameObject MovementVFX;
        public float moveVFXTime;

        private string position;

        public override void HandleSkill(PlayerManager owner, string _position)
        {
            player = owner.GetComponent<PlayerManager>();
            SetUp();

            position = _position;

            animatorOV["Anim_Combat_SkillPlaceHolder"] = animation;
            animatorOV["GapCloser_StartUp"] = startUp;
            animatorOV["GapCloser_Movement"] = Movement;
            player.animator.runtimeAnimatorController = animatorOV;

            combatManager.ResetCombo();
            combatManager.ResetCombatAnimations();

            player.combatManager.currentGPSkill = this;
            player.playerLocomotion.adMoveForce = movementSpeed;
            if (player.modeManager.currentMode == PlayMode.LockOnMode)
            {
                player.combatManager.curGPTime = GapClosingTime;
            }
            else if (player.modeManager.currentMode == PlayMode.FreeMode)
            {
                player.combatManager.curGPTime = GapClosingTime / 2;
            }

            animEvents = owner.GetComponentInChildren<PlayerCombatAnimationEvents>();
            animEvents.OnSkillTriggered += PerformSkill;
            animEvents.OnSkillDeactiveTriggered += CleanSkill;
            animEvents.OnFXTriggered += moveVFX;

            player.combatManager.GapClosing = true;
            player.playerAnimations.PlayTargetAnimation("GapCloser_StartUp", true, startUp, 0, 1, 0);
        }

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

        public void GapClosed()
        {
            player.combatManager.GapClosing = false;
            player.animator.SetBool("AdMovement", false);
            player.playerLocomotion.ResetAdditionalInteractionMovement();
            player.playerAnimations.PlayTargetAnimation(position, true, useRootmotion, layerNum: 1);
        }

        public void moveVFX(System.Object sender, EventArgs e)
        {
            GameObject fx = Instantiate(MovementVFX, player.transform);
            Destroy(fx, moveVFXTime);
        }

        public void PerformSkill(System.Object sender, EventArgs e)
        {
            //use SFX
            player.soundManager.PlayWeaponSound(releaseSFX);

            //use screenshake
            if(useScreenShake)
            {
                player.cameraHandler.EffectShake(shakeDuration, shakeMagnitude);
            }

            BaseDamageCollider _hitbox = Instantiate(hitbox, player.transform.position, player.transform.rotation, player.transform);
            _hitbox.transform.localPosition = hitboxPlacement;
            _hitbox.transform.parent = null;
            
            _hitbox.DestroyWithTime = true;
            _hitbox.DestroyTimer = hitboxKillTime;

            _hitbox.Init(DestroyHitbox, player, baseDamage);
            //Debug.LogError("Check");
        }

        public void CleanSkill(System.Object sender, EventArgs e)
        {
            animEvents.OnSkillTriggered -= PerformSkill;
            animEvents.OnSkillDeactiveTriggered -= CleanSkill;
            animEvents.OnFXTriggered -= moveVFX;
        }

        protected void DestroyHitbox(BaseDamageCollider obj)
        {
            Destroy(obj.gameObject);
        }

    }
}
