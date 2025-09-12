using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace KS
{
    public class UIOptionsBloom : BaseUIOptions
    {
        [SerializeField] private Volume postProcessObject;

        private Bloom bloom;

        protected override void Start()
        {
            postProcessObject.profile.TryGet(out bloom);

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
            if (currentIndex == 0)
                bloom.active = true;
            else if (currentIndex == 1)
                bloom.active = false;
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

    }
}