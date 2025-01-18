using KS;
using KS.AI;
using UnityEngine;

[CreateAssetMenu(fileName = "Dash Cooldown Consideration", menuName = "AI Behaviour/Boss Considerations/Dash Cooldown Consideration")]
public class AIBC_Dash_Cooldown : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float maxCooldownTime = 60;

    public override float ScoreConsideration(AIBossManager a, Node _node)
    {
        float cd = 0;
        node = _node;

        if (node is UDashAttackNode)
        {
            UDashAttackNode uan = (UDashAttackNode)node;
            cd = uan.DashAttackSO.cooldown;
        }

        return score = responseCurve.Evaluate(cd / maxCooldownTime);

    }
}