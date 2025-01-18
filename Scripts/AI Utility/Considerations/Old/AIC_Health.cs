using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.AI
{
    [CreateAssetMenu(fileName = "HealthConsideration", menuName = "AI Behaviour/Test Considerations/Health Consideration")]
    public class AIC_Health : Consideration
    {
        [SerializeField] private AnimationCurve responseCurve;
        public override float ScoreConsideration(AIBossManager ai, Node node)
        {
            return score = responseCurve.Evaluate(Mathf.Clamp01(ai.charStatManager.currentHealth / ai.charStatManager.maxHealth));
        }
    }
}