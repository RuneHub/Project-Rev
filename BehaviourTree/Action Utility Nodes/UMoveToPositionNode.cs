using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UMoveToPositionNode : UtilityActionNode
{
    public bool useBB;
    public Vector3 nextLocation;

    protected override void OnStart() 
    {
        if (useBB)
        {
            context.boss.bossLocomotion.nextTeleportLocation = blackboard.moveToPosition;
            nextLocation = blackboard.moveToPosition;
            Debug.Log("NextLocation: " + nextLocation);
        }

        nextLocation.y = context.boss.bossLocomotion.yHeight;
        context.boss.bossLocomotion.isMoving = true;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {

        Vector3 lookTarget = new Vector3(context.target.transform.position.x,
                                           context.boss.transform.position.y,
                                           context.target.transform.position.z);
        context.boss.transform.LookAt(lookTarget);

        float dist = Vector3.Distance(context.boss.transform.position, nextLocation);
        if (dist < 1)
        {
            context.boss.bossLocomotion.isMoving = false;
            context.boss.bossLocomotion.InputMovement(Vector3.zero, 0,0);
            return State.Success;
        }
        else
        {
            Vector3 moveDir = nextLocation - context.boss.transform.position;
            context.boss.bossLocomotion.InputMovement(moveDir, -moveDir.x, -moveDir.z);
            return State.Running;
        }
        
    }
}