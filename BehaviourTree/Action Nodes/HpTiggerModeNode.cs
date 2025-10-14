using KS;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

[System.Serializable]
public class HpTiggerModeNode : ActionNode
{
    public BossMovementSO so;

    protected override void OnStart()
    {
        Debug.Log("HpTrigger");


        context.boss.hpTriggerManager.StartPhaseTransition();
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (context.boss.HpTriggerFlag)
        {
            context.boss.HpTriggerFlag = false;
            context.boss.bossLocomotion.HandleTeleportBack(so);
            context.boss.animationEvents.ActivateStormAura();
            context.boss.animationEvents.FanVisible();
        }

        if (context.boss.TeleportingCompleted)
        {
            context.boss.HpTriggerFlag = false;
            context.boss.animator.SetBool("TeleportingCompleted", false);
            context.boss.SwapMode(BossMode.NormalMode);
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }

}