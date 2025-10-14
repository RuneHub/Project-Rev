using KS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ApproachSelectorNode : CompositeNode
{
    private List<UApproachNode> approachNodes = new List<UApproachNode>();
    private Node LastChosenNode;
    private Node BestApproach;
    private bool approachSelected = false;

    protected override void OnStart() 
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] is UApproachNode)
            {
                approachNodes.Add((UApproachNode)children[i]);
            }
        }

        DecideBestApproach();
    }

    protected override void OnStop() 
    {
        approachNodes.Clear();
    }

    protected override State OnUpdate() 
    {
        if (approachSelected)
        {
            var child = BestApproach;
            return child.Update();
        }
        else
        {
            return State.Running;
        }
    }

    private void DecideBestApproach()
    {
        float score = 0f;
        int nextBestApproachIndex  = 0;

        for (int i = 0; i < approachNodes.Count; i++)
        {
            if (approachNodes[i] == LastChosenNode)
                continue;

            approachNodes[i].listAction();
            float actionScore = 0f;
            for (int j = 0; j < approachNodes[i].actions.Count; j++)
            {
                float curActionScore = approachNodes[i].actions[j].score = context.utility.ScoreAction(approachNodes[i].actions[j]);
                actionScore += curActionScore;
            }
            actionScore = GlobalUtils.RoundTwoDP(actionScore / approachNodes[i].actions.Count);
           // Debug.Log("ActionScore: " + actionScore);

            float appoachScore = context.utility.ScoreApproach(approachNodes[i]);

            float totalScore = appoachScore + actionScore;
            Debug.Log("Full " + approachNodes[i].name + " Approach Score: " + totalScore);
            //Debug.Log("--");

            if (totalScore > score)
            {
                nextBestApproachIndex = i;
                score = totalScore;
            }
            approachNodes[i].ClearAction();
        }

        BestApproach = approachNodes[nextBestApproachIndex];
        LastChosenNode = approachNodes[nextBestApproachIndex];
        approachNodes[nextBestApproachIndex].SelectedApproach();
        approachSelected = true;
        
        for (int i = 0; i < approachNodes.Count; i++)
        {
            approachNodes[i].UpdateApproach();
        }
        context.boss.battleData.chosenBehaviourAmount++;
        Debug.Log("Selected Approach: " + BestApproach.name + ", score: " + score);
        Debug.Log("---------------------------------------------");

    }

}
