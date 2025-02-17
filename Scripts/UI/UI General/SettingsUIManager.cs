using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class SettingsUIManager : BaseUIManager
    {

        public override void OpenMenu()
        {
            base.OpenMenu();
        }

        public override void OpenMenu(GameObject menu)
        {
            base.OpenMenu(menu);
            ReturningInMenu = true;
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
        }

        public override void CloseMenuAfterFixedUpdate()
        {
            base.CloseMenuAfterFixedUpdate();
        }

    }
}