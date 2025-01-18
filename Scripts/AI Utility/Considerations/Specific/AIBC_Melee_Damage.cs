using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Damage Consideration", menuName = "AI Behaviour/Boss Considerations/Melee Damage Consideration")]
public class AIBC_Melee_Damage : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float maxDamagePercentage = 1000;

    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        float percentage = 0;
        node = _node;

        if (node is UMeleeAttackNode)
        {
            UMeleeAttackNode uan = (UMeleeAttackNode)node;
            percentage = uan.MeleeAttack.baseDamage;
        }

        return score = responseCurve.Evaluate(percentage / maxDamagePercentage);

    }
}
