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
        //stateEnter++;
        rb = animator.gameObject.GetComponent<Rigidbody>();
        //origRotation = rb.rotation;
        
        //var rotationParam = animator.GetFloat("FeetAngle");
        //rotationParam = 90f * Mathf.Round(rotationParam / 90f);
        //targetRotation = Quaternion.AngleAxis(rotationParam, rb.transform.up) * rb.rotation;
        ////origRotation = animator.bodyRotation;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        //Debug.Log(animator.bodyRotation);
    }
    

    Quaternion prevBodyRotation;


    public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //var curQuat = Quaternion.Lerp(origRotation, targetRotation, stateInfo.normalizedTime);
        //rb.MoveRotation(curQuat);
        //animator.SetFloat("FeetAngle", 0f);
        //Debug.Log($"Turning [{stateEnter++}]: " + animator.deltaRotation);
        //Debug.Log(animator.deltaRotation);
        animator.ApplyBuiltinRootMotion();
        if (animator.deltaRotation.ToString() != new Quaternion(0, 0, 0, 1).ToString())
        {
            //Debug.Log("Diff");
        }
        //base.OnStateMove(animator, stateInfo, layerIndex);
        
        //rb.rotation *= animator.deltaRotation;
        //rb.position += animator.deltaPosition;
    
    }
    public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //targetRotation = animator.bodyRotation;
        //var rot = animator.bodyRotation * Quaternion.Inverse(origRotation);
        //Debug.Log("br: "+animator.bodyRotation+ " og: "+ origRotation +" go: " +rb.transform.rotation + " dr: " +animator.deltaRotation);
        //if (rb.rotation != animator.bodyRotation)
        //{
        //    rb.rotation = animator.bodyRotation;
        //}



        //animator.gameObject.transform.position += animator.deltaPosition;
        //rb.MoveRotation(newRotation);




        //rb.MoveRotation(rb.rotation * animator.deltaRotation);
        //base.OnStateIK(animator, stateInfo, layerIndex);
    }
    void LateUpdate()
    {
        rb.MoveRotation(targetRotation);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
