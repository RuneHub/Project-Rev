using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Fan Damage Consideration", menuName = "AI Behaviour/Boss Considerations/Fan Damage Consideration")]
public class AIBC_Fan_Damage : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float maxDamagePercentage = 1000;

    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        float percentage = 0;
        node = _node;

        if (node is UFanAttackNode)
        {
            UFanAttackNode uan = (UFanAttackNode)node;
            percentage = uan.FanAttack.baseDamage;
        }

        return score = responseCurve.Evaluate(percentage / maxDamagePercentage);

    }
}
