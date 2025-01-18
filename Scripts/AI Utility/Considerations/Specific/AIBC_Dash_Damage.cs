using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DashMech Damage Consideration", menuName = "AI Behaviour/Boss Considerations/DashMech Damage Consideration")]
public class AIBC_Dash_Damage : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float maxDamagePercentage = 1000;

    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        float percentage = 0;
        node = _node;

        if (node is UDashAttackNode)
        {
            UDashAttackNode uan = (UDashAttackNode)node;
            percentage = uan.DashAttackSO.baseDamage;
        }

        return score = responseCurve.Evaluate(percentage / maxDamagePercentage);
    }
}
