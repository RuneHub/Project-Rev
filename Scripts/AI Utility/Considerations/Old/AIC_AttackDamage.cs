using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.AI
{
    [CreateAssetMenu(fileName = "AttackDamageConsideration", menuName = "AI Behaviour/Test Considerations/Attack Damage Consideration")]
    public class AIC_AttackDamage : Consideration
    {
        [SerializeField] private AnimationCurve responseCurve;
        public override float ScoreConsideration(AIBossManager ai, Node _node)
        {
            float AP = ai.combatManager.currentMeleeAttack.baseDamage; //AP stand for Attack Percentage
            node = _node;

            if (node is UtilityActionNode)
            {
                UtilityActionNode uan = (UtilityActionNode)node;
                 AP = uan.percentage;
            }
            
            return score = responseCurve.Evaluate(Mathf.Clamp01(AP / 500));
        }
    }
}