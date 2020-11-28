using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Walk : StateMachineBehaviour
{
    private Rigidbody rb;
    private BoxCollider bc;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.gameObject.GetComponent<Rigidbody>();
        bc = animator.gameObject.GetComponent<BoxCollider>();
        base.OnStateEnter(animator, stateInfo, layerIndex);
      
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        Debug.Log($"rm: {animator.hasRootMotion}, rr: {animator.deltaRotation}");
        //bc.transform.rotation = animator.gameObject.transform.rotation * animator.deltaRotation;
        base.OnStateMove(animator, stateInfo, layerIndex);
        //animator.gameObject.transform.position += animator.deltaPosition;
        //animator.gameObject.transform.rotation *= animator.deltaRotation;       
    }
}

