using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class Interactible : MonoBehaviour
    {
        public string interactibleText;
        public bool isInteractable = true;
        [SerializeField] protected Collider interactibleCollider;
        
        protected virtual void Awake()
        {
            //failsafe for when it is not assigned in the editor
            if (interactibleCollider == null)
                interactibleCollider = GetComponent<Collider>();

        }

        protected virtual void Start()
        {
            
        }

        public virtual void Interact(PlayerManager player)
        {
            if (isInteractable)
            {
                Debug.Log("YOU HAVE INTERACTED!!");

                interactibleCollider.enabled = false;
                player.interactionManager.RemoveInteractionFromList(this);
                UIManager.instance.popupManager.CloseAllPopUpWindows();
                isInteractable = false;
            }
        }

        public virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            
            if (isInteractable)
            {
                if (player != null)
                {
                    //pass interaction to player.
                    player.interactionManager.AddInteractionToList(this);
                }
            }

        }

        public virtual void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (isInteractable)
            {
                if (player != null)
                {
                    //pass interaction to player.
                    player.interactionManager.RemoveInteractionFromList(this);
                    UIManager.instance.popupManager.CloseAllPopUpWindows();
                }
            }

        }

    }
}