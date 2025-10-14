using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class URandomSelectorNode : UtilityCompositeNode
{
    /*
    * Selector Random node.
    *  Selects a random node from its children to execute.
    *
    */

    protected int current;

    protected override void OnStart()
    {
        current = Random.Range(0, children.Count);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        var child = children[current];
        return child.Update();
    }
}
