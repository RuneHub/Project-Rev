using KS;
using KS.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Interest Consideration", menuName = "AI Behaviour/Boss Considerations/Interest Consideration")]
public class AIBC_Interest : Consideration
{
    [SerializeField] private AnimationCurve responseCurve;

    public override float ScoreConsideration(AIBossManager boss, Node _node)
    {
        node = _node;

        float selectedAmount = 0;

        if (node is UGoalNode)
        {
            UGoalNode ugn = (UGoalNode)node;
            selectedAmount = ugn.selectedAmount;
        }
        else if(node is UApproachNode)
        {
            UApproachNode uan = (UApproachNode)node;
            selectedAmount = uan.selectedAmount;
        }

        return score = responseCurve.Evaluate(Mathf.Clamp01(selectedAmount/boss.battleData.GetChosenBehaviourAmountInterest()));
    }
}
