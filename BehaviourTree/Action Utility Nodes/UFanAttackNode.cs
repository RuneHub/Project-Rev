using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UFanAttackNode : UtilityActionNode
{
    public BossFanSO FanAttack;

    protected override void OnStart() 
    {
        if (useCooldown)
        {
            cooldown = FanAttack.cooldown;
            blackboard.cooldown = FanAttack.cooldown;
        }

        Vector3 lookTarget = new Vector3(context.target.transform.position.x,
                                           context.boss.transform.position.y,
                                           context.target.transform.position.z);
        context.boss.transform.LookAt(lookTarget);

        context.boss.animator.SetBool("PerformingAttackAction", true);
        context.boss.combatManager.HandleFanAttack(FanAttack);

    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (context.boss.FanCatchFlag)
        {
            context.boss.FanCatchFlag = false;
            context.boss.combatManager.ResetCombatAnimations();
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}