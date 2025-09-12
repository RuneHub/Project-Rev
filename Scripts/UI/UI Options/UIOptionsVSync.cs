using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class UIOptionsVSync : BaseUIOptions
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

            if (currentIndex == 0)
            { 
                QualitySettings.vSyncCount = 0;
            }
            else
            {
                QualitySettings.vSyncCount = 1;
            }

        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

    }
}