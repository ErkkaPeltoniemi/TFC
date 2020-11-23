using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Walk : StateMachineBehaviour
{
    private Rigidbody rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.gameObject.GetComponent<Rigidbody>();     
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    { 
        rb.MovePosition(animator.gameObject.transform.position + animator.deltaPosition);
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var leftFootIK = animator.GetFloat("LeftFootIK");
        var rightFootIK = animator.GetFloat("RightFootIK");
        //animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1);
        animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootIK);
        animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootIK);
        //var rightFootIKPos = animator.GetIKPositionWeight(AvatarIKGoal.RightFoot);
        //Debug.Log($"l: {leftFootIKPos} r: {rightFootIKPos}");
    }  
}
