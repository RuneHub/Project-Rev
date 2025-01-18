using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace KS {
    public static class StatCalculator
    {
        private static List<StatusEffectsSO> SEAttacker;
        private static List<StatusEffectsSO> SETarget;

        //calaculate base stats depending on the level.
        public static float CalculateStats(int currentLevel, float minLevelStat, float maxLevelStat)
        {
            return Mathf.Floor((float)((.8 + currentLevel / 50) * currentLevel * ((maxLevelStat - minLevelStat) / 275.222) + minLevelStat));
        }

        //Calculate the damage
        public static (float dmg, bool isCrit) CalculateDamage(CharacterManager attacker, float curAtkPer, CharacterManager target)
        {
            float finalizedDamage;
            float baseDamage;
            float totalDefense;
            bool isCrit = false;

            SEAttacker = new List<StatusEffectsSO>(attacker.charStatManager.statusEffects);
            SETarget = new List<StatusEffectsSO>(target.charStatManager.statusEffects);

            //get the base damage
            if (GetPercentageChance(1, 100, attacker.charStatManager.CriticalHitRate))
            {
                float atk = GetTotalCharacterAttack(attacker);
                baseDamage =  ((atk/100)* attacker.charStatManager.CriticalHitBuff) + atk;
                isCrit = true;
            }
            else
            {
                baseDamage = GetTotalCharacterAttack(attacker);
            }

            //add the attacks percentage
            //Debug.Log("basedamage: " + baseDamage);
            baseDamage = ((baseDamage / 100) * curAtkPer) + baseDamage;
            //Debug.Log("basedamage+: " + baseDamage);

            //get the total Defense
            totalDefense = GetTotalDefense(target);

            //actual finalized damage
            float randomMod = GetRandomNumber(0.95f, 1.05f);

            finalizedDamage = Mathf.Round((baseDamage / totalDefense) * randomMod);

            //clean lists
            ClearSELists();

            return (finalizedDamage, isCrit);
        }

        //handles the status effect list, it checks if a status effect already exists in the given list.
        //removes and adds a total to the list after checking the hardcap for it.
        public static List<StatusEffectsSO> HandleStatusEffectListUpdate(List<StatusEffectsSO> affectedList, StatusEffectsSO newSE)
        {
            List<StatusEffectsSO> updatedList = new List<StatusEffectsSO>();
            List<StatusEffectsSO> removedList = new List<StatusEffectsSO>();

            float totalValue = 0;

            //check for the total value of the stat in the current list.
            //and remove them
            for (int i = 0; i < affectedList.Count; i++)
            {
                if (affectedList[i].statusEffectType == newSE.statusEffectType &&
                    affectedList[i].affectedStat == newSE.affectedStat)
                {
                    totalValue += affectedList[i].value;
                    affectedList.Remove(affectedList[i]);
                }
            }

            //add the new status effect value 
            totalValue += newSE.value;

            //hard cap it to 50
            if (totalValue > 50)
            {
                totalValue = 50;
            }

            //set the new value to the new Status effect
            newSE.value = totalValue;

            //add the status effect with the new value to the list
            affectedList.Add(newSE);

            //sort the list
            updatedList = SortStatusEffectList(affectedList);

            return updatedList;
            
        }

        //sort the list on buffs, sort order is "Offensive","Defensive","Enfeeblement"
        public static List<StatusEffectsSO> SortStatusEffectList(List<StatusEffectsSO> oldList)
        {
            List<StatusEffectsSO> sortedList = new List<StatusEffectsSO>();
            sortedList = oldList;

            sortedList = oldList.OrderBy(x => (int)(x.statusEffectType)).ThenBy(x => (int)(x.affectedStat)).ToList();
            return sortedList;
        }

        //calculate the base damage that the attacker does.
        private static float GetTotalCharacterAttack(CharacterManager attacker)
        {
            float totalAtk;

            float baseAtk = attacker.charStatManager.baseAttack;

            float buffPercentage = 1;
            float debuffPercentage = 1;

            
            if (SEAttacker.Count != 0)
            {
                //Buffs.
                //buffs are in %
                for (int i = 0; i < SEAttacker.Count; i++)
                {
                    if (SEAttacker[i].name == null)
                    {
                        break;
                    }

                    if (SEAttacker[i].statusEffectType == StatusEffectType.Offensive &&
                        SEAttacker[i].affectedStat == StatusEffectAffectedStat.Attack)
                    {
                        buffPercentage += SEAttacker[i].value;
                    }
                }

                for (int i = 0; i < SEAttacker.Count; i++)
                {
                    if (SEAttacker[i].name == null)
                    {
                        break;
                    }

                    if (SEAttacker[i].statusEffectType == StatusEffectType.Enfeeblement &&
                        SEAttacker[i].affectedStat == StatusEffectAffectedStat.Attack)
                    {
                        debuffPercentage += SEAttacker[i].value;
                    }
                }
            }

            totalAtk = baseAtk / 100 * (buffPercentage + 100) * debuffPercentage;
            //Debug.Log("totalatk: " + totalAtk);
            return totalAtk;
        }

        //calculate the total defense the target has.
        private static float GetTotalDefense(CharacterManager target)
        {
            float totalDef;
            float innateDef = target.charStatManager.baseDefense;
            float defMods;
            float armorMod;

            float buffPercentage = 1;
            float debuffPercentage = 1;

            
            if (SEAttacker.Count != 0)
            {
                //buffs
                for (int i = 0; i < SETarget.Count; i++)
                {
                    if (SEAttacker[i].name == null)
                    {
                        break;
                    }

                    if (SETarget[i].statusEffectType == StatusEffectType.Defensive &&
                        SETarget[i].affectedStat == StatusEffectAffectedStat.Defense)
                    {
                        buffPercentage += SETarget[i].value;
                    }
                }

                //debuffs
                for (int i = 0; i < SETarget.Count; i++)
                {
                    if (SEAttacker[i].name == null)
                    {
                        break;
                    }

                    if (SETarget[i].statusEffectType == StatusEffectType.Enfeeblement &&
                        SETarget[i].affectedStat == StatusEffectAffectedStat.Defense)
                    {
                        debuffPercentage += SETarget[i].value;
                    }
                }
            }

            //armor modifier
            if (target.charStatManager.hasArmor)
            {
                armorMod = innateDef * .7f;
            }
            else
            {
                armorMod = innateDef * .2f;
            }

            //defense calculations
            defMods = 1 + armorMod + buffPercentage - debuffPercentage;

            totalDef = innateDef / 100 * (defMods + 100);

            return totalDef;
        }

        //returns a random number between the given numbers.
        private static float GetRandomNumber(float min, float max)
        {
            return Random.Range(min, max);
        }

        /*a boolean function that return true or false depending on percentage chance,
            you give a min & max and the threshold, it makes a random num,
            if it is below the threshold you win, else you don't
            it is below instead of above because of smaller numbers. */
        private static bool GetPercentageChance(float min, float max, float thresholdNum)
        {
            float randomNum = Random.Range(min, max);

            if (randomNum > thresholdNum)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Clears the lists after use
        static void ClearSELists()
        {
            SEAttacker.Clear();
            SETarget.Clear();
        }

    }
}