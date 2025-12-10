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
            cancelSwap();
            base.CloseMenu();
        }

        public override void CloseMenuAfterFixedUpdate()
        {
            cancelSwap();
            base.CloseMenuAfterFixedUpdate();
        }
        #endregion

        #region Set Skills
        //Updates the UI and calls for the HUD to be updated
        private void SetSkillSlotIcon()
        {
            skillButtonN.SetSkillButton(combatManager.SkillNorth);
            skillButtonE.SetSkillButton(combatManager.SkillEast);
            skillButtonS.SetSkillButton(combatManager.SkillSouth);
            skillButtonW.SetSkillButton(combatManager.SkillWest);

            UIManager.instance.SetupHUD();
        }

        //Sets the string of text in the explanation textbox of the UI
        public void SetSkillExplaination(string name, string description)
        {
            SkillTitle.text = name;
            SkillDescription.text = description;
        }
        #endregion

        #region Skill Swap
        //UI function, selects the correct slot of which ability needs to be swapped.
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

        //selects the first swapping slot.
        public void SwappingSelect(UISkillButton selected)
        {
            swappingButton = selected;
        }

        //selects the second slot to be swapped and starts the swapping function.
        public void SwappingToSelect(UISkillButton toSelected)
        {
            swapToButton = toSelected;

            SwapButtons();
        }

        //swaps the skills in the CombatManager, turns off the boolean and Updates the UI & HUD.
        private void SwapButtons()
        {
            combatManager.SwapSkillSlots(swappingButton.skill, swapToButton.skill);
            combatManager.SwapSkillSlots(swapToButton.skill, swappingButton.skill);
            swapping = false;

            SetSkillSlotIcon();
            cancelSwap();
        }

        //empties both swapping slots and sets the bool to false.
        private void cancelSwap()
        {
            swapping = false;
            swappingButton = null;
            swapToButton = null;
            
        }
        #endregion

    }
}