using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon Damage Consideration", menuName = "AI Behaviour/Boss Considerations/Summon Damage Consideration")]
public class AIBC_Summon_Damage : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float maxDamagePercentage = 1000;

    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        float percentage = 0;
        node = _node;

        if (node is UMagicSummonNode)
        {
            UMagicSummonNode uan = (UMagicSummonNode)node;
            percentage = uan.summonAttack.baseDamage;
        }

        return score = responseCurve.Evaluate(percentage / maxDamagePercentage);

    }
}
