using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomPositionNode : ActionNode
{
    public bool relative = false;
    public Vector3 min = Vector3.one * -10;
    public Vector3 max = Vector3.one * 10;
    

    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {

        Vector3 pos = new Vector3();
        if (relative)
        {
            pos.x = Random.Range(min.x, max.x);
            pos.y = Random.Range(min.y, max.y);
            pos.z = Random.Range(min.z, max.z);

            pos.x += context.gameObject.transform.position.x;
            pos.y += context.gameObject.transform.position.y;
            pos.z += context.gameObject.transform.position.z;
        }
        else
        {
            pos.x = Random.Range(min.x, max.x);
            pos.y = Random.Range(min.y, max.y);
            pos.z = Random.Range(min.z, max.z);
        }
        blackboard.moveToPosition = pos;

        return State.Success;
    }
}