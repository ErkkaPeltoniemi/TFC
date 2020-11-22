using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnInPlace : StateMachineBehaviour
{
    private Rigidbody rb;
    Quaternion origRotation;
    Quaternion targetRotation;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.gameObject.GetComponent<Rigidbody>();
        origRotation = rb.rotation;

        var rotationParam = animator.GetFloat("FeetAngle");
        targetRotation = Quaternion.AngleAxis(rotationParam, rb.transform.up) * rb.rotation;    
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var curQuat = Quaternion.Lerp(origRotation, targetRotation, stateInfo.normalizedTime); 
        rb.MoveRotation(curQuat);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
