using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class UIOptionsFPS : BaseUIOptions
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

            Application.targetFrameRate = currentIndex;
            QualitySettings.vSyncCount = 0;

        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }
    }
}