using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public enum PlayMode { FreeMode, LockOnMode }//, OTSMode }

    public class ModeManager : MonoBehaviour
    {
        PlayerManager player;

        public bool FreeMovement;
        public bool StrafeMovement;
        public bool FreeCam;
        public bool OTSCam;

        /* PlayMode.
         * PlayMode consist of three different modes, 
         *  1, FreeMode: the camera move independant from the character.
         *                  movement of the character consist of moving forward + rotation.
         *                  camera forward is also character forward.
         *  2, LockOnMode: the Camera is locked on unto an entity, 
         *                  this means that it will always try to get that entity as focus of the screen.
         *                  character will still be visible.
         *                  character movement consist of strafe movement in eight directions
         *                  with forward being in the direction of the locked on entity so the character
         *                  always faces it.
         *  3, OTSMode: The camera is moved somewhat behind and to the side of the character.
         *              the camera also dictates the rotation of the character.
         *              character movement is based of strafe movement.
         *              character's back will always be facing the camera so he is looking forward.
         */

        //starts in free mode.
        public PlayMode currentMode = PlayMode.FreeMode;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();

        }

        //sets the mode and calls for the update.
        public void swapMode()
        {
            if (currentMode == PlayMode.LockOnMode)
            {
                OnLockedOn();
            }
            else if (currentMode == PlayMode.FreeMode)
            {
                currentMode = PlayMode.FreeMode;
            }

            UpdateMode();
        }

        //sets lockon mode if it isn't otherwise sets it back to freemode.
        public void OnLockedOn()
        {
            if (currentMode == PlayMode.FreeMode || currentMode == PlayMode.LockOnMode)
            {

                if (currentMode == PlayMode.FreeMode)
                {
                    currentMode = PlayMode.LockOnMode;
                }
                else if (currentMode == PlayMode.LockOnMode)
                {
                    currentMode = PlayMode.FreeMode;
                }

                UpdateMode();
            }
        }

        //as backup for if there goes something wrong. it will set it back to Freemode.
        public void ResetModes()
        {
            player.cameraHandler.ClearLockOnTargets();
            SetFreeMode();
            swapMode();
        }

        //Updates the values of the modes.
        private void UpdateMode()
        {
            switch (currentMode)
            {
                case PlayMode.FreeMode:
                    SetFreeMode();
                    break;
                case PlayMode.LockOnMode:
                    SetLockOnMode();
                    break;
                default:
                    currentMode = PlayMode.FreeMode;
                    break;
            }

        }

        //sets all the variables to true that have to do with free mode and turns off the ones that don't.
        private void SetFreeMode()
        {
            FreeMovement = true;
            StrafeMovement = false;
            FreeCam = true;
            OTSCam = false;
            player.inputs.lockOnFlag = false;
            player.cameraHandler.ClearLockOnTargets();
            player.cameraHandler.SetLockCameraOffset();


            player.isAiming = false;
            player.animationEvents.SwapWeaponToHolster("Both");
        }
        
        //sets all the variables to true that have to do with LockOn mode and turns off the ones that don't.
        private void SetLockOnMode()
        {
            FreeMovement = false;
            StrafeMovement = true;
            FreeCam = true;
            OTSCam = false;
            player.inputs.lockOnFlag = true;
            player.cameraHandler.SetLockCameraOffset();

            player.isAiming = false;
            player.animationEvents.SwapWeaponToHolster("Both");
        }
        
    }
}