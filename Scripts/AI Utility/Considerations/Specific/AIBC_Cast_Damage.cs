using KS;
using KS.AI;
using UnityEngine;

[CreateAssetMenu(fileName = "Cast Damage Consideration", menuName = "AI Behaviour/Boss Considerations/Cast Damage Consideration")]
public class AIBC_Cast_Damage : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float maxDamagePercentage = 1000;

    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        float percentage = 0;
        node = _node;

        if (node is ULongCastNode)
        {
            ULongCastNode uan = (ULongCastNode)node;
            percentage = uan.MagicCastSO.baseDamage;
        }

        return score = responseCurve.Evaluate(percentage / maxDamagePercentage);

    }
}
