using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class PlayerUICharacterWindowManager : BaseUIManager
    {

        public override void OpenMenu()
        {
            base.OpenMenu();
        }

        public override void OpenMenu(GameObject Menu)
        {
            base.OpenMenu(Menu);
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