using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{ 
    public class PlayerInteractionManager : MonoBehaviour
    {
        PlayerManager player;

        [SerializeField] private List<Interactible> currentInteractibleActions;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }
        private void Start()
        {
            
        }

        private void FixedUpdate()
        {
            //if our UI Menu is NOT open, and we don't have a POP UP (current interaction message) check for interactible.
            if (!UIManager.instance.menuWindowIsOpen &&
                !UIManager.instance.popUpWindowisOpen)
            {
                CheckForInteractable(); 
            }

        }

        private void CheckForInteractable()
        {
            if (currentInteractibleActions.Count == 0)
                return;

            if (currentInteractibleActions[0] == null)
            {
                //failsafe for when in-world item is removed, but listed item still exists.
                currentInteractibleActions.RemoveAt(0);
            }

            //when interactable exists but not has been notified.
            if (currentInteractibleActions[0] != null)
            {
                // popup manager sent player message.
                UIManager.instance.popupManager.SendPlayerMessagePopUp(currentInteractibleActions[0].interactibleText);
            }

        }

        private void RefreshInteractionList()
        {
            for (int i = currentInteractibleActions.Count - 1; i > -1; i--)
            {
                if (currentInteractibleActions[i] == null)
                {
                    currentInteractibleActions.RemoveAt(i);
                }
            }
        }

        public void AddInteractionToList(Interactible interactable)
        {
            if (!interactable.isInteractable)
                return;

            RefreshInteractionList();

            if (!currentInteractibleActions.Contains(interactable))
                currentInteractibleActions.Add(interactable);
        }

        public void RemoveInteractionFromList(Interactible interactable)
        {
            if (currentInteractibleActions.Contains(interactable))
                currentInteractibleActions.Remove(interactable);

            RefreshInteractionList();

        }

        public void Interact()
        {
            if (currentInteractibleActions.Count <= 0)
                return;

            if (currentInteractibleActions[0] != null)
            {
                currentInteractibleActions[0].Interact(player);
                RefreshInteractionList();
            }
        }


    }
}