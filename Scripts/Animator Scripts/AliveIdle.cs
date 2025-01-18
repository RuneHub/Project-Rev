using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveIdle : StateMachineBehaviour
{
    
    [SerializeField] private float _timeUntilAlive;

    private bool _isAlive;
    private float _idleTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdleAnimation();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_isAlive)
        {
            _idleTime += Time.deltaTime;

            if (_idleTime > _timeUntilAlive &&
                stateInfo.normalizedTime % 1 < 0.02f)
            {
                _isAlive = true;
                animator.SetBool("AliveIdle", true); ;
            }

        }
    }

    private void ResetIdleAnimation()
    {
        _isAlive = false;
        _idleTime = 0;
    }

   
}
