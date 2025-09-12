using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace kS
{
    public class UICustomOptionsRenderQuality : BaseUIOptions
    {
        [SerializeField] private UniversalRenderPipelineAsset customRenderAsset;

        protected override void Start()
        {
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
            switch (currentIndex)
            {
                case 0:
                    customRenderAsset.renderScale = 1;
                    break;
                case 1:
                    customRenderAsset.renderScale = 1.5f;
                    break;
                case 2:
                    customRenderAsset.renderScale = 2f;
                    break;
            }

        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }
    }
}