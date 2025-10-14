using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using KS;

[System.Serializable]
public class GoalSelectorNode : CompositeNode
{

    private List<UGoalNode> goalNodes = new List<UGoalNode>();
    private Node BestGoal;
    private bool goalSelected = false;

    protected override void OnStart() 
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] is UGoalNode)
            {
                goalNodes.Add((UGoalNode)children[i]);
            }
        }

        if (context.boss.currentMode == BossMode.NormalMode)
        {
            DecideBestGoal();
        }
    }

    protected override void OnStop() 
    {
        goalNodes.Clear();
    }

    protected override State OnUpdate() 
    {
        if (goalSelected)
        {
            var child = BestGoal;
            return child.Update();
        }
        else
        {
            return State.Running;
        }
    }

    private void DecideBestGoal()
    {
        float score = 0f;
        int nextBestGoalIndex = 0;

        for (int i = 0; i < goalNodes.Count; i++)
        {
            if (!goalNodes[i].Available)
                continue;

            //Debug.Log("Goals: " + goalNodes[i].name + ", goal score: " + context.utility.ScoreGoals(goalNodes[i]));
            if (context.utility.ScoreGoals(goalNodes[i]) > score)
            {
                nextBestGoalIndex = i;
                score = goalNodes[i].score;
            }
        }
        
        BestGoal = goalNodes[nextBestGoalIndex];
        goalSelected = true;
        goalNodes[nextBestGoalIndex].SelectedGoal();

        for (int i = 0; i < goalNodes.Count; i++)
        {
            goalNodes[i].UpdateGoal();
        }

        Debug.Log("Selected Goal: " + BestGoal.name + ", score: " + score);
        Debug.Log("+++++++++++++++");
        
    }

}
