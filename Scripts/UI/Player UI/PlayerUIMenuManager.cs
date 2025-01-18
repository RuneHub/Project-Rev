using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS { 
    public class PlayerUIMenuManager : MonoBehaviour
    {
        [Header("Menu")]
        [SerializeField] GameObject menu;

        public void OpenGameMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = true;
            menu.SetActive(true);
            PlayerUIManager.instance.UnlockCursor();
        }

        public void CloseGameMenu()
        {
            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);
            PlayerUIManager.instance.LockCursor();
        }

        public void CloseMainMenuAfterFixedUpdate()
        {
            StartCoroutine(WaitThenCloseMenu());
        }

        private IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();

            PlayerUIManager.instance.menuWindowIsOpen = false;
            menu.SetActive(false);
        }

    }
}
