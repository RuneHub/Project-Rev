using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.AI
{
    [CreateAssetMenu(fileName = "CooldownConsideration", menuName = "AI Behaviour/Test Considerations/Cooldown Consideration")]
    public class AIC_Cooldown : Consideration
    {
        [SerializeField] private AnimationCurve responseCurve;
        public override float ScoreConsideration(AIBossManager ai, Node _node)
        {
            float CD = 10f;// ai.combatCooldown;
            node = _node;

            if (node is UtilityActionNode)
            {
                UtilityActionNode uan = (UtilityActionNode)node;
                CD = uan.cooldown;
            }

            return score = responseCurve.Evaluate(Mathf.Clamp01(CD / 120));
        }
    }
}