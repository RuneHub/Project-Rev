using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UApproachNode : UtilityCompositeNode
{
    public List<UtilityActionNode> actions = new List<UtilityActionNode>();
    int current;

    public bool Available = true;
    public bool useCooldown;
    public int CD;
    public int selectedAmount = 0;

    private int currentCD;


    protected override void OnStart() 
    {
        current = 0;
        currentCD = CD;
    }

    protected override void OnStop() 
    {
        actions.Clear();
    }

    protected override State OnUpdate()
    {
        var child = children[current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                return State.Failure;
            case State.Success:
                current++;
                break;
        }

        return current == children.Count ? State.Success : State.Running;
    }

    public void listAction()
    {
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] is UtilityActionNode)
            {
                actions.Add((UtilityActionNode)children[i]);
            }
        }
        //Debug.Log("Actions num: " + actions.Count);
    }

    public void ClearAction()
    {
        actions.Clear();
    }

    public void UpdateApproach()
    {
        if (useCooldown)
        {
            currentCD--;
            if (currentCD == 0)
            {
                current = CD;
                Available = true;
            }
        }
    }

    public void SelectedApproach()
    {
        selectedAmount++;
        if (useCooldown)
        {
            Available = false;

        }
    }

}
