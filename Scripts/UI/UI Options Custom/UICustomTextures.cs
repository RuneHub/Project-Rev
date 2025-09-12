using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;

namespace KS { 
public class UICustomTexturesUICustomOptionsRenderQuality : BaseUIOptions
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
                    QualitySettings.globalTextureMipmapLimit = 3;
                    break;
                case 1:
                    QualitySettings.globalTextureMipmapLimit = 2;
                    break;
                case 2:
                    QualitySettings.globalTextureMipmapLimit = 1;
                    break;
                case 3:
                    QualitySettings.globalTextureMipmapLimit = 0;
                    break;
            }
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }
    }
}