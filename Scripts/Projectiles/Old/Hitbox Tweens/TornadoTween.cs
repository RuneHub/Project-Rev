using KSTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class TornadoTween : MonoBehaviour
    {
        public bool additivePosition;
        public Vector3 toPosition;
        public float toPositionTimer;

        public bool additiveScale;
        public Vector3 toScale;
        public float toScaleTimer;

        private void OnEnable()
        {
            if (additivePosition)
            {
                toPosition += transform.position;
            }

            if (additiveScale)
            {
                toScale += transform.localScale;
            }

            this.TweenPosition(toPosition, toPositionTimer).SetOnComplete(() => this.TweenLocalScale(toScale, toScaleTimer));

        }
    }
}