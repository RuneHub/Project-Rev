using KSTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class DashTween : MonoBehaviour
    {
        public bool SetInCode;

        public bool additivePosition;
        public Vector3 toPosition;
        public float toPositionTimer;

        public bool additiveScale;
        public Vector3 toScale;
        public float toScaleTimer;

        private void OnEnable()
        {
            if (!SetInCode)
            {
                if (additivePosition)
                {
                    toPosition += transform.position;
                }

                if (additiveScale)
                {
                    toScale += transform.localScale;
                }

                this.TweenLocalScale(toScale, toScaleTimer).SetOnComplete(() => this.TweenPosition(toPosition, toPositionTimer));
            }
        }

        public void SetScale(Vector3 ScaleSet, bool additive)
        {
            if (additive)
            {
                toScale += ScaleSet;
            }
            else
            {
                toScale = ScaleSet;
            }

        }

        public void SetPosition(Vector3 positionSet, bool additive)
        {
            if (additive)
            {
                toPosition += positionSet;
            }
            else
            {
                toPosition = positionSet;
            }
        }

        public void StartTween()
        {
            this.TweenLocalScale(toScale, toScaleTimer).SetOnComplete(() => this.TweenPosition(toPosition, toPositionTimer));
        }


    }
}