using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KS { 
    public class PlayerUIMenuManager : BaseUIManager
    {
        [Space]
        [SerializeField] private UIQuickSkill QSN;
        [SerializeField] private UIQuickSkill QSE;
        [SerializeField] private UIQuickSkill QSS;
        [SerializeField] private UIQuickSkill QSW;

        public override void OpenMenu()
        {
            Debug.Log("opening gameplay menu");
            UIManager.instance.menuWindowIsOpen = true;
            UIManager.instance.gameplayMenuIsOpen = true;
            UIManager.instance.player.inputs.DisableGameplayInput();
            base.OpenMenu();
            Time.timeScale = 0;           
            GetSelectOnOpen().GetComponent<UIButtonOnSelect>().ManuallySelect(true);

            QSN.SetQuickSkill(UIManager.instance.player.combatManager.SkillNorth);
            QSE.SetQuickSkill(UIManager.instance.player.combatManager.SkillEast);
            QSS.SetQuickSkill(UIManager.instance.player.combatManager.SkillSouth);
            QSW.SetQuickSkill(UIManager.instance.player.combatManager.SkillWest);
        }

        public override void CloseMenu()
        {
            UIManager.instance.gameplayMenuIsOpen = false;
            UIManager.instance.player.inputs.EnableGameplayInput();
            base.CloseMenu();
        }

        public override void CloseMenuAfterFixedUpdate()
        {
            UIManager.instance.gameplayMenuIsOpen = false;
            base.CloseMenuAfterFixedUpdate();
        }

    }
}
