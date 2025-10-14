using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class WaitNode : ActionNode
{
    public float duration;
    float startTime;
    public bool useForCooldown;

    protected override void OnStart()
    {
        startTime = Time.time;
        if (useForCooldown)
        {
            duration = blackboard.cooldown;
        }
        else
        {
            if (duration == 0)
            {
                duration = 1;
            }
        }
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (Time.time - startTime > duration)
        {
            return State.Success;
        }

        return State.Running;
    }

}
