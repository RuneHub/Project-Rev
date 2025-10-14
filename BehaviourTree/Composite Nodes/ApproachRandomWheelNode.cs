using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[System.Serializable]
public class ApproachRandomWheelNode : CompositeNode
{
    private List<UApproachNode> approachNodes = new List<UApproachNode>();
    private List<(float, Node)> nodeScores = new List<(float, Node)>();
    private Node ChosenApproach;
    private Node LastChosenNode;
    private int chosenNodeIndex;
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

        CalculateScores();
    }

    protected override void OnStop()
    {
        approachNodes.Clear();
    }

    protected override State OnUpdate()
    {
        if (approachSelected)
        {
            var child = ChosenApproach;
            return child.Update();
        }
        else
        {
            return State.Running;
        }
    }

    private void CalculateScores()
    {
        for (int i = 0; i < approachNodes.Count; i++)
        {
            approachNodes[i].listAction();
            float actionScore = 0f;
            for (int j = 0; j < approachNodes[i].actions.Count; j++)
            {
                float curActionScore = approachNodes[i].actions[j].score = context.utility.ScoreAction(approachNodes[i].actions[j]);
                actionScore += curActionScore;
            }
            actionScore = GlobalUtils.RoundTwoDP(actionScore / approachNodes[i].actions.Count);
            Debug.Log("ActionScore: " + actionScore);

            float appoachScore = context.utility.ScoreApproach(approachNodes[i]);

            float totalScore = appoachScore + actionScore;
            Debug.Log("Full " + approachNodes[i].name + " Approach Score: " + totalScore);
            Debug.Log("--");

            if (approachNodes[i] == LastChosenNode)
            {
                totalScore = (totalScore / 3) * 2;
            }

            nodeScores.Add((totalScore, approachNodes[i]));

            approachNodes[i].ClearAction();

        }


        ChooseNode();

        approachNodes[chosenNodeIndex].SelectedApproach();
        approachSelected = true;

        for (int i = 0; i < approachNodes.Count; i++)
        {
            approachNodes[i].UpdateApproach();
        }

        context.boss.battleData.chosenBehaviourAmount++;

        Debug.Log("Selected Approach: " + ChosenApproach.name);
        Debug.Log("---------------------------------------------");
    }

    private void ChooseNode()
    {
        float totalChance = 0;

        foreach (var n in nodeScores) 
        {
            totalChance += n.Item1;
        }
        
        float rand = Random.Range(0, totalChance);
        float cummulativeChance = 0f;

        foreach (var n in nodeScores)
        {
            cummulativeChance += n.Item1;

            if (rand <= cummulativeChance)
            {
                ChosenApproach = n.Item2;
                return;
            }

        }

        for (int i = 0; i < nodeScores.Count; i++) 
        {
            cummulativeChance += nodeScores[i].Item1;

            if (rand <= cummulativeChance)
            {
                ChosenApproach = nodeScores[i].Item2;
                LastChosenNode = ChosenApproach;
                chosenNodeIndex = i;
                return;
            }
        }

    }

}
