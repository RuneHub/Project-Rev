using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        PlayerManager player;

        public GameObject LeftHandWeapon;
        public GameObject RightHandWeapon;
        public GameObject LeftHolsterWeapon;
        public GameObject RightHolsterWeapon;
        [Space]
        public GameObject LeftSpinEffect;
        public GameObject RightSpinEffect;
        

        private void Awake()
        {
            player = GetComponentInParent<PlayerManager>();
        }

        #region Visuals & VFX
        //gives an visual effect of taking weapons from the holster while in reality
        // it just turns off the object in the holster and turns on the object in the hand.
        public void SwapWeaponToHand(string _side)
        {
            if (_side == "Left")
            {
                LeftHandWeapon.SetActive(true);
                LeftHolsterWeapon.SetActive(false);
            }
            else if (_side == "Right")
            {
                RightHandWeapon.SetActive(true);
                RightHolsterWeapon.SetActive(false);
            }
            else if (_side == "Both")
            {
                LeftHandWeapon.SetActive(true);
                RightHandWeapon.SetActive(true);

                LeftHolsterWeapon.SetActive(false);
                RightHolsterWeapon.SetActive(false);
            }


        }

        //for the visual effect of putting the weapon back into the holster.
        //can be done for either side or both.
        public void SwapWeaponToHolster(string _side)
        {
            if (_side == "Left")
            {
                LeftHolsterWeapon.SetActive(true);
                LeftHandWeapon.SetActive(false);
            }
            else if (_side == "Right")
            {
                RightHolsterWeapon.SetActive(true);
                RightHandWeapon.SetActive(false);
            }
            else if (_side == "Both")
            {
                LeftHolsterWeapon.SetActive(true);
                RightHolsterWeapon.SetActive(true);

                LeftHandWeapon.SetActive(false);
                RightHandWeapon.SetActive(false);
            }

        }

        public void PlayPhaseShift(float time)
        {
            player.trailEffect.ActivateFX(time);
        }

        public void StopPhaseShift()
        {
            player.trailEffect.DeactivateFX();
        }

        //VFX effect for jumping
        public void CircleSmoke()
        {
            player.effectManager.JumpEffect();
        }

        //spin effect turning on
        public void SpinFXOn(string _side)
        {
            if (_side == "Left")
            {
                LeftSpinEffect.SetActive(true);
            }
            else if (_side == "Right")
            {

                RightSpinEffect.SetActive(true);
            }
            else if (_side == "Both")
            {
                LeftSpinEffect.SetActive(true);
                RightSpinEffect.SetActive(true);
            }
        }

        //spin effect turning off
        public void SpinFXOff(string _side)
        {
            if (_side == "Left")
            {
                LeftSpinEffect.SetActive(false);
            }
            else if (_side == "Right")
            {

                RightSpinEffect.SetActive(false);
            }
            else if (_side == "Both")
            {
                LeftSpinEffect.SetActive(false);
                RightSpinEffect.SetActive(false);
            }
        }

        #endregion

        #region Sound FX
        //for movement on solid underground
        public void PlayRunSFX()
        {
            player.soundManager.PlayFootstepAudio();
        }

        //for jumping from solid underground
        public void PlayJumpingSFX()
        {
            player.soundManager.PlayJumpSound();
        }

        //for landing on solid underground
        public void PlayLandingSFX()
        {
            player.soundManager.PlayLandingSound();
        }

        public void PlaySprintStopSFX()
        {
            player.soundManager.PlayerSprintStop();
        }

        //when dodging
        public void PlayDodgeSFX()
        {
            player.soundManager.PlayDodgeSound();
        }
        #endregion

        #region Animation Cancel
        //sets the boolean to true for animation cancelling
        public void AnimCancellable()
        {
            player.animator.SetBool("isCancellable", true);
        }

        public void StopAnimCancel()
        {
            player.animator.SetBool("isCancellable", false);
        }

        //handles the animation cancel when attack,jump or movement inputs are inputted.
        //also happens before dodge, these parameter ensure that everything animation wise gets reset
        public void HandleCancelAnim()
        {
            //Debug.Log("Anim cancel");

            player.animator.SetBool("isInteracting", false);
            player.animator.SetBool("isUsingRootmotion", false);

            SwapWeaponToHolster("Both");
            player.combatAnimationEvents.ResetCombatAnimations();
            player.combatAnimationEvents.HardComboReset();

            player.combatAnimationEvents.DeactivateSkill();

            if (player.inputs.moveAmount > 0)
            {
                player.animator.Play("empty", -1);
            }

            player.animator.SetBool("Cancelled", false);
        }

        //handles the animation cancel, if cancellable is true and there is an input.
           // set rootmotion & interacting to false, reset the animations and swap the gun to the holsters.
        public void HandleAllAnimCancels()
        {
            if (player.isCancellable)
            {
                if (player.inputs.moveAmount > 0
                || player.isJumping || player.animCancelled)
                {

                    player.animator.SetBool("Cancelled", true);
                    //Cancel Animation

                    player.animator.SetBool("isInteracting", false);
                    player.animator.SetBool("isUsingRootmotion", false);

                    SwapWeaponToHolster("Both");
                    player.combatAnimationEvents.ResetCombatAnimations();
                    player.combatAnimationEvents.HardComboReset();

                    if (player.inputs.moveAmount > 0)
                    {
                        player.animator.Play("empty", -1);
                    }

                    player.animator.SetBool("Cancelled", false);

                }
            }
        }
        #endregion

        #region Cutscene

        public void IntroCutsceneMove()
        {
            StartCoroutine(MoveToZero());
        }

        IEnumerator MoveToZero()
        {
            float duration = 4f;
            float time = 0;
            Vector3 targetPos = new Vector3(0, 0, 0);

            while (time < duration) 
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, time/ duration);
                
                if(transform.localPosition == targetPos )
                {
                    break;
                }
                
                time += Time.deltaTime;
                yield return null;
            }

            transform.localPosition = Vector3.zero;

        }

        #endregion

    }
}