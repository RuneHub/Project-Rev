using kS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AIBossLocomotionManager : CharacterLocomotionManager
    {
        AIBossManager manager;
        private AnimatorOverrideController animatorOV;

        [Header("Movement")]
        public float movementSpeed = 10;
        public float horizontalInput;
        public float verticalInput;

        [Header("Teleport")]
        [Space(10)]
        public BossMovementSO TeleportSo;
        public BossMovementSO CurrentTPso;
        public Vector3 nextTeleportLocation;
        
        private BossTeleportLocations nextLoc;

        protected override void Awake()
        {
            base.Awake();

            manager = GetComponent<AIBossManager>();
            animatorOV = manager.animatorOV;

        }

        protected override void Start()
        {
            base.Start();

        }

        protected override void Update()
        {
            base.Update();

        }

        #region movement (temp)
        public void HandleAllMovement()
        {
            HandleGoundMovement();
        }

        public void InputMovement(Vector3 moveDir, float hori, float verti)
        {
            moveDirection = moveDir;
            horizontalInput = hori;
            verticalInput = verti;
        }

        private void HandleGoundMovement()
        {
            if (manager.isInteracting)
                return;

            if (!manager.isGrounded)
                return;

            //moveDirection = manager.GetTarget().transform.position - transform.position;
            moveDirection.Normalize();
            moveDirection.y = transform.position.y;

            manager.controller.Move(moveDirection * movementSpeed * Time.deltaTime);
            manager.bossAnimations.UpdateAnimatorValues(horizontalInput, verticalInput);
        }
        #endregion

        #region Teleport
        //set the given animation information in the animator overrider, and the current teleport So,
        //play's the given animation
        public void HandleTeleport(BossMovementSO so, BossTeleportLocations nextLocation)
        {
            animatorOV[TeleportSo.startupAnim.name] = so.startupAnim;
            animatorOV[TeleportSo.endAnim.name] = so.endAnim;
            manager.animator.runtimeAnimatorController = animatorOV;
            CurrentTPso = so;

            nextLoc = nextLocation;

            manager.bossAnimations.PlayTargetAnimation("TeleportStart", true, false, CrossFadeSpeed: 0, layerNum: 2, normalizedTime: 0);
        }

        //the actual teleport function.
        //uses the current TP so for the VFX, turns off the visuals + colliders.
        // teleporting is done by disabling the character controller, and changing the position.
        //waits for the timer, turn everything back on whilst masking it with the second VFX.
        //and play's the end animation.
        public IEnumerator ExecuteTeleport()
        {
            //play teleport go VFX
            //make invisible
            //remove player lock on <- maybe
            Instantiate(CurrentTPso.StartUpVFX, transform.position, Quaternion.identity, null);
            manager.statManager.InvulnOFF();
            yield return new WaitForSeconds(1);
            manager.animationEvents.CharInvisible();

            //move boss to next location
            //play teleport arrived VFX
            //make visible
            //add player lockon <- maybe
            //play animation
            yield return new WaitForSeconds(CurrentTPso.VFXDuration + CurrentTPso.timer);

            manager.controller.enabled = false;
            switch (nextLoc)
            {
                case BossTeleportLocations.Mechanics:
                    Instantiate(CurrentTPso.EndVFX, manager.CentralPosition, Quaternion.identity, null);
                    transform.position = manager.CentralPosition;
                    transform.rotation = manager.CentralRotation;
                    break;
                case BossTeleportLocations.NextLocation:
                    Instantiate(CurrentTPso.EndVFX, nextTeleportLocation, Quaternion.identity, null);
                    transform.position = nextTeleportLocation + new Vector3(0, 1, 0);
                    break;
            }
            manager.controller.enabled = true;
            yield return new WaitForSeconds(1f);
            transform.LookAt(manager.GetTarget().transform.position);
            manager.statManager.InvulnON();
            manager.animationEvents.CharVisible();
            manager.bossAnimations.PlayTargetAnimation("TeleportEnd", true, false, CrossFadeSpeed: 0, layerNum: 2, normalizedTime: 0);
        }

        //sets the given information to TP back, always uses "Mechanics" as TeleportLocation
        public void HandleTeleportBack(BossMovementSO so)
        {
            animatorOV[TeleportSo.startupAnim.name] = so.startupAnim;
            animatorOV[TeleportSo.endAnim.name] = so.endAnim;
            manager.animator.runtimeAnimatorController = animatorOV;
            CurrentTPso = so;

            StartCoroutine(ExecuteTeleportBack());
        }

        //this is only for when the last part of the TP is needed.
        //uses the same logic as the whole teleport
        public IEnumerator ExecuteTeleportBack()
        {
            Instantiate(CurrentTPso.EndVFX, manager.CentralPosition, Quaternion.identity, null);
            transform.position = manager.CentralPosition;
            transform.rotation = manager.CentralRotation;

            manager.controller.enabled = true;
            yield return new WaitForSeconds(1f);
            transform.LookAt(manager.GetTarget().transform.position);
            manager.statManager.InvulnON();
            manager.animationEvents.CharVisible();
            manager.bossAnimations.PlayTargetAnimation("TeleportEnd", true, false, CrossFadeSpeed: 0, layerNum: 2, normalizedTime: 0);

        }

        //places the character at a given location, if invisibili and invulnability are turned on, turns those off.
        public void FastTravel(Vector3 Location, bool isInvisible, bool isInvincible)
        {
            manager.controller.enabled = false;
            transform.position = Location;

            if (isInvisible)
            {
                manager.animationEvents.CharVisible();
            }

            if (isInvincible)
            {
                manager.statManager.InvulnON();
            }

            manager.controller.enabled = true;
        }
        
        #endregion

    }
}