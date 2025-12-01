using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;

namespace KS
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        [Header("Movement")]
        [SerializeField] float movementSpeed = 7;
        [SerializeField] float sprintSpeed = 10;
        [SerializeField] float walkSpeed = 4;
        [SerializeField] float rotationSpeed = 15;

        [Header("Jumping")]
        //[SerializeField]
        public float jumpHeight = 4;
        [SerializeField] Vector3 jumpDirection;

        [Header("Dodging")]
        [SerializeField] private PlayerDodgeSO playerDodge;
        [Space]
        [SerializeField] int airDodgeCount = 3;
        [SerializeField] int currentAirDodgeCount;
        private InputDirections JDDir = InputDirections.Zero;
        [SerializeField] float jdSlomoTime = .3f;

        [Header("AE movement")]
        public bool aeMovement;
        public float aeMoveForce = 7f;
        public Vector3 aeMoveVelo;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

        }

        protected override void Start()
        {
            base.Start();

            currentAirDodgeCount = airDodgeCount;
        }

        protected override void Update()
        {
            base.Update();

            if (player.isGrounded &&
                currentAirDodgeCount != airDodgeCount)
            {
                ResetAirDodges();
            }

            AeMovement();
        }

        public void HandleAllMovements()
        {
            HandleGroundMovement();
            HandleRotation();
            HandleJumpMovement();
        }

        //handles all ground movement related changes,
        //checks for walk & sprinting and changes movement speed & animations based on the inputs
        public void HandleGroundMovement()
        {
            if (player.isInteracting)
                return;

            if (player.isHit)
                return;

            if (player.isJumping && player.inputs.jumpFlag)
                return;

            if (!player.isGrounded)
                return;

            moveDirection = player.cameraHandler.transform.forward * player.inputs.verticalInput;
            moveDirection = moveDirection + player.cameraHandler.transform.right * player.inputs.horizontalInput;
            moveDirection.Normalize();
            moveDirection.y = 0;

            player.controller.Move(GetPlayerVelocity(true));

            if (player.modeManager.currentMode == PlayMode.FreeMode)
            {
                player.playerAnimations.UpdateAnimatorValues(0, player.inputs.moveAmount, player.isSprinting);
            }
            else
            {
                player.playerAnimations.UpdateAnimatorValues(player.inputs.horizontalInput, player.inputs.verticalInput, player.isSprinting);
            }

        }

        //handles all player rotation, behaviour changes depending on the mode.
        //in OTS mode the player rotation is based on the camera.
        //in LockOn mode the rotation is based on the lockon target direction.
        //in free mode the rotation is based on the camera frontal direction.
        public void HandleRotation()
        {
            if (player.isInteracting)
                return;

            if (player.isHit)
                return;

            if (player.modeManager.currentMode == PlayMode.LockOnMode)
            {
                if (player.inputs.sprintInput)
                {
                    // whilst sprinting
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = player.cameraHandler.transform.forward * player.inputs.verticalInput;
                    targetDirection += player.cameraHandler.transform.right * player.inputs.horizontalInput;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                        targetDirection = transform.forward;

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
                else
                {
                    //whilst strafing
                    Vector3 rotationDirection = moveDirection;
                    if (player.modeManager.currentMode == PlayMode.LockOnMode)
                    {
                        rotationDirection = player.cameraHandler.currentLockOnTarget.position - transform.position;
                    }
                    else
                    {
                        player.modeManager.ResetModes();
                    }
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();

                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                    if (player.inputs.moveAmount > 0)
                    {
                        transform.rotation = targetRotation;
                    }
                }
            }
            else
            {
                //whilst free
                Vector3 targetDirection = Vector3.zero;

                targetDirection = player.cameraHandler.transform.forward * player.inputs.verticalInput;
                targetDirection = targetDirection + player.cameraHandler.transform.right * player.inputs.horizontalInput;
                targetDirection.Normalize();
                targetDirection.y = 0;

                if (targetDirection == Vector3.zero)
                    targetDirection = transform.forward;

                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                transform.rotation = playerRotation;
            }
        }

        //helps with the momentum of the ground speed when jumping.
        //it basically add more directional movement to the jump.
        private void HandleJumpMovement()
        {
            if (player.isJumping)
            {
                player.controller.Move(jumpDirection * movementSpeed * Time.deltaTime);
            }
        }

        //checks if able to jump.
        //plays jumping animation and checks for possible movement.
        //uses the applyJumpVelocity function to shoot up.
        public void AttemptToPerformJump()
        {
            if (player.isInteracting)
                return;

            if (player.isJumping)
                return;

            if (!player.isGrounded)
                return;

            player.playerAnimations.PlayTargetAnimation("Jump", false, layerNum: 1);
            player.isJumping = true;

            jumpDirection = player.cameraHandler.transform.forward * player.inputs.verticalInput;
            jumpDirection += player.cameraHandler.transform.right * player.inputs.horizontalInput;
            jumpDirection.y = 0;

            if (jumpDirection != Vector3.zero)
            {
                if (player.isSprinting)
                {
                    jumpDirection *= 2;
                }
                else if (player.inputs.moveAmount > 0.5f)
                {
                    jumpDirection *= 1f;
                }
                else if (player.inputs.moveAmount < 0.5f)
                {
                    jumpDirection *= 0.5f;
                }
            }

            player.animator.SetBool("isJumping", true);
            ApplyJumpingVelocity();
            player.animationEvents.CircleSmoke(); //for the smoke VFX

        }

        //adds velocity to the vertical velocity of the player.
        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }

        //Checks if the player is moving or not. if they are the character will do a backstep.
        //else it will dodge forward in free mode.
        // in both OTS & LockedOn mode it will stepDodge into the inputted direction.
        public void HandleDodge()
        {
            //checks if it is dodge cancellable if it is then handle the animation cancel process first.
            //then do the dodge, if it isn't the return.
            if (!player.dodgeCancellable)
            {
                if (player.isInteracting)
                    return;
            }
            else
            {
                player.animator.SetBool("Cancelled", true);
                player.animationEvents.HandleCancelAnim();
            }

            if (!player.isGrounded)
            {
                player.isAerial = true;

                if (currentAirDodgeCount <= 0)
                {
                    currentAirDodgeCount = 0;
                    return;
                }

                currentAirDodgeCount -= 1;

            }

            //change the dodge direction based on the input and the play mode
            // if in lock on mode, dodge directional, if not then dodge forward.
            if (player.inputs.moveAmount > 0)
            {
                if (player.modeManager.currentMode == PlayMode.LockOnMode)
                {
                    playerDodge.PerformDodge(player, player.inputs.GetInputDirections());
                }
                else if (player.modeManager.currentMode == PlayMode.FreeMode)
                {
                    playerDodge.PerformDodge(player, InputDirections.North);
                }
            }
            else
            {
                playerDodge.PerformDodge(player, InputDirections.South);
            }

        }

        //resets the air dodge count
        public void ResetAirDodges()
        {
            currentAirDodgeCount = airDodgeCount;
        }

        public void SetJDDirection(InputDirections dir)
        {
            JDDir = dir;
        }

        //call the just dodge function on the scriptable object with all the needed data
        public void HandleJustDodge(Vector3 cross)
        {
            if (JDDir == InputDirections.Zero)
                return;


            if (JDDir == InputDirections.North)
            {
                aeMoveVelo = player.transform.forward;
            }
            else if (JDDir == InputDirections.South)
            {
                aeMoveVelo = -player.transform.forward;
            }
            else if (JDDir == InputDirections.West)
            {
                aeMoveVelo = -player.transform.right;
            }
            else if (JDDir == InputDirections.East)
            {
                aeMoveVelo = player.transform.right;
            }
           
            /*
            if (cross.y > 0)
            {
                //from the left
                if (player.inputs.GetInputDirections() == InputDirections.North)
                {

                    aeMoveVelo = new Vector3(1, 0, 1);
                }
                else 
                {

                    aeMoveVelo = new Vector3(1, 0, -1);
                }
                
            }
            else
            {
                //from the right
                if (player.inputs.GetInputDirections() == InputDirections.North)
                {

                    aeMoveVelo = new Vector3(-1, 0, 1);
                }
                else
                {

                    aeMoveVelo = new Vector3(-1, 0, -1);
                }
            }
            */
            playerDodge.HandleJustDodge();

        }

        public void RemoveJDDir()
        {
            JDDir = InputDirections.Zero;
        }

        public void SetupAeMovement(Vector3 move)
        {
            aeMoveVelo = move;
        }

        public void AeMovement()
        {
            if (aeMovement)
            {
                player.controller.Move(aeMoveVelo * aeMoveForce * Time.deltaTime);
            }
        }

        public float GetSloMoTime()
        {
            return jdSlomoTime;
        }

        // returns the "MoveDirection" times a movement speed depending on the input.
        // it can be returned with or without deltaTime,
        //if it is not it will only return direction times speed.
        private Vector3 GetPlayerVelocity(bool _deltaTime)
        {
            Vector3 pVelocity = Vector3.zero;
            if (_deltaTime)
            {
                if (player.isSprinting)
                {
                    pVelocity = moveDirection * sprintSpeed * Time.deltaTime;
                }
                else
                {
                    if (player.inputs.moveAmount > 0.5f)
                    {
                        pVelocity = moveDirection * movementSpeed * Time.deltaTime;
                    }
                    else if (player.inputs.moveAmount <= 0.5f)
                    {
                        pVelocity = moveDirection * walkSpeed * Time.deltaTime;
                    }
                }
            }
            else
            {
                if (player.isSprinting)
                {
                    pVelocity = moveDirection * sprintSpeed;
                }
                else
                {
                    if (player.inputs.moveAmount > 0.5f)
                    {
                        pVelocity = moveDirection * movementSpeed;
                    }
                    else if (player.inputs.moveAmount <= 0.5f)
                    {
                        pVelocity = moveDirection * walkSpeed;
                    }
                }
            }

            return pVelocity;
        }
    }

}