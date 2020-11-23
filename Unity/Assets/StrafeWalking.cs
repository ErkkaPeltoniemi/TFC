using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrafeWalking : StateMachineBehaviour
{
    private int stateEnter = 0;
    private Rigidbody rb;
    Quaternion origRotation;
    Quaternion targetRotation;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stateEnter++;
        rb = animator.gameObject.GetComponent<Rigidbody>();
        origRotation = rb.rotation;

        var rotationParam = animator.GetFloat("FeetAngle");
        rotationParam = 90f * Mathf.Round(rotationParam / 90f);
        targetRotation = Quaternion.AngleAxis(rotationParam, rb.transform.up) * rb.rotation;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var curQuat = Quaternion.Lerp(origRotation, targetRotation, stateInfo.normalizedTime);
        rb.MoveRotation(curQuat);
        animator.SetFloat("FeetAngle", 0f);
        Debug.Log($"Turning [{stateEnter}]: " + Time.realtimeSinceStartup);
        base.OnStateMove(animator, stateInfo, layerIndex);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
