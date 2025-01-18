using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBoolOnExit : StateMachineBehaviour
{
    public string targetBool;
    public bool targetBoolStatus;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(targetBool, targetBoolStatus);
    }

}
