using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomPositionAroundTargetNode : ActionNode
{

    public float range = 15f;

    protected override void OnStart()
    {
        blackboard.moveToPosition = Vector3.zero;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {

        blackboard.moveToPosition.x = Random.insideUnitCircle.x * range + context.target.transform.position.x;
        blackboard.moveToPosition.z = Random.insideUnitCircle.y * range + context.target.transform.position.z;
        blackboard.moveToPosition.y = context.target.transform.position.y;

        return State.Success;
    }
}