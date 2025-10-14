using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ULongCastFinishNode : UtilityActionNode
{
    public BossMagicCastsMechanicSO MagicCastSO;

    protected override void OnStart() 
    {
        if (useCooldown)
        {
            cooldown = MagicCastSO.cooldown;
            blackboard.cooldown = MagicCastSO.cooldown;
        }

        context.boss.animator.SetBool("LongCastRelease", true);

    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (!context.boss.ActiveMechanic && context.boss.LongCastFinish)
        {
            context.boss.LongCastFinish = false;
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}