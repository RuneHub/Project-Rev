using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KS { 
    public class UISetText : MonoBehaviour
    {
        [TextArea(2, 3)]
        [SerializeField] private string message = "";

        [Header("Setup")]
        [SerializeField] private UISpriteAssetSO spriteAssets;
        [SerializeField] private DeviceType device;

        private TMP_Text textBox;

        private void Awake()
        {
            textBox = GetComponent<TMP_Text>();
        }

        private void Start()
        {
            SetText();
        }

        [ContextMenu("Set Text")]
        private void SetText()
        {
            if ((int)device > spriteAssets.spriteAssets.Count - 1)
            {
                Debug.LogError($"Missing Sprite asset for {device}");
                return;
            }

            InputBinding oldBinding = PlayerInputManager.controls.UI.Confirm.bindings[(int)device];

            //InputBinding dynamicBinding = PlayerInputManager.GetBinding("UI/Confirm", device);

            Debug.Log("DEVICE: " + device);
            textBox.text = UITextPromptReplacer.ReplaceBinding(
                message,
                PlayerInputManager.controls.UI.Confirm.bindings[(int)device],
                spriteAssets.spriteAssets[(int)device]);
        }

        private enum DeviceType
        {
            MouseAndKeyboard = 0,
            Gamepad = 1
        }

    }
}