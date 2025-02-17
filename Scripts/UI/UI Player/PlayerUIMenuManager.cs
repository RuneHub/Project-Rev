using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS { 
    public class PlayerUIMenuManager : BaseUIManager
    {

        public override void OpenMenu()
        {
            Debug.Log("opening gameplay menu");
            UIManager.instance.menuWindowIsOpen = true;
            UIManager.instance.gameplayMenuIsOpen = true;
            UIManager.instance.UnlockCursor();
            UIManager.instance.player.inputs.DisableGameplayInput();
            base.OpenMenu();
            Time.timeScale = 0;
        }

        public override void CloseMenu()
        {
            UIManager.instance.gameplayMenuIsOpen = false;
            UIManager.instance.LockCursor();
            UIManager.instance.player.inputs.EnableGameplayInput();
            base.CloseMenu();
        }

        public override void CloseMenuAfterFixedUpdate()
        {
            UIManager.instance.gameplayMenuIsOpen = false;
            base.CloseMenuAfterFixedUpdate();
        }
    }
}
