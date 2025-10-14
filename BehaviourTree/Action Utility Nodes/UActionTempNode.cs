using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UActionTempNode : UtilityActionNode
{

    protected override void OnStart() 
    {
        Debug.Log(name);
    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate() 
    {
        return State.Success;
    }
}