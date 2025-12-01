using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class BaseUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject menu; 
        [SerializeField] private GameObject PreviousMenu;
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject selectedOnOpen;
        [SerializeField] private GameObject lastSelected;
        public bool ReturningInMenu;
        
        [Space(10)]
        [SerializeField] private GameObject selectedVFX;
        [SerializeField] private bool useCommandBar;
        [SerializeField] private UICommandBar commandBar;
        [SerializeField] private bool useCustomPrompts;
        [SerializeField] private List<UIInputPrompt> promptList;
        
        public virtual void OpenMenu()
        {
            UIManager.instance.currentOpenMenu = this;
            UIManager.instance.hudManager.ToggleHUD(false);
            menu.SetActive(true);
            eventSystem.SetSelectedGameObject(selectedOnOpen);

            SetCommandBar();
        }

        public virtual void OpenMenu(GameObject Menu)
        {
            UIManager.instance.currentOpenMenu = this;
            UIManager.instance.hudManager.ToggleHUD(false);
            menu.SetActive(true);
            PreviousMenu = Menu;
            Debug.Log("prevMenu: " + PreviousMenu.name);
            eventSystem.SetSelectedGameObject(selectedOnOpen);

            SetCommandBar();
        }

        public virtual void CloseMenu()
        {
            UIManager.instance.hudManager.ToggleHUD(true);
            menu.SetActive(false);

            ClearCommandBar();

            if (ReturningInMenu)
            {
                ReturningInMenu = false;
                BaseUIManager bui = PreviousMenu.GetComponentInParent<BaseUIManager>();
                bui.OpenMenu();
            }

        }

        public virtual void CloseMenuAfterFixedUpdate()
        {
            StartCoroutine(WaitThenCloseMenu());
        }

        protected virtual IEnumerator WaitThenCloseMenu()
        {
            yield return new WaitForFixedUpdate();
            
            menu.SetActive(false);

            ClearCommandBar();

            if (ReturningInMenu)
            {
                ReturningInMenu = false;
                BaseUIManager bui = PreviousMenu.GetComponentInParent<BaseUIManager>();
                bui.OpenMenu(); 
            }

        }

        public GameObject GetSelectedVFX()
        {
            return selectedVFX;
        }

        public GameObject GetSelectOnOpen()
        {
            return selectedOnOpen;
        }

        //adds all the prompts for the current menu.
        private void SetCommandBar()
        {
            commandBar.SetCommandBar(useCommandBar);

            if (useCommandBar && useCustomPrompts)
            {
                for (int i = 0; i < promptList.Count; i++)
                {
                    commandBar.AddToCommandBar(promptList[i]);
                }
            }
        }

        //removes all of the added prompts from the current menu.
        private void ClearCommandBar()
        {
            if (useCommandBar && useCustomPrompts)
            {
                commandBar.RemoveCurrentPrompts();
            }

        }

    }
}