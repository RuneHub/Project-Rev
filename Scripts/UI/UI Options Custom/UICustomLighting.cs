using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

namespace KS { 
public class UICustomLighting : BaseUIOptions
    {

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
                    QualitySettings.realtimeGICPUUsage = (int)RealtimeGICPUUsage.Low;
                    break;
                case 1:
                    QualitySettings.realtimeGICPUUsage = (int)RealtimeGICPUUsage.Medium;
                    break;
                case 2:
                    QualitySettings.realtimeGICPUUsage = (int)RealtimeGICPUUsage.High;
                    break;
                case 3:
                    QualitySettings.realtimeGICPUUsage = (int)RealtimeGICPUUsage.Unlimited;
                    break;
            }

        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }
    }
}