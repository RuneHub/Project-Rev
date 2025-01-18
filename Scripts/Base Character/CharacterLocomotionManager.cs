using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS { 
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        public Vector3 moveDirection;
        public LayerMask groundLayer;

        [Header("Gravity variables")]
        public float inAirTimer;
        [SerializeField] public Vector3 yVelocity;
        [SerializeField] protected float groundedYVelocity = -20; //ground applied force.
        [SerializeField] protected float fallStartYVelocity = -7; //increases over time, immediet applied fall force.
        [SerializeField] public float gravityForce = -25;
        [SerializeField] protected float groundCheckSphereRadius = 0.3f;

        [SerializeField]
        protected bool fallingVelocitySet = false;

        [Header("Additional Movement")]
        public bool useAdditionalMovement = false;
        public float adMoveForce;
        public Vector3 adMoveDirection;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Start()
        {
            yVelocity.y = groundedYVelocity;
        }

        //checks to see if player is truly grounded and sends its info to the animator and the "HandleGroundCheck" function.
        protected virtual void Update()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
            character.animator.SetBool("isGrounded", character.isGrounded);
            character.animator.SetBool("isAerial", character.isAerial);

            HandleGroundCheck();
            HandleAdditionalInteractionMovement();
        }

        //a virtual function that can be overridden, checks if the character is in the air.
        //if they increase their gravity else set their gravity to ground level.
        public virtual void HandleGroundCheck()
        {

            if (character.isGrounded)
            {
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocitySet = false;
                    yVelocity.y = groundedYVelocity;
                }
                character.isAerial = false;
            }
            else if (character.isAerial && character.isInteracting)
            {
                yVelocity.y = 0;
            }
            else
            {
                if (!character.isJumping 
                    && !fallingVelocitySet 
                    && !character.isHit)
                {
                    //Debug.Log("starts falling");
                    fallingVelocitySet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer = inAirTimer + Time.deltaTime;
                yVelocity.y += gravityForce * Time.deltaTime;

            }

            character.animator.SetFloat("inAirTimer", inAirTimer);
            character.controller.Move(yVelocity * Time.deltaTime);

        }

        //a virtual function that is running in the background,
        //it can be used to move thbe character whilst it is doing an animation.
        public virtual void HandleAdditionalInteractionMovement()
        {
            if (character.isInteracting && useAdditionalMovement)
            {
                character.controller.Move(adMoveDirection * adMoveForce * Time.deltaTime);
            }
        }

        //turns off the movment boolean and reset the values.
        public virtual void ResetAdditionalInteractionMovement()
        {
            adMoveDirection = Vector3.zero;
            adMoveForce = 0;
        }

        //temp, for debug purposes
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, groundCheckSphereRadius);
        }

    }
}
