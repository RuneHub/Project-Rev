using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

[System.Serializable]
public class RandomInFieldLocationNode : ActionNode
{
    private bool locationSelected;
    private Vector3 newPos = Vector3.zero;
    protected override void OnStart()
    {
        Bounds b = context.boss.field.bounds;
        newPos = new Vector3(
            Random.Range(b.min.x, b.max.x),
            0.1f,
            Random.Range(b.min.z, b.max.z)
        );

        blackboard.moveToPosition = newPos;
        locationSelected = true;
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        if (locationSelected)
        {
            locationSelected = false;
            return State.Success;
        }
        else
        {
            return State.Running;
        }
    }
}