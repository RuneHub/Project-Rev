using KS.SHADING;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class GetCharacterTheme : MonoBehaviour
    {
        [SerializeField] PlayerUICharacterWindowManager characterUI;
        public ShaderMaterial shading;

        //grabs the current theme from the UI & applies it to the character
        public void UpdateCharacterTheme()
        {
            characterUI.GetCurrentTheme();
            shading.character.theme = characterUI.currentTheme.characterTheme;
            shading.UniqueMaterials();
        }
    }
}