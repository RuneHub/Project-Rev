using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace KS
{
    public class RebindUI : MonoBehaviour
    {
        [SerializeField] private InputActionReference InputReference;
        [SerializeField] private bool excludeMouse = true;
        [Range(0, 10)][SerializeField] private int selectBinding;
        [SerializeField] private InputBinding.DisplayStringOptions displayStringOptions;
        
        [Header("Binding Info")]
        [SerializeField] private InputBinding InputBinding;
        private int bindingIndex;

        private string actionName;

        [Header("UI Fields")]
        [SerializeField] private bool updateActionText = true;
        [SerializeField] private TextMeshProUGUI actionText;
        [SerializeField] private Button rebindButton;
        [SerializeField] private TextMeshProUGUI rebindText;
        [SerializeField] private Button resetButton;

        private void OnEnable()
        {
            rebindButton.onClick.AddListener(() => DoRebind());

            PlayerInputManager.rebindComplete += UpdateUI;
            PlayerInputManager.rebindCanceled += UpdateUI;

        }

        private void Start()
        {
            if (InputReference != null)
            {
                GetBindingInfo();
                PlayerInputManager.LoadBindingOverride(actionName);
                UpdateUI();
            }
        }

        private void OnDisable()
        {
            PlayerInputManager.rebindComplete -= UpdateUI;
            PlayerInputManager.rebindCanceled -= UpdateUI;
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
                if (PlayerInputManager.controls == null)
                    return;

            if (InputReference == null)
                return;

            GetBindingInfo();
            UpdateUI();

        }

        private void GetBindingInfo()
        {
            if (InputReference.action != null)
                actionName = InputReference.action.name;

            if (InputReference.action.bindings.Count > selectBinding)
            {
                InputBinding = InputReference.action.bindings[selectBinding];
                bindingIndex = selectBinding;
            }

        }

        private void UpdateUI()
        {
            if (actionText != null && updateActionText)
            {
                actionText.text = actionName;
            }

            if (rebindText != null)
            {
                if (Application.isPlaying)
                {
                    rebindText.text = PlayerInputManager.GetBindingName(actionName, bindingIndex);
                }
                else
                {
                    rebindText.text = InputReference.action.GetBindingDisplayString(bindingIndex);
                }
            }

        }

        private void DoRebind()
        {
            PlayerInputManager.StartRebind(actionName, bindingIndex, rebindText, excludeMouse);
        }

        public void ResetBinding()
        {
            PlayerInputManager.ResetBindings(actionName, bindingIndex);
            UpdateUI();
        }

    }
}