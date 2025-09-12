using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class UITabSystem : MonoBehaviour
    {

        [SerializeField] private List<UITab> Tabs = new List<UITab>();

        public void OpenTab(UITab tab)
        {
            for (int i = 0; i < Tabs.Count; i++)
            {
                if (tab == Tabs[i])
                {
                    Tabs[i].gameObject.SetActive(true);
                    UIManager.instance.UITabsAreOpen = true;
                    UIManager.instance.currentTab = Tabs[i];
                }
                else
                {
                    Tabs[i].gameObject.SetActive(false);
                }
            }
        }

        public void CloseTab(UITab tab)
        {
            tab.gameObject.SetActive(false);
        }

        public void CloseAllTabs()
        {
            for (int i = 0; i < Tabs.Count; i++)
            {
                Tabs[i].gameObject.SetActive(false);
                UIManager.instance.UITabsAreOpen = false;
            }
        }


    }
}