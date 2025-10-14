using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UMeleeAttackNode : UtilityActionNode
{
    public BossMeleeSO MeleeAttack;
    
    protected override void OnStart() 
    {
        if (useCooldown)
        {
            cooldown = MeleeAttack.cooldown;
            blackboard.cooldown = MeleeAttack.cooldown;
        }

        Vector3 lookTarget = new Vector3(context.target.transform.position.x,
                                           context.boss.transform.position.y,
                                           context.target.transform.position.z);
        context.boss.transform.LookAt(lookTarget);

        context.boss.animator.SetBool("PerformingAttackAction", true);
        context.boss.combatManager.HandleMeleeAttack(MeleeAttack);
        
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate()
    {
        if(context.boss.comboFlag)
        {
            context.boss.comboFlag = false;
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}