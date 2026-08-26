using KS;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomIdle : StateMachineBehaviour
{
    [SerializeField] AnimatorOverrideController animatorOV;
    [SerializeField] AnimationClip idle;
    [SerializeField] AnimationClip randomIdle;
    [SerializeField] float idleTime;
    [SerializeField] float RandomIdleTime;
    [SerializeField] bool RandomIdleEnabled;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle(animator);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!RandomIdleEnabled && 
            ( animator.GetFloat("Horizontal") < .3 
            && animator.GetFloat("Vertical") < .3))
        {
            idleTime += Time.deltaTime;

            if (idleTime > RandomIdleTime && stateInfo.normalizedTime % 1 < 0.02f)
            {
                RandomIdleEnabled = true;
                animatorOV["Anim_Combat_Idle"] = randomIdle;
                animator.runtimeAnimatorController = animatorOV;
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98f)
        {
            ResetIdle(animator);
        }

    }

    private void ResetIdle(Animator animator)
    {
        RandomIdleEnabled = false;
        idleTime = 0;
        animatorOV["Anim_Combat_Idle"] = idle;
        animator.runtimeAnimatorController = animatorOV;
    }

}
