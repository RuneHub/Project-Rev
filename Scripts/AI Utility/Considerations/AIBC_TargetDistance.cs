using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Target Distance Consideration", menuName = "AI Behaviour/Boss Considerations/Target Distance Consideration")]
public class AIBC_TargetDistance : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;

    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        float range = boss.CombatRange;
        node = _node;

        float distance = Vector3.Distance(boss.transform.position, boss.GetTarget().transform.position);

        return score = responseCurve.Evaluate(Mathf.Clamp01(distance/range));

    }
}
