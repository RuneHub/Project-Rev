using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor Consideration", menuName = "AI Behaviour/Boss Considerations/Armor Consideration")]
public class AIBC_Armor : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;

    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        return score = responseCurve.Evaluate(Mathf.Clamp01(boss.statManager.currentArmor / boss.statManager.maxArmor));
    }
}
