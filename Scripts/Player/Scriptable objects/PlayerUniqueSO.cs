using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS {
    [CreateAssetMenu(menuName = "Player/Attacks/Unique")]
    public class PlayerUniqueSO : ScriptableObject
    {
        private PlayerManager player;
        private PlayerCombatManager combatManager;
        private PlayerCombatAnimationEvents animEvents;
        private AnimatorOverrideController animatorOV;

        [Space(10), Header("Unique Animations")]
        public AnimationClip UForward;
        public AnimationClip UBackwards;
        public AnimationClip ULeft;
        public AnimationClip URight;

        [Space(10), Header("Animation Parameters")]
        public int animationLayer;
        public bool isInteracting, useRootmotion;

        [Space(10), Header("skill data")]
        public AttackStandardSO uniqueData;
        public AttackStandardSO uniqueFinisherData;
        private bool UFinisher;

        [Space(10), Header("VFX")]
        public GameObject UniqueFinisherVFX;
        public float UFDestroyTimer;
        public GameObject UniqueMovementVFX;
        public float UMDestroyTimer;

        [Space(10), Header("Sound Effects")]
        public List<APSoundData> skillSFXList = new List<APSoundData>();

        //set up
        private void Setup(PlayerManager owner)
        {
            player = owner;
            combatManager = player.combatManager;
            animEvents = player.combatAnimationEvents;
            animatorOV = player.animatorOV;

            animEvents.OnShootEventTriggered += Shoot;
            animEvents.OnSkillDeactiveTriggered += CleanUp;
            animEvents.OnFXTriggered += ActivateFX;

            for (int i = 0; i < skillSFXList.Count; i++)
            {
                player.soundManager.AddToSkillSFXList(skillSFXList[i]);   
            }
        }

        public void PerformUnique(PlayerManager owner, bool isFinisher, InputDirections inputs)
        {
            Setup(owner);
            Vector3 dir = Vector3.zero;

            //NE,NW,SE,SW will be put under E & W respectively because the movement is 8-directional but unique is 4-directional. 
            switch (inputs)
            {
                case InputDirections.North:
                    dir = player.cameraHandler.transform.forward;
                    animatorOV["Anim_Combat_UnqiuePlaceHolder"] = UForward;
                    break;
                case InputDirections.South:
                    dir = -player.cameraHandler.transform.forward;
                    animatorOV["Anim_Combat_UnqiuePlaceHolder"] = UBackwards;
                    break;
                case InputDirections.East:
                case InputDirections.NorthEast:
                case InputDirections.SouthEast:
                    dir = player.cameraHandler.transform.right;
                    animatorOV["Anim_Combat_UnqiuePlaceHolder"] = URight;
                    break;
                case InputDirections.West:
                case InputDirections.NorthWest:
                case InputDirections.SouthWest:
                    dir = -player.cameraHandler.transform.right;
                    animatorOV["Anim_Combat_UnqiuePlaceHolder"] = ULeft;
                    break;
               
            }

            player.animator.runtimeAnimatorController = animatorOV;

            player.animationEvents.HandleInvisible();
            player.animationEvents.SpinFXOff("Both");

            GameObject UMFX = Instantiate(UniqueMovementVFX, player.transform);
            Destroy(UMFX, UMDestroyTimer);

            if (isFinisher)
            {
                UFinisher = true;
                combatManager.PerformUnqiueMovement(-player.transform.forward);
                player.playerAnimations.PlayTargetAnimation("Unique_Finisher", isInteracting, useRootmotion, layerNum: animationLayer);

                combatManager.CanUFinish = false;
                player.uniqueMechManager.ResetLoadedLevel();
                player.UniqueFinisher = false;

            }
            else
            {
                combatManager.PerformUnqiueMovement(dir);
                player.playerAnimations.PlayTargetAnimation("Unique_Skill", isInteracting, useRootmotion, layerNum: animationLayer);


                //gauge reset
                if (player.uniqueMechManager.loadedLevel != PlayerUniqueMechanicManager.MechLoadedLevel.lvl0)
                {
                    player.UniqueFinisher = true;
                }

            }

        }

        public void Shoot(System.Object sender, Transform output)
        {
            if (UFinisher)
            {
                player.combatManager.ShootRaycastHitscan(output, uniqueFinisherData);
            }
            else
            {
                player.combatManager.ShootRaycastHitscan(output, uniqueData);
            }
        }

        public void CleanUp(System.Object sender, EventArgs e)
        {
            player.soundManager.ClearSkillSFXList();
            UFinisher = false;

            animEvents.OnShootEventTriggered -= Shoot;
            animEvents.OnSkillDeactiveTriggered -= CleanUp;
            animEvents.OnFXTriggered -= ActivateFX;
        }

        private void ActivateFX(System.Object sender, EventArgs e)
        {
            GameObject vfx = Instantiate(UniqueFinisherVFX, player.transform);
            Destroy(vfx, UFDestroyTimer);
        }


    }
}