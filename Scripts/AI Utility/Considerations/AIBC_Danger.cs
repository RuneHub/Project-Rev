using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Danger Consideration", menuName = "AI Behaviour/Boss Considerations/Danger Consideration")]
public class AIBC_Danger : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;
    [SerializeField] private float HighHealthLossInPercentages = 10;

    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        return score = responseCurve.Evaluate(Mathf.Clamp01(boss.battleData.accumulatedDamage/HighHealthLossInPercentages));
    }
}
