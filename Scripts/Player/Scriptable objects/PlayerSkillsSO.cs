using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS
{
    
    public abstract class PlayerSkillsSO : ScriptableObject
    {
        protected PlayerManager player;
        protected PlayerCombatManager combatManager;
        protected PlayerCombatAnimationEvents animEvents;
        protected AnimatorOverrideController animatorOV;

        public string skillID;
        public string skillName;
        //skill icon
        public Sprite SkillIconHUD;
        public string description;

        public AnimationClip animation;
        public bool useRootmotion;

        public float cooldown;
        public bool OnCD;

        public abstract void HandleSkill(PlayerManager owner, string position);

        public abstract void SetUp();

    }
}