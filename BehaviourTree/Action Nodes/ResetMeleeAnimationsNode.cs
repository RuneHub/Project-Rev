using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResetMeleeAnimationsNode : ActionNode
{
    protected override void OnStart()
    {
        context.boss.combatManager.ResetCombatAnimations();
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        return State.Success;
    }
}