using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UFastCastNode : UtilityActionNode
{
    public BossMagicCastsMechanicSO MagicCastSO;

    protected override void OnStart() 
    {
        context.boss.animator.SetBool("PerformingAttackAction", true);
        context.boss.combatManager.HandleMagicCast(MagicCastSO); 
        context.boss.bossAnimations.PlayTargetAnimation("MagicCast", true, true, CrossFadeSpeed: 0, layerNum: 2, normalizedTime: 0);

    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (context.boss.LongCastFinish)
        {
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}