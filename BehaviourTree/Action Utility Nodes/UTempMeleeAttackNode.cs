using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UTempMeleeAttackNode : UtilityActionNode
{
    private float timer;

    protected override void OnStart()
    {
        timer = cooldown;
        //context.owner.combatManager.ChargeHitBox();
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                return State.Success;
            }

            return State.Running;
        }
        else
        {
            return State.Running;
        }
    }
}