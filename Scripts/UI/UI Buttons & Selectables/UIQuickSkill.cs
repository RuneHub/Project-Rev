using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace KS 
{ 
    public class UIQuickSkill : MonoBehaviour
    {
        [SerializeField] Image skillIcon;
        [SerializeField] TextMeshProUGUI skillName;

        public void SetQuickSkill(PlayerSkillsSO skill)
        {
            skillIcon.sprite = skill.SkillIconHUD;
            skillName.text = skill.skillName;
        }

    }
}