using KS;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class UTeleportNode : UtilityActionNode
{
    public BossMovementSO so;
    public BossTeleportLocations nextLocation;
    public bool useRandomTeleportTime;
    public Vector2 teleportTime;

    protected override void OnStart() 
    {
        if (nextLocation == BossTeleportLocations.NextLocation)
        {
            context.boss.bossLocomotion.nextTeleportLocation = blackboard.moveToPosition;
        }
        context.boss.bossLocomotion.HandleTeleport(so, nextLocation);

    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (context.boss.TeleportingCompleted)
        {
            context.boss.animator.SetBool("TeleportingCompleted", false);
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}