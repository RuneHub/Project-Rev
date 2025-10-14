using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BreakModeNode : ActionNode
{

    public float BreakTime;
    private float startTime;

    public bool BreakDone;

    protected override void OnStart()
    {
        context.boss.animator.SetBool("Staggered", true);
        context.boss.bossAnimations.PlayTargetAnimation("StaggerBreak", true, false, CrossFadeSpeed: 0, layerNum: 3, normalizedTime: 0);
        
        BreakTime = context.boss.breakModeTime;
        startTime = Time.time;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (Time.time - startTime > BreakTime) 
        {
            BreakDone = true;
        }

        if (BreakDone)
        {
            BreakDone = false;
            context.boss.RestoreBreakMode();
            return State.Success;
        }

        return State.Running;
    }
}