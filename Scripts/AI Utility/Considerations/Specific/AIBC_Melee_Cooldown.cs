using KS;
using KS.AI;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Cooldown Consideration", menuName = "AI Behaviour/Boss Considerations/Melee Cooldown Consideration")]
public class AIBC_Melee_Cooldown : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float maxCooldownTime = 60;

    public override float ScoreConsideration(AIBossManager a, Node _node)
    {
        float cd = 0;
        node = _node;

        if (node is UMeleeAttackNode)
        {
            UMeleeAttackNode uan = (UMeleeAttackNode)node;
            cd = uan.MeleeAttack.cooldown;
        }

        return score = responseCurve.Evaluate(cd / maxCooldownTime);

    }
}
