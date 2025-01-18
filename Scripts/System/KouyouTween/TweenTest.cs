using KSTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KouyouTween
{
    public class TweenTest : MonoBehaviour
    {
        public bool additivePosition;
        public Vector3 toPosition;
        public float toPositionTimer;
        public Vector3 toScale;
        public float toScaleTimer;

        private void OnEnable()
        {

            //this.TweenPosition(new Vector3(this.transform.position.x, this.transform.position.y + 4, this.transform.position.z), 1).SetOnComplete(()=> this.TweenLocalScale(new Vector3(5, 8, 5), 2));

            if (additivePosition)
            {
                toPosition += transform.position;
            }
            

            this.TweenLocalScale(toScale, toScaleTimer).SetOnComplete(()=>this.TweenPosition(toPosition, toPositionTimer));
        }

        [ContextMenu("do test")]
        public void doTest()
        {
            this.TweenLocalScale(new Vector3(5, 8, 5), 5);
            this.TweenPosition(new Vector3(1, 1, 10), 10);
        }
    }
}