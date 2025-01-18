using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class PlayerUIToggle : MonoBehaviour
    {
        private void OnEnable()
        {
            //Hide the HUD.
            PlayerUIManager.instance.hudManager.ToggleHUD(false);
        }

        private void OnDisable()
        {
            //Show the hud
            PlayerUIManager.instance.hudManager.ToggleHUD(true);
        }

    }
}