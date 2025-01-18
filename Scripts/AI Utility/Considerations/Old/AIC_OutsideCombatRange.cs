using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.AI
{
    [CreateAssetMenu(fileName = "OutsideCombatRange", menuName = "AI Behaviour/Test Considerations/Outside Range Consideration")]
    public class AIC_OutsideCombatRange : Consideration
    {
        [SerializeField] private AnimationCurve responseCurve;
        public override float ScoreConsideration(AIBossManager ai, Node _node)
        {
            float range = ai.CombatRange;
            node = _node;

            Vector3 rangePos = (ai.GetTarget().transform.position - ai.transform.position) / 2.85f; //2.85 is about 35 which is the base combat range

            float newDis = Vector3.Distance(rangePos, ai.GetTarget().transform.position);

            //Debug.Log("Attack Range: " + range + " , newDis: " + newDis + " ,score: " + responseCurve.Evaluate(Mathf.Clamp01(newDis / range)));

            return score = responseCurve.Evaluate(Mathf.Clamp01(newDis / range));
        }
    }
}