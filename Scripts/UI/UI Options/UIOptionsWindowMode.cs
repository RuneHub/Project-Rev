using KS;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{ 
    public class UIOptionsWindowMode : BaseUIOptions, IDeselectHandler
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
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            switch (currentIndex)
            {
                case 0:
                    Screen.fullScreen = true;
                    break;
                case 1:
                    Screen.fullScreen = false;
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
            }
        }
    }
}