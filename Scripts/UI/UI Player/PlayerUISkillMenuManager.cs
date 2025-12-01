using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace KS
{
    public class PlayerUISkillMenuManager : BaseUIManager
    {
        private PlayerCombatManager combatManager;

        [Space]
        [SerializeField] private TextMeshProUGUI SkillTitle;
        [SerializeField] private TextMeshProUGUI SkillDescription;

        [Header("Ability Slots")]
        public UISkillButton skillButtonN;
        public UISkillButton skillButtonE;
        public UISkillButton skillButtonS;
        public UISkillButton skillButtonW;

        [Header("Swap Slots")]
        [SerializeField] private bool swapping;
        [SerializeField] private UISkillButton swappingButton;
        [SerializeField] private UISkillButton swapToButton;

        [Header("Sprites")]
        public Sprite skillSlotIcon_Empty;
        public Sprite skillSlotIcon_NoIcon;

        private void Start()
        {
            combatManager = UIManager.instance.player.combatManager;
        }

        #region Menu's
        public override void OpenMenu()
        {
            base.OpenMenu();
        }
        public override void OpenMenu(GameObject Menu)
        {
            base.OpenMenu(Menu);
            ReturningInMenu = true;

            SetSkillSlotIcon();
            SetSkillExplaination(combatManager.SkillNorth.skillName, combatManager.SkillNorth.description);

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

        #region Set Skills
        private void SetSkillSlotIcon()
        {
            skillButtonN.SetSkillButton(combatManager.SkillNorth);
            skillButtonE.SetSkillButton(combatManager.SkillEast);
            skillButtonS.SetSkillButton(combatManager.SkillSouth);
            skillButtonW.SetSkillButton(combatManager.SkillWest);
        }
        public void SetSkillExplaination(string name, string description)
        {
            SkillTitle.text = name;
            SkillDescription.text = description;
        }
        #endregion

        #region Skill Swap
        public void SelectedButton(UISkillButton selected)
        {
            if (swapping)
            {
                SwappingToSelect(selected);
            }
            else
            {
                SwappingSelect(selected);
                swapping = true;
            }
        }

        public void SwappingSelect(UISkillButton selected)
        {
            swappingButton = selected;
        }

        public void SwappingToSelect(UISkillButton toSelected)
        {
            swapToButton = toSelected;

            SwapButtons();
        }

        private void SwapButtons()
        {
            UISkillButton tempButton = swappingButton;

            swappingButton = swapToButton;

            swapToButton = tempButton;

            swapping = false;

            SetSkillSlotIcon();
            SetSkillExplaination(combatManager.SkillNorth.skillName, combatManager.SkillNorth.description);

        }

        private void cancelSwap()
        {
            swapping = false;
            swappingButton = null;
            swapToButton = null;
            
        }
        #endregion

    }
}