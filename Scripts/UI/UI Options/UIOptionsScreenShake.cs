using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class UIOptionsScreenShake : BaseUIOptions
    {
        private PlayerManager playerManager;

        protected override void Start()
        {
            playerManager = FindObjectOfType<PlayerManager>();
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
                playerManager.ScreenShake = true;
            else if(currentIndex == 1)
                playerManager.ScreenShake = false;
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

    }
}