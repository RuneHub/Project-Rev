using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UMagicSummonNode : UtilityActionNode
{
    public BossSummonSO summonAttack;

    protected override void OnStart() 
    {
        if (useCooldown)
        {
            cooldown = summonAttack.cooldown;
            blackboard.cooldown = summonAttack.cooldown;
        }

        Vector3 lookTarget = new Vector3(context.target.transform.position.x,
                                           context.boss.transform.position.y,
                                           context.target.transform.position.z);
        context.boss.transform.LookAt(lookTarget);

        context.boss.animator.SetBool("PerformingAttackAction", true);
        context.boss.combatManager.HandleMagicSummon(summonAttack);

    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (context.boss.MagicSummonFlag)
        {
            context.boss.MagicSummonFlag = false;
            context.boss.combatManager.ResetCombatAnimations();
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}