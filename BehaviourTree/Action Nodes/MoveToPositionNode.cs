using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MoveToPositionNode : ActionNode
{
    public float speed = 5;
    public Vector3 nextLocation;

    protected override void OnStart() {
        nextLocation = blackboard.moveToPosition;
    }

    protected override void OnStop() {
    }
        
    protected override State OnUpdate() {

        context.gameObject.transform.position = Vector3.MoveTowards(context.gameObject.transform.position, nextLocation, Time.deltaTime * speed);

        if (context.gameObject.transform.position == nextLocation)
        {
            return State.Success;
        }

        return State.Running;
    }
}