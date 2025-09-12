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

            SetSkillExplaination(combatManager.SkillNorth.skillName, combatManager.SkillNorth.description);

            SetSkillSlotIcon();
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


        #endregion

        public void SetSkillExplaination(string name, string description)
        {
            SkillTitle.text = name;
            SkillDescription.text = description;
        }
    }
}