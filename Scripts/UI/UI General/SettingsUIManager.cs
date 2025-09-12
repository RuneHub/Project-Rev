using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class SettingsUIManager : BaseUIManager
    {
        [SerializeField] private UITabSystem tabSystem;
        public enum UIGamepadPrompts { xbox, playstation }
        public static UIGamepadPrompts uiPrompt;

        public override void OpenMenu()
        {
            UIManager.instance.menuWindowIsOpen = true;
            base.OpenMenu();
        }

        public override void OpenMenu(GameObject menu)
        {
            UIManager.instance.menuWindowIsOpen = true;
            base.OpenMenu(menu);
            ReturningInMenu = true;
        }

        public override void CloseMenu()
        {
            UIManager.instance.menuWindowIsOpen = false;
            base.CloseMenu();
            tabSystem.CloseAllTabs();
        }

        public override void CloseMenuAfterFixedUpdate()
        {
            UIManager.instance.menuWindowIsOpen = false;
            base.CloseMenuAfterFixedUpdate();
            tabSystem.CloseAllTabs();
        }

    }
}