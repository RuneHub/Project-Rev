using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace KS
{
    public class UISkillButton : MonoBehaviour, ISelectHandler
    {
        public PlayerUISkillMenuManager skillManager;
        
        [SerializeField] PlayerSkillsSO skill;
        [Space]
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] List<UISkillSlot> SkillSlotIcon = new List<UISkillSlot>();

        public void SetSkillButton(PlayerSkillsSO skillSO)
        {
            skill = skillSO;
            SetUp();
        }

        private void SetUp()
        {
            title.text = skill.skillName;
            for (int i = 0; i < SkillSlotIcon.Count; i++)
            {
                SkillSlotIcon[i].AbilityIcon.sprite = skill.SkillIconHUD;
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (skill != null)
            {
                skillManager.SetSkillExplaination(skill.skillName, skill.description);
            }
        }



    }
}