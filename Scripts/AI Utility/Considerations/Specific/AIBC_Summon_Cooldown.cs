using KS;
using KS.AI;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon Cooldown Consideration", menuName = "AI Behaviour/Boss Considerations/Summon Cooldown Consideration")]
public class AIBC_Summon_Cooldown : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float maxCooldownTime = 60;

    public override float ScoreConsideration(AIBossManager a, Node _node)
    {
        float cd = 0;
        node = _node;

        if (node is UMagicSummonNode)
        {
            UMagicSummonNode uan = (UMagicSummonNode)node;
            cd = uan.summonAttack.cooldown;
        }

        return score = responseCurve.Evaluate(cd / maxCooldownTime);

    }
}
