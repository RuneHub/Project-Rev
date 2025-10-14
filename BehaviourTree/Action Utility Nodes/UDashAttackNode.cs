using KS;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class UDashAttackNode : UtilityActionNode
{
    public BossDashAttackSO DashAttackSO;

    protected override void OnStart() 
    {
        if (useCooldown)
        {
            cooldown = DashAttackSO.cooldown;
            blackboard.cooldown = DashAttackSO.cooldown;
        }

        context.boss.animator.SetBool("PerformingAttackAction", true);
        context.boss.bossAnimations.PlayTargetAnimation("DashAttack", true, true, CrossFadeSpeed: 0, layerNum: 2);

    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (!context.boss.ActiveMechanic && context.boss.DashAttackCompleted)
        {
            context.boss.DashAttackCompleted = false;
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}