using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

namespace kS { 
public class UICustomAnisotropicTextures : BaseUIOptions
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
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                    break;
                case 1:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                    break;
                case 2:
                    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                    break;
            }

        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }
    }
}