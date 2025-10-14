using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SelectorNode : CompositeNode
{
    /*
     * Selector node.
     *  it stops executing when a child succeeds,
     *  the outcome of the slector node is the same as their child.
     *  so whent he child fails so does the selector and that goes for succeeding too.
     *
     */

    protected int current;
    protected override void OnStart()
    {
        current = 0;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        for (int i = current; i < children.Count; i++)
        {
            current = i;
            var child = children[current];

            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Success:
                    return State.Success;
                case State.Failure:
                    continue;
            }

        }
        return State.Failure;
    }
}

