using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KS
{
    public class UISetElementToSelectOnInteract : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private Selectable elementToSelect;

        [Header("Visualization")]
        [SerializeField] private bool showVisuals;
        [SerializeField] private Color navigationColour = Color.magenta;

        private void OnDrawGizmos()
        {
            if (!showVisuals)
                return;

            if (elementToSelect == null)
                return;

            Gizmos.color = navigationColour;
            Gizmos.DrawLine(gameObject.transform.position, elementToSelect.gameObject.transform.position);

        }

        private void Reset()
        {
            eventSystem = FindObjectOfType<EventSystem>();

            if (eventSystem == null)
            {
                Debug.Log("Event System NOT Existing in scene!");
            }

        }

        public void JumpToElement()
        {
            if (elementToSelect == null)
                Debug.Log("NO Event System Referenced!");

            if (elementToSelect == null)
                Debug.Log("No Element to jump to!");

            eventSystem.SetSelectedGameObject(elementToSelect.gameObject);
        }

    }
}