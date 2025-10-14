using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeLimitNode : DecoratorNode
{
    /*
     * Time Limit Node
     *      it will give it children a set amount of time to finish tasks before stopping it
     *      the timer reset everytime on entry.
     *
     */

    public float duration = 10;
    float startTime;

    protected override void OnStart() 
    {
        startTime = Time.time;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        if (Time.time - startTime > duration)
        {
            return State.Failure;
        }
        else
        {
            if (child.state == State.Running)
            {
                return child.Update();
            }
            else if (child.state == State.Success)
            {
                return State.Success;
            }
        }
        return State.Running;
    }
}
