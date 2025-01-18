using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Player/Skills/Buff Skill")]
    public class PlayerBuffSkillSO : PlayerSkillsSO
    {

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

        private void PerformSkill(System.Object sender, EventArgs e)
        {

            if (statusEffects.Count == 0)
            {
                return;
            }

            for (int i = 0; i < statusEffects.Count; i++)
            {
                player.playerStats.AddStatusEffect(statusEffects[i]);
            }

            animEvents.OnSkillTriggered -= PerformSkill;
        }

    }
}