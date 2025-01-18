using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    [CreateAssetMenu(menuName = "Player/Locomotion/Dodge")]
    public class PlayerDodgeSO : ScriptableObject
    {
        private PlayerManager player;
        private PlayerLocomotionManager locomotionManager;
        private PlayerCombatAnimationEvents animEvents;
        private AnimatorOverrideController animatorOV;

        [Header("Dodges")]
        public AnimationClip dNorth;
        public AnimationClip dSouth;
        public AnimationClip dWest;
        public AnimationClip dEast;
        [Space]
        public bool useVFX;
        [DrawIf("useVFX", true)] public float VFXTime;

        [Header("Animation parameters")]
        public int animationLayer;
        public bool isInteracting, useRootmotion;

        [Header("Just Dodge")]
        public AnimationClip justDodgeAnim;
        public bool useRotationOffset;
        [DrawIf("useRotationOffset", true)] public float rotationOffset;
        public bool useJustVFX;
        [DrawIf("useJustVFX", true)] public bool useSlomo;
        [DrawIf("useSlomo", true)] public float slomoTime;

        //sets up all the references.
        private void SetUp(PlayerManager owner)
        {
            player = owner;
            locomotionManager = player.playerLocomotion;
            animEvents = player.combatAnimationEvents;

            animatorOV = player.animatorOV;
        }

        //swaps the animation based on the given input and plays the animations
        public void PerformDodge(PlayerManager owner, InputDirections dir)
        {
            SetUp(owner);

            //swapping animation depending on given directions
            if (dir == InputDirections.North)
            {
                // 12, forward
                animatorOV["Anim_Combat_DodgePlaceHolder"] = dNorth;
            }
            else if(dir == InputDirections.South)
            {
                // 6, backwards
                animatorOV["Anim_Combat_DodgePlaceHolder"] = dSouth;
            }
            else if (dir == InputDirections.West)
            {
                // 9, Left
                animatorOV["Anim_Combat_DodgePlaceHolder"] = dWest;
            }
            else if (dir == InputDirections.East)
            {
                // 3, right
                animatorOV["Anim_Combat_DodgePlaceHolder"] = dEast;
            }

            player.playerLocomotion.SetJDDirection(dir);

            player.animator.runtimeAnimatorController = animatorOV;

            //play animation & effect if used
            player.playerAnimations.PlayTargetAnimation("Dodge", isInteracting, useRootmotion, layerNum: animationLayer);

        }

        public void HandleJustDodge()
        {
            animatorOV["Anim_Combat_DodgePlaceHolder"] = justDodgeAnim;
            player.animator.runtimeAnimatorController = animatorOV;

            if (useSlomo)
            {
                // slow motion effect
            }

            player.playerAnimations.PlayTargetAnimation("Dodge", isInteracting, false, layerNum: animationLayer);

        }

    }
}