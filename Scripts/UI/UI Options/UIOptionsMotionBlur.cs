using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace KS
{
    public class UIOptionsMotionBlur : BaseUIOptions
    {
        [SerializeField] private Volume postProcessObject;

        [Header("Effects")]
        private MotionBlur motionBlur;

        protected override void Start()
        {
            postProcessObject.profile.TryGet(out motionBlur);

            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }
        protected override void NextOption()
        {
            base.NextOption();
        }

        protected override void PrevOption()
        {
            base.PrevOption();
        }

        protected override void SetOption()
        {
            base.SetOption();
            if(currentIndex == 0)
                motionBlur.active = true;
            else if(currentIndex == 1)
                motionBlur.active = false;
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

    }
}