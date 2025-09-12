using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace KS
{

    public class UIRememberCurrentSelected : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GameObject lastSelectedElement;

        private void Reset()
        {
            eventSystem = FindAnyObjectByType<EventSystem>();

            if (!eventSystem)
            {
                Debug.Log("NO available Event System");
                return;
            }

            lastSelectedElement = eventSystem.firstSelectedGameObject;
        }

        private void Update()
        {
            if (!eventSystem)
                return;

            if (eventSystem.currentSelectedGameObject && lastSelectedElement != eventSystem.currentSelectedGameObject)
            {
                lastSelectedElement = eventSystem.currentSelectedGameObject;
            }

            if (!eventSystem.currentSelectedGameObject && lastSelectedElement)
            {
                eventSystem.SetSelectedGameObject(lastSelectedElement);
            }

        }

        public bool CheckSelected()
        {
            if (lastSelectedElement == eventSystem.currentSelectedGameObject)
                return true;
            else
                return false;
        }

        public GameObject GetSelected()
        {
            return eventSystem.currentSelectedGameObject;
        }

    }
}