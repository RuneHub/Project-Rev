using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KS
{
    public static class UITextPromptReplacer
    {
        public static string ReplaceBinding(string displayText, InputBinding action, TMP_SpriteAsset spriteAsset)
        {

            string sButtonName = action.effectivePath;
            //Debug.Log("effective path: " + sButtonName);

            SettingsUIManager.UIGamepadPrompts current = SettingsUIManager.uiPrompt;

            sButtonName = RenameInput(sButtonName, current);

            displayText = displayText.Replace(
                "[inputPrompt]",
                $"<sprite=\"{spriteAsset.name}\" name=\"{sButtonName}\">");

            return displayText;
        }

        private static string RenameInput(string sButtonName, SettingsUIManager.UIGamepadPrompts controller) 
        {
            sButtonName = sButtonName.Replace(
                "Confirm:", string.Empty);

            sButtonName = sButtonName.Replace(
                "<Mouse>/", "Mouse_");
            sButtonName = sButtonName.Replace(
                "<Keyboard>/", "Keyboard_");

            switch (controller)
            {
                case SettingsUIManager.UIGamepadPrompts.xbox:
                    sButtonName = sButtonName.Replace(
                "<Gamepad>/", "xbox_");
                    break;
                case SettingsUIManager.UIGamepadPrompts.playstation:
                    sButtonName = sButtonName.Replace(
                "<Gamepad>/", "playstation_");
                    break;
            }

            return sButtonName;
        }

    }
}