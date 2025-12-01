using KS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class PlayerCombatAnimationEvents : MonoBehaviour
    {
        PlayerManager player;
        Animator anim;
        PlayerAnimationEvents playerAnimEvents;

        public Transform WeaponOutputLeft;
        public Transform WeaponOutputRight;
        public Transform WeaponsMiddlePoint;

        public event EventHandler<Transform> OnShootEventTriggered;
        public event EventHandler OnSkillTriggered;
        public event EventHandler OnSkillDeactiveTriggered;
        public event EventHandler OnFXTriggered;

        private void Awake()
        {
            player = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            playerAnimEvents = GetComponent<PlayerAnimationEvents>();

            //make both weapon shoot at the same time with the output of the transform in the middle of both weapons.
            WeaponsMiddlePoint.position = WeaponOutputLeft.position + (WeaponOutputRight.position - WeaponOutputLeft.position) / 2;

        }

        #region Combo functions
        //sets the boolean true on the animator.
        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        //Disables the combo, turns the bool on the animator to false and calls for resets.
        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
            HardComboReset();
        }

        //enables perfect timing, turns on the bool on the animator
        public void EnablePerfectTime()
        {
            anim.SetBool("perfectTimedCombo", true);
        }

        //disables perfect timing, turns off the bool on the animator
        public void DisablePerfectTime()
        {
            anim.SetBool("perfectTimedCombo", false);
            player.combatManager.perfectCombo = false;
        }

        public void ComboContinue()
        {

        }

        //Animation even function that calls the hard combo reset on combat manager.
        public void HardComboReset()
        {
            player.combatManager.ResetCombo();
        }

        //Animation event function that calls the Reset combo animation on combat manager.
        public void ResetCombatAnimations()
        {
            //Debug.Log("reset combo");
            player.combatManager.ResetCombatAnimations();
        }
        #endregion

        #region Action functions
        //Animation event function triggered by attack animations.
        //calls the combat manager for additional effects and gives the transform where the output is.
        public void WeaponShoot(string _side)
        {
            if (_side == "Right")
            {
                if (playerAnimEvents.RightHandWeapon != null)
                {
                    //player.combatManager.AttackManifestation(WeaponOutputRight);
                    //player.combatManager.shootProjectile(WeaponOutputRight);

                    //player.combatManager.ShootRaycast(WeaponOutputRight);

                    player.combatManager.ShootRaycastHitscan(WeaponOutputRight, player.combatManager.currentAttack);
                }
            }
            else if (_side == "Left")
            {
                if (playerAnimEvents.LeftHandWeapon != null)
                {
                    //player.combatManager.AttackManifestation(WeaponOutputLeft);
                    //player.combatManager.shootProjectile(WeaponOutputLeft);

                    //player.combatManager.ShootRaycast(WeaponOutputLeft);

                    player.combatManager.ShootRaycastHitscan(WeaponOutputLeft, player.combatManager.currentAttack);
                }
            }
            else if (_side == "Both")
            {
                if (playerAnimEvents.LeftHandWeapon != null &&
                    playerAnimEvents.RightHandWeapon != null)
                {
                    //make both weapon shoot at the same time with the output of the transform in the middle of both weapons.

                    WeaponsMiddlePoint.position = WeaponOutputLeft.position + (WeaponOutputRight.position - WeaponOutputLeft.position) / 2;
                    //player.combatManager.AttackManifestation(WeaponsMiddlePoint);
                    // player.combatManager.shootProjectile(WeaponsMiddlePoint);


                    //player.combatManager.ShootRaycast(WeaponsMiddlePoint);

                    player.combatManager.ShootRaycastHitscan(WeaponsMiddlePoint, player.combatManager.currentAttack);
                }
            }

        }

        //Function that gets called by an animation event for Skills.
        //an function will be invoked back on the scriptable object the animation came from.
        public void TriggerSkillShot(string _side)
        {
            if (_side == "Right" && playerAnimEvents.RightHandWeapon != null)
            {
                OnShootEventTriggered?.Invoke(this, WeaponOutputRight);
            }
            else if (_side == "Left" && playerAnimEvents.LeftHandWeapon != null)
            {
                OnShootEventTriggered?.Invoke(this, WeaponOutputLeft);
            }
            else if (_side == "Both" && playerAnimEvents.LeftHandWeapon != null &&
                    playerAnimEvents.RightHandWeapon != null)
            {
                OnShootEventTriggered?.Invoke(this, WeaponsMiddlePoint);
            }
        }

        public void ActivateSkill()
        {
            OnSkillTriggered?.Invoke(this, EventArgs.Empty);
        }

        public void DeactivateSkill()
        {
            OnSkillDeactiveTriggered?.Invoke(this, EventArgs.Empty);
        }

        public void ActivateFX()
        {
            OnFXTriggered?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Invuln & Just Dodge functions
        /*
         * the animator event function that get called from a animation.
         * sets the player manager boolean & calls the function for actually turning on/off the colliders
         * */

        [ContextMenu("InvulnON")]
        public void InvulnON()
        {
            player.isInvulnerable = true;
            player.playerStats.Setvulnerability(true);
        }

        [ContextMenu("InvulnOFF")]
        public void InvulnOFF()
        {
            player.isInvulnerable = false;
            player.playerStats.Setvulnerability(false);
        }

        public void JDOn()
        {
            player.playerStats.ActivateJD();
        }

        public void JDOff()
        {
            player.JustDodge = false;
            player.playerLocomotion.RemoveJDDir();
        }

        #endregion

        #region Ae movement
        public void AeDoMove()
        {
            player.playerLocomotion.aeMovement = true;
        }

        public void AeStopMove()
        {
            player.playerLocomotion.aeMovement = false;
        }
        #endregion

        #region Slow motion Time
        public void SlowMoTime()
        {
            Time.timeScale = player.playerLocomotion.GetSloMoTime();
        }

        public void EndSlowMoTime()
        {
            Time.timeScale = 1;
        }
        #endregion

        #region Celestial Clone
        public void HandleAdditional()
        {
            if (player.combatManager.perfectCombo)
            {
                player.combatManager.CelestialCloneAddition();
            }
        }

        public void HandleAdditionalFinish()
        {
            if (player.combatManager.perfectFinish)
            {
                player.combatManager.CelestialClonePerfectFinish();
            }
        }

        #endregion

        #region Unique Mechanic
        public void ResetLoadedGauge()
        {
            player.uniqueMechManager.ResetLoadedLevel();
        }

        #endregion

    }
}
