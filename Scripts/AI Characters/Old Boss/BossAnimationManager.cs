using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace KS { 
    public class BossAnimationManager : CharacterAnimationManager
    {
        int horizontal, vertical;

        public RigBuilder rigbuilderAim;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
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

            animator.SetFloat(horizontal, snappedHorizontal, 0.1f, Time.deltaTime);
            animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
        }

        //enables the rig Aim.
        public void SetRigAimActive()
        {
            rigbuilderAim.enabled = true;
        }

        //disables the rig aim.
        public void SetRigAimDisable()
        {
            rigbuilderAim.enabled = false;
        }

    }
}