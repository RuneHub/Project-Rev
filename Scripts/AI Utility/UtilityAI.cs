using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KS.AI
{
    public class UtilityAI : MonoBehaviour
    {
        private AIBossManager ai;

        private void Awake()
        {
            ai = GetComponent<AIBossManager>();
        }

        public float ScoreGoals(UGoalNode goals)
        {
            if (goals.considerations.Length == 0)
                return 0;

            Debug.Log("Goal: " + goals.name);
            return goals.score = CalculateTotalScore(goals.considerations, goals);
        }

        public float ScoreApproach(UApproachNode approach)
        {
            if (approach.considerations.Length == 0)
                return 0;

            Debug.Log(" Approach: " + approach.name);
            return approach.score = CalculateTotalScore(approach.considerations, approach);
        }

        public float ScoreAction(UtilityActionNode action)
        {
            if (action.considerations.Length == 0)
                return 0;

            Debug.Log("  Action: " + action.name);
            return action.score = CalculateTotalScore(action.considerations, action);
        }

        private float CalculateTotalScore(Consideration[] list, Node node)
        {
            float score = 1f;
            for (int i = 0; i < list.Length; i++)
            {
                float considerationScore = list[i].ScoreConsideration(ai, node);

                if (considerationScore == 0)
                {
                    considerationScore = 0.01f;
                }

                Debug.Log("   Consideration: " + list[i].name + ", score: " + considerationScore);
                score *= considerationScore;

            }

            float originalScore = score;
            float modFactor = 1 - (1 / list.Length);
            float makeupValue = (1 - originalScore) * modFactor;
            float totalScore = originalScore + (makeupValue * originalScore);
            totalScore = GlobalUtils.RoundTwoDP(totalScore);
            Debug.Log("Node: " + node.name + " - totalScore: " + totalScore);
            
            return totalScore;
        }

    }
}