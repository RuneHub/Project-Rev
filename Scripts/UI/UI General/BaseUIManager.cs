using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class BaseUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject menu; 
        [SerializeField] private GameObject PreviousMenu;
        public bool ReturningInMenu;

        public virtual void OpenMenu()
        {
            UIManager.instance.currentOpenMenu = this;
            UIManager.instance.hudManager.ToggleHUD(false);
            menu.SetActive(true);
        }

        public virtual void OpenMenu(GameObject Menu)
        {
            UIManager.instance.currentOpenMenu = this;
            UIManager.instance.hudManager.ToggleHUD(false);
            menu.SetActive(true);
            PreviousMenu = Menu;
            Debug.Log("prevMenu: " + PreviousMenu.name);
        }

        public virtual void CloseMenu()
        {
            UIManager.instance.hudManager.ToggleHUD(true);
            menu.SetActive(false);

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

            if (ReturningInMenu)
            {
                ReturningInMenu = false;
                BaseUIManager bui = PreviousMenu.GetComponentInParent<BaseUIManager>();
                bui.OpenMenu(); 
            }

        }

    }
}