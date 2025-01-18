using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace KS {
    public class PlayerAnimationManager : CharacterAnimationManager
    {

        //animation variables
        int horizontal, vertical;

        protected override void Awake()
        {
            base.Awake();

            //to shorten search for memory
            horizontal = Animator.StringToHash("Horizontal");
            vertical = Animator.StringToHash("Vertical");
        }

        protected override void Start()
        {
            base.Start();

        }

        //an value function that makes the values more rounded numbers like 0, 0.5 or 1 so that the animation that gets blended is more clear.
        //also works with the animator to get desired animations like sprint.
        public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
        {
            //Animation snapping
            float snappedHorizontal;
            float snappedVertical;

            #region snappedHorizontal
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                snappedHorizontal = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                snappedHorizontal = 1f;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                snappedHorizontal = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                snappedHorizontal = -1;
            }
            else
            {
                snappedHorizontal = 0;
            }
            #endregion
            #region snappedVertical
            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                snappedVertical = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                snappedVertical = 1f;
            }
            else if (verticalMovement < 0 && verticalMovement > -0.55f)
            {
                snappedVertical = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                snappedVertical = -1;
            }
            else
            {
                snappedVertical = 0;
            }
            #endregion

            if (isSprinting)
            {
                snappedHorizontal = horizontalMovement;
                snappedVertical = 2;
            }

            animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
        }

        //gets the animation multiplier from the animation and changes the animation speed for the given duration.
        //changes it back to normal aftwards.
        public void AdjustAnimationSpeed(string multiplier, float speed, float duration)
        {
            StartCoroutine(AdjustSpeed(multiplier, speed, duration));
        }

        IEnumerator AdjustSpeed(string multiplier, float speed, float duration)
        {
            animator.SetFloat(multiplier, speed);

            yield return new WaitForSeconds(duration);

            animator.SetFloat(multiplier, 1);

        }

    }

}
