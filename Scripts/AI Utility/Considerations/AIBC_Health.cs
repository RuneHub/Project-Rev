using KS;
using KS.AI;
using UnityEngine;

[CreateAssetMenu(fileName = "Health Consideration", menuName = "AI Behaviour/Boss Considerations/Health Consideration")]
public class AIBC_Health : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        return score = responseCurve.Evaluate(Mathf.Clamp01(boss.statManager.currentHealth / boss.statManager.maxHealth));
    }
}
