using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KS
{
    public class UICommandBar : MonoBehaviour
    {

        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Prompts")]
        [SerializeField] private Transform barObject;
        [SerializeField] private List<UIInputPrompt> currentMenuPrompts = new List<UIInputPrompt>();
            
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
           
        }

        //Turns Commandbar visible or not
        public void SetCommandBar(bool state)
        {
            if (state)
            {
                canvasGroup.alpha = 1;
            }
            else
            {
                canvasGroup.alpha = 0;
            }
        }

        //instantiate prompt and add it to the list.
        public void AddToCommandBar(UIInputPrompt prompt)
        {
            UIInputPrompt obj = Instantiate(prompt, barObject);
            currentMenuPrompts.Add(obj);
        }

        //remove prompts from the list and destroy the instantiated prompt.
        public void RemoveCurrentPrompts()
        {
            foreach (UIInputPrompt child in currentMenuPrompts)
            {
                currentMenuPrompts.Remove(child);
                Destroy(child.gameObject);
            }

        }

    }
}