using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ULongCastNode : UtilityActionNode
{

    public BossMagicCastsMechanicSO MagicCastSO;

    float duration;
    float startTime;

    protected override void OnStart() 
    {
        startTime = Time.time;

        duration = MagicCastSO.CastTime;

        context.boss.animator.SetBool("PerformingAttackAction", true);
        context.boss.combatManager.HandleMagicCast(MagicCastSO);
        context.boss.bossAnimations.PlayTargetAnimation("LongCastStart", true, true, CrossFadeSpeed: 0, layerNum: 2, normalizedTime: 0);

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
        else
        {
            return State.Running;
        }
    }
}