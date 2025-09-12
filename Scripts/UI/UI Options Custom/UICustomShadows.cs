using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

namespace KS
{
    public class UICustomShadows : BaseUIOptions
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
                    GetGraphicSetting.MainLightCastShadows = false;
                    customRenderAsset.shadowCascadeCount = 1;
                    GetGraphicSetting.SoftShadowsEnabled = false;
                    break;
                case 1:
                    GetGraphicSetting.MainLightCastShadows = true;
                    GetGraphicSetting.MainLightShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._1024;
                    customRenderAsset.shadowDistance = 50;
                    customRenderAsset.shadowCascadeCount = 1;
                    GetGraphicSetting.SoftShadowsEnabled = true;
                    GetGraphicSetting.SoftShadowQualitySetting = SoftShadowQuality.Low;
                    break;
                case 2:
                    GetGraphicSetting.MainLightCastShadows = true;
                    GetGraphicSetting.MainLightShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._2048;
                    customRenderAsset.shadowDistance = 75;
                    customRenderAsset.shadowCascadeCount = 2;
                    GetGraphicSetting.SoftShadowsEnabled = true;
                    GetGraphicSetting.SoftShadowQualitySetting = SoftShadowQuality.Medium;
                    break;
                case 3:
                    GetGraphicSetting.MainLightCastShadows = true;
                    GetGraphicSetting.MainLightShadowResolution = UnityEngine.Rendering.Universal.ShadowResolution._4096;
                    customRenderAsset.shadowDistance = 100;
                    customRenderAsset.shadowCascadeCount = 3;
                    GetGraphicSetting.SoftShadowsEnabled = true;
                    GetGraphicSetting.SoftShadowQualitySetting = SoftShadowQuality.High;
                    break;
            }

        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }
    }
}