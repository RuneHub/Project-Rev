using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KS
{
    public class UIControllerLayoutManager : MonoBehaviour
    {
        public List<UIControllerLayoutButton> layoutButtons = new List<UIControllerLayoutButton>();

        private void Awake()
        {

            Transform[] Children = GetComponentsInChildren<Transform>();

            foreach (var child in Children)
            {
                if (child.GetComponent<UIControllerLayoutButton>() != null)
                {
                    layoutButtons.Add(child.GetComponent<UIControllerLayoutButton>());
                }
            }
            
        }

        private void OnEnable()
        {
            UpdateLayoutButtons();

        }

        public void UpdateLayoutButtons()
        {
            Debug.Log("Update layout");
            if (layoutButtons.Count > 0)
            {
                for (int i = 0; i < layoutButtons.Count; i++)
                {
                    if (layoutButtons[i].UpdateButton)
                    {
                        layoutButtons[i].UpdateLayout();
                    }
                }
            }
        }

        public void SetOverrideLayout(string path, InputAction action)
        {
            for (int i = 0; i < layoutButtons.Count; i++) 
            {
                if (layoutButtons[i].ButtonPath == path)
                {
                    layoutButtons[i].overrideAction = action;
                    layoutButtons[i].overrideActionAvailable = true;
                    layoutButtons[i].UpdateLayout();
                }
            }

        }


    }
}