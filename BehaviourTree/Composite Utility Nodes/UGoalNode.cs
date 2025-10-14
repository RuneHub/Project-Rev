using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UGoalNode : UtilityCompositeNode
{
    public bool Available = true;
    public bool useCooldown;
    public int CD;
    public int selectedAmount = 0;

    private int currentCD;
    
    protected int current;

    protected override void OnStart() 
    {
        current = 0;
        currentCD = CD;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        for (int i = current; i < children.Count; i++)
        {
            current = i;
            var child = children[current];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                case State.Failure:
                    continue;
            }

        }
        return State.Failure;
    }

    public void UpdateGoal()
    {
        if(useCooldown)
        {
            currentCD--;
            if (currentCD == 0)
            {
                current = CD;
                Available = true;
            }
        }
    }

    public void SelectedGoal()
    {
        selectedAmount++;
        if (useCooldown)
        {
            Available = false;
            
        }
    }

}
