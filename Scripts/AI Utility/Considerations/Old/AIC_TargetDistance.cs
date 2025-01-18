using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KS.AI
{
    [CreateAssetMenu(fileName = "TargetDistanceConsideration", menuName = "AI Behaviour/Test Considerations/Target Distance Consideration")]
    public class AIC_TargetDistance : Consideration
    {
        [SerializeField] private AnimationCurve responseCurve;
        public override float ScoreConsideration(AIBossManager ai, Node _node)
        {
            float range = ai.CombatRange;
            node = _node;

            if (node is UtilityActionNode)
            {
                UtilityActionNode uan = (UtilityActionNode)node;
                range = uan.range;
            }
            float distance = Vector3.Distance(ai.transform.position, ai.GetTarget().transform.position);
            
            return score = responseCurve.Evaluate(Mathf.Clamp01(distance / range));
        }
    }
}