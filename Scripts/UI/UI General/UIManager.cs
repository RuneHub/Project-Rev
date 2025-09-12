using kS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS 
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public PlayerManager player;
        public PlayerUIHUDManager hudManager;
        public FloatingUIManager floatingManager;
        public PlayerUIMenuManager menuManager;
        public PlayerUICharacterWindowManager characterWindowManager;
        public PlayerUISkillMenuManager skillMenuManager;
        public PlayerUIPopUpManager popupManager;
        public SettingsUIManager settingsManager;
        public UITab currentTab;

        [Header("UI")]
        public bool titleWindowIsOpen = false;
        public bool menuWindowIsOpen = false;
        public bool gameplayMenuIsOpen = false;
        public bool popUpWindowIsOpen = false;
        public bool UITabsAreOpen = false;

        public BaseUIManager currentOpenMenu;

        public GameObject guideBar;

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
            skillMenuManager = GetComponentInChildren<PlayerUISkillMenuManager>();
            popupManager = GetComponentInChildren<PlayerUIPopUpManager>();
            settingsManager = FindObjectOfType<SettingsUIManager>();

            guideBar.SetActive(false);
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
            skillMenuManager.CloseMenu();
            settingsManager.CloseMenu();
            menuWindowIsOpen = false;
            gameplayMenuIsOpen= false;
            Time.timeScale = 1;
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

        private void Update()
        {
            if (menuWindowIsOpen)
                guideBar.SetActive(true);
            else
                guideBar.SetActive(false);
        }
    }
}