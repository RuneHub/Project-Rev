using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AIModeNode : CompositeNode
{

    public KS.BossMode modeLeft;
    public KS.BossMode modeMiddle;
    public KS.BossMode modeRight;

    protected override void OnStart() 
    {

    }

    protected override void OnStop() 
    {
    }

    protected override State OnUpdate()
    {
        if (context.boss.currentMode == modeLeft)
        {
            //update left so 1nd child
            return children[0].Update();
        }
        else if (context.boss.currentMode == modeMiddle)
        {
            //update middle so 2nd child
            return children[1].Update();
        }
        else if (context.boss.currentMode == modeRight)
        {
            //update right so 3nd child
            return children[2].Update();
        }

        return State.Failure;
    }
}
