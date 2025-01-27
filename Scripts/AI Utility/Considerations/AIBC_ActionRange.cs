using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Action Range Consideration", menuName = "AI Behaviour/Boss Considerations/Action Range Consideration")]

public class AIBC_ActionRange : Consideration
{

    [SerializeField] private AnimationCurve responseCurve;

    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        float range = 0;
        node = _node;

        if (node is UtilityActionNode)
        {
            UtilityActionNode uan = (UtilityActionNode)node;
            range = uan.range;
        }

        float distance = Vector3.Distance(boss.transform.position, boss.GetTarget().transform.position);

        return score = responseCurve.Evaluate(Mathf.Clamp01(distance / range));

    }
}
