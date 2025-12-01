using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Player/Skills/Buff Skill")]
    public class PlayerBuffSkillSO : PlayerSkillsSO
    {
        public GameObject FX_BuffEffect;
        public float vfxDestroyTimer;
        public AudioClip BuffSFX;
        [Range(0, 1)] public float BuffSFXVolume = 1; 
        public bool useScreenShake;
        [DrawIf("useScreenShake", true)] public float shakeDuration;
        [DrawIf("useScreenShake", true)] public float shakeMagnitude;
        public List<StatusEffectsSO> statusEffects;

        public override void HandleSkill(PlayerManager owner, string position)
        {
            if (owner != null)
            {
                player = owner.GetComponent<PlayerManager>();
                SetUp();

                animEvents = owner.GetComponentInChildren<PlayerCombatAnimationEvents>();

                animatorOV["Anim_Combat_SkillPlaceHolder"] = animation;
                player.animator.runtimeAnimatorController = animatorOV;

                animEvents.OnSkillTriggered += PerformSkill;
                animEvents.OnFXTriggered += ActivateFX;
                animEvents.OnSkillDeactiveTriggered += CleanSkill;
                owner.playerAnimations.PlayTargetAnimation(position, true, useRootmotion, layerNum: 1);
            }
        }

        public override void SetUp()
        {
            if (skillID == "" || skillID == null)
            {
                Debug.LogError("Skill has no ID");
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

        private void ActivateFX(System.Object sender, EventArgs e)
        {
            if (FX_BuffEffect != null)
            {
                GameObject vfx = Instantiate(FX_BuffEffect);
                vfx.transform.position = player.transform.position;
                vfx.transform.rotation = player.transform.rotation;
                Destroy(vfx, vfxDestroyTimer);

            }

            if (BuffSFX != null) 
            {
                player.soundManager.PlayEffectSound(BuffSFX, BuffSFXVolume);
            }

        }

        private void PerformSkill(System.Object sender, EventArgs e)
        {

            if (useScreenShake)
            {
                player.cameraHandler.EffectShake(shakeDuration, shakeMagnitude);
            }

            if (statusEffects.Count == 0)
            {
                return;
            }

            for (int i = 0; i < statusEffects.Count; i++)
            {
                player.playerStats.AddStatusEffect(statusEffects[i]);
            }

            
        }

        private void CleanSkill(System.Object sender, EventArgs e)
        {
            animEvents.OnSkillTriggered -= PerformSkill;
            animEvents.OnFXTriggered -= ActivateFX;
            animEvents.OnSkillDeactiveTriggered -= CleanSkill;
        }

    }
}