using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{
    public class UITab : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject selectToReturn;

        public void ReturnSelected()
        {
            eventSystem.SetSelectedGameObject(selectToReturn);
            UIManager.instance.UITabsAreOpen = false;
        }
    }
}