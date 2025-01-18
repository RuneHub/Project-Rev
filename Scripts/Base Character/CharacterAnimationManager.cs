using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS { 
    public class CharacterAnimationManager : MonoBehaviour
    {
        public CharacterManager character;

        public Animator animator;

        protected virtual void Awake()
        {
            character = GetComponentInParent<CharacterManager>();
        }

        protected virtual void Start()
        {
            animator = character.animator;
        }

        //the function that will override the animation to play a targeted animation,
        //has parameters that have to be filled and ones that have a default value if left empty.
        public void PlayTargetAnimation(string targetAnimation, bool isInteracting, bool UseRootmotion = false, float CrossFadeSpeed = 0.2f, int layerNum = 0, float normalizedTime = 0f)
        {
            animator.SetBool("isInteracting", isInteracting);
            animator.SetBool("isUsingRootmotion", UseRootmotion);
            animator.CrossFade(targetAnimation, CrossFadeSpeed, layerNum, normalizedTime);
        }

        //set the animator value for invulnerability on true
        public void EnableInvuln()
        {
            animator.SetBool("isInvulnerable", true);
        }

        //set the animator value for invulnerability on false
        public void DisableInvuln()
        {
            animator.SetBool("isInvulnerable", false);
        }
        
        //enables the character controller
        public void EnableController()
        {
            character.controller.enabled = true;
        }
        
        //disables the character controller
        public void DisableController()
        {
            character.controller.enabled = false;
        }

        //checks every animation frame, will move the character together with rootmotion
        //does this by turning off the rigidbody velocity.
        public void OnAnimatorMove()
        {
            if (character.isInteracting == false)
                return;

            Vector3 velocity = character.animator.deltaPosition;
            character.controller.Move(velocity);
            character.transform.rotation *= character.animator.deltaRotation;
        }

    }
}
