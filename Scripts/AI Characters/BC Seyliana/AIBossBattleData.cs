using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS
{
    public class AIBossBattleData : MonoBehaviour
    {
        AIBossManager manager;

        [Header("General")]
        public int chosenBehaviourAmount = 0;
        public int HighDecisionMakingCount = 10;

        [Header("Damage Accumulation in Percentages")]
        public bool damageTrackerRunning = false;
        public float damageTime = 1.5f;
        public float accumulatedDamage;

        private void Awake()
        {
            manager = GetComponent<AIBossManager>();
        }

        #region General

        public int GetChosenBehaviourAmountInterest()
        {
            if (chosenBehaviourAmount == 0)
            {
                return 1 * HighDecisionMakingCount;
            }
            else
            {
                return chosenBehaviourAmount * HighDecisionMakingCount;
            }
        }
        #endregion

        #region Damage Accumulation
        public void StartAccumuledDamage()
        { 
            StartCoroutine(AccumulateDamage());
        }

        IEnumerator AccumulateDamage()
        {   
            damageTrackerRunning = true;
            float beforeTimeHealth = manager.statManager.CurrentHealthPercantage;

            yield return new WaitForSeconds(damageTime);

            float afterTimeHealth = manager.statManager.CurrentHealthPercantage;

            accumulatedDamage = beforeTimeHealth - afterTimeHealth;

            damageTrackerRunning = false;
        }
        #endregion



    }
}