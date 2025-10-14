using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TestActionNode : ActionNode
{
    protected override void OnStart()
    {
        context.boss.animator.SetBool("PerformingAttackAction", true);
        //context.boss.combatManager.HandleMeleeAttack();
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (!context.boss.performingAttackAction && !context.boss.isInteracting)
        {
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
    
}