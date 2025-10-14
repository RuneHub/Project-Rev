using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LoopNode : DecoratorNode
{
    /*
    *  Loop node.
    *  this node loops a inputted amount of time.
    *
    */

    public int loopAmount;

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        for (int i = 0; i < loopAmount; i++)
        {
            child.Update();    
        }

        return State.Success;

    }
}
