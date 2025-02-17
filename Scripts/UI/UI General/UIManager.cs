using kS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS {
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public PlayerManager player;
        public PlayerUIHUDManager hudManager;
        public FloatingUIManager floatingManager;
        public PlayerUIMenuManager menuManager;
        public PlayerUICharacterWindowManager characterWindowManager;
        public PlayerUIPopUpManager popupManager;
        public SettingsUIManager settingsManager;

        [Header("UI")]
        public bool menuWindowIsOpen = false;
        public bool gameplayMenuIsOpen = false;
        public bool popUpWindowisOpen = false;

        public BaseUIManager currentOpenMenu;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            { 
                Destroy(gameObject);
            }

            player = FindObjectOfType<PlayerManager>();
            hudManager = GetComponentInChildren<PlayerUIHUDManager>();
            floatingManager = FindObjectOfType<FloatingUIManager>();
            menuManager = GetComponentInChildren<PlayerUIMenuManager>();
            characterWindowManager = GetComponentInChildren<PlayerUICharacterWindowManager>();
            popupManager = GetComponentInChildren<PlayerUIPopUpManager>();
            settingsManager = FindObjectOfType<SettingsUIManager>();
        }

        public void SetupHUD()
        {
            hudManager.SetupPlayerVitality();
            hudManager.SetSkillSlotIcon();
            hudManager.SetUpHealingItems();
        }

        public void CloseAllMenuWindows()
        {
            menuManager.CloseMenu();
            characterWindowManager.CloseMenu();
            settingsManager.CloseMenu();
            menuWindowIsOpen = false;
            gameplayMenuIsOpen= false;
            Time.timeScale = 1;
            LockCursor();
        }

        public void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}