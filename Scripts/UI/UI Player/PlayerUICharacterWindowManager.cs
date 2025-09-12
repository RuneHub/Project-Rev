using KS.SHADING;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KS
{
    public class PlayerUICharacterWindowManager : BaseUIManager
    {
        [Header("Character Theme")]
        public ShaderMaterial shading;

        public CharacterUIThemeAsset currentTheme;
        public List<CharacterUIThemeAsset> characterThemes;

        [SerializeField] private CharacterThemeAsset currentThemeAsset;
        [SerializeField] private int index;

        [SerializeField] private Image UICharacterPreview;
        [SerializeField] private Image UILeftWeaponPreview;
        [SerializeField] private Image UIRightWeaponPreview;
        [SerializeField] private TextMeshProUGUI UIThemeName;
        [SerializeField] private Image UIThemeImage;

        [Header("Character Status")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI attackText;

        #region Menu's
        public override void OpenMenu()
        {
            base.OpenMenu();

            GetCurrentTheme();
            GetStatus();
        }

        public override void OpenMenu(GameObject Menu)
        {
            base.OpenMenu(Menu);
            ReturningInMenu = true;

            GetCurrentTheme();
            GetStatus();
        }

        public override void CloseMenu()
        {
            base.CloseMenu();
        }

        public override void CloseMenuAfterFixedUpdate()
        {
            base.CloseMenuAfterFixedUpdate();
        }
        #endregion

        #region Character Theme

        //get the current theme of the character,
        //once found sets the correct index & updates the UI.
        public void GetCurrentTheme()
        {
            currentThemeAsset = shading.character.theme;
            for (int i = 0; i < characterThemes.Count; i++)
            {
                if (currentThemeAsset == characterThemes[i].characterTheme)
                {
                    currentTheme = characterThemes[i];
                    index = i;
                }
            }

            UpdateCharacterUI();
        }

        //Updates the UI to the current theme.
        public void UpdateCharacterUI()
        {
            UICharacterPreview.sprite = currentTheme.UICharacterPreview;
            //UILeftWeaponPreview.sprite = currentTheme.LeftWeaponPreview;
            //UIRightWeaponPreview.sprite = currentTheme.RightWeaponPreview;
            UIThemeName.text = currentTheme.ThemeName;
            UIThemeImage.sprite = currentTheme.UICharacterThemeImage;
        }

        public void CycleRight()
        {
            index++;
            CycleThemes();
        }

        public void CycleLeft()
        {
            index--;
            CycleThemes();
        }

        //cycles the the list depending on the index, sets the current theme.
        //Updates the UI & the character mesh materials.
        private void CycleThemes()
        {
            index = index < 0 ? characterThemes.Count - 1 : index >= characterThemes.Count ? 0 : index;
            currentTheme = characterThemes[index];

            UpdateCharacterUI();

            //update character based on theme
            shading.character.theme = currentTheme.characterTheme;
            shading.UniqueMaterials();
        }

        #endregion

        #region Character Status

        private void GetStatus()
        {
            levelText.text = playerManager.playerStats.Level.ToString();
            healthText.text = playerManager.playerStats.currentHealth.ToString();
            attackText.text = playerManager.playerStats.baseAttack.ToString();
        }

        #endregion
    }
}