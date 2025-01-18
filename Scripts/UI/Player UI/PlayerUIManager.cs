using kS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KS {
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        public PlayerManager player;
        public PlayerUIHUDManager hudManager;
        public FloatingUIManager floatingManager;
        public PlayerUIMenuManager menuManager;
        public PlayerUICharacterWindowManager characterWindowManager;
        public PlayerUIPopUpManager popupManager;

        [Header("UI")]
        public bool menuWindowIsOpen = false;
        public bool popUpWindowisOpen = false;

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
        }

        private void Start()
        {
            LockCursor();
        }

        private void Update()
        {
            
        }

        public void SetupHUD()
        {
            hudManager.SetupPlayerVitality();
            hudManager.SetSkillSlotIcon();
        }

        public void CloseAllMenuWindows()
        {
            menuManager.CloseGameMenu();
            characterWindowManager.CloseCharacterMenu();

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