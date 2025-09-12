using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class UIOptionsGamepadUI : BaseUIOptions
    {
        public static event Action GamepadUIChanged;

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
                SettingsUIManager.uiPrompt = SettingsUIManager.UIGamepadPrompts.xbox;
            else if (currentIndex == 1)
                SettingsUIManager.uiPrompt = SettingsUIManager.UIGamepadPrompts.playstation;

            GamepadUIChanged?.Invoke();
        }

        public override void OnMove(AxisEventData eventData)
        {
            base.OnMove(eventData);
        }
    }
}