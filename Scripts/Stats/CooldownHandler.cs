using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace KS
{
    public class CooldownHandler : MonoBehaviour
    {
        public static CooldownHandler instance;
        public float cooldownSpeed = 1;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        [SerializeField] private List<CooldownData> skillsOnCD = new List<CooldownData>();

        //create cooldown data sets
        private class CooldownData
        {
            public string skillID;
            public float cooldown;

            public CooldownData(string skill, float cooldown)
            {
                this.skillID = skill;
                this.cooldown = cooldown;
            }
        }

        //checks for the cooldown time and removes them if they are done.
        private void Update()
        {
            for (int i = 0; i < skillsOnCD.Count; i++)
            {
                skillsOnCD[i].cooldown -= Time.deltaTime * cooldownSpeed;
            }

            for (int i = skillsOnCD.Count - 1; i >= 0; i--)
            {
                if (skillsOnCD[i].cooldown <= 0)
                {
                    skillsOnCD.RemoveAt(i);
                }
            }

        }

        //creates new data set of the received skill
        //adds it to the cooldown list.
        public void PutOnCooldown(PlayerSkillsSO skill)
        {
            skillsOnCD.Add(new CooldownData(skill.skillID, skill.cooldown));
        }

        //checks via ID if they skill is on cooldown
        //returns true or false
        public bool isOnCooldown(string ID)
        {
            foreach (CooldownData data in skillsOnCD)
            {
                if (data.skillID == ID)
                {
                    return true;
                }
            }
            return false;
        }

        //returns the cooldown time, if doesn't exists returns 0
        public float GetCooldownTimer(string ID)
        {
            foreach (CooldownData data in skillsOnCD)
            {
                if (data.skillID == ID)
                {
                    return data.cooldown;
                }
            }

            return 0;
        }

    }
}