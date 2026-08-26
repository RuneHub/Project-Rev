using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;


namespace KS
{
    public class UIControllerLayoutButton : MonoBehaviour
    {
        [Header("Layout")]
        public bool UpdateButton = true;
        public string ButtonPath;
        [SerializeField] private TMP_Text inputTextField;

        [Range(0, 10), SerializeField] private int selectBinding;

        [Header("Binding Info")]
        [SerializeField] private InputBinding inputBinding;
        private int bindingIndex;
        private string actionName;

        [Header("Override")]
        public InputAction overrideAction;
        public bool overrideActionAvailable = false;

        public void UpdateLayout()
        {
            CheckActiveInputs();
        }

        private void CheckActiveInputs()
        {
            //Debug.Log("-----------------------");
            InputAction action;

            if (overrideActionAvailable)
            {
                action = overrideAction;
            }
            else
            {
                action = PlayerInputManager.GetInputAction(ButtonPath);
            }

            if (action != null)
            {
                //Debug.Log("binding action on path " + ButtonPath);
                inputTextField.text = AddSpaces(action.name);

                inputBinding = action.bindings[selectBinding];
                bindingIndex = selectBinding;
            }
            else if (action == null)
            {
                inputTextField.text = " - ";
                //Debug.Log("no binded action on path " + ButtonPath);
            }

            if (overrideActionAvailable) 
            {
                overrideActionAvailable = false;
                overrideAction = null;
            }

        }

        private string AddSpaces(string text)
        {
            const string pattern = "(?<=[a-z])(?=[A-Z0-9])|(?<=[A-Z])(?=[0-9])|(?<=[A-Z0-9])(?=[A-Z][a-z])";
            return Regex.Replace(text, pattern, " ");
        }

    }
}