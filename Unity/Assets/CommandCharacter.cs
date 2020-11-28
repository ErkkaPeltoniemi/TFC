using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCharacter : MonoBehaviour
{
    Vector3 point;
    Plane plane;
    //CharacterController charController;
    Animator anim;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        point = Vector3.zero;
        plane = new Plane(Vector3.up, 0);
        //charController = GetComponent<CharacterController>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

    }




    private Vector3 screenPointToWorldPoint(Vector3 screenPoint)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPoint);
      
        Vector3 result = Vector3.zero;
        
        float dist;
        if (plane.Raycast(ray, out dist))
        {
            result = ray.GetPoint(dist);
        }
        return result;
    }

    //private void OnAnimatorMove()
    //{
    //    if (anim != null)
    //    {
    //        transform.SetPositionAndRotation(anim.bodyPosition, anim.bodyRotation);
    //    }

    //}

    void LateUpdate()
    {
        var camera = Camera.main;
        var mv = GetMovementInfo();
        anim.SetFloat("FeetAngle", mv.aimingInfo.angleRelativeToBody);

        anim.SetFloat("SpineAngle", mv.aimingInfo.angleRelativeToBody);
        //Debug.Log(mv.aimingInfo.angleRelativeToBody);
        //Debug.Log(rb.velocity);
        Debug.Log($"mv: {mv.moveCommandDirection} f: {mv.forwardMoveRelativeToBody}, r: {mv.rightMoveRelativeToBody}, a: {mv.targetRelativeToMovementDir}");
        //Debug.Log(rb.transform.forward);
        if (mv.moveCommandDirection == Vector3.zero)
        {
            
            anim.SetFloat("Forward", 0f, 0.1f, Time.deltaTime);
            anim.SetFloat("Right", 0f, 0.1f, Time.deltaTime);
        }
        else
        {
            anim.SetFloat("Forward", mv.forwardMoveRelativeToBody, 0.1f, Time.deltaTime);
            anim.SetFloat("Right", mv.rightMoveRelativeToBody, 0.1f, Time.deltaTime);
        }
        anim.SetFloat("TargetRelativeToMvm", mv.targetRelativeToMovementDir);
        anim.SetFloat("TotalMovement", Mathf.Abs(mv.forwardMoveRelativeToBody) + Math.Abs(mv.rightMoveRelativeToBody));
        //Debug.Log("TotalMovement: "+ mv.forwardMoveRelativeToBody + mv.rightMoveRelativeToBody);
    }

    private Vector3 GetCurrentMoveCommand()
    {
        var camera = Camera.main;
        var moveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            moveDir += camera.transform.up;
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveDir += -camera.transform.up;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDir += camera.transform.right;         
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDir += -camera.transform.right;
        }
        moveDir.y = 0f;
        return moveDir.normalized;
       
    }

    private MovementInfo GetMovementInfo()
    {
        var moveDir = GetCurrentMoveCommand();


        var mv = new MovementInfo
        {
            moveCommandDirection = moveDir,
        };

        mv.aimingInfo = GetCurrentAimingInfo(mv);

        var fromChartoTarget =  mv.aimingInfo.targetPositionInWorldCoordinates- rb.position;
        fromChartoTarget.Normalize();
        mv.forwardMoveRelativeToBody = Vector3.Dot(fromChartoTarget, rb.transform.forward);
        mv.rightMoveRelativeToBody = Vector3.Dot(fromChartoTarget, rb.transform.right);
        mv.targetRelativeToMovementDir = Vector3.Dot(fromChartoTarget, moveDir);


        return mv;
    }
    
    private AimingInfo GetCurrentAimingInfo(MovementInfo mv)
    {
        var targetPoint = screenPointToWorldPoint(Input.mousePosition);
        targetPoint.y = rb.transform.position.y;

        Vector3 deltaVec = targetPoint - rb.transform.position;
        Quaternion rotation = Quaternion.LookRotation(deltaVec);

        float angleToTarg = Vector3.SignedAngle(rb.transform.forward, targetPoint - rb.transform.position, rb.transform.up);
        float angleToTargWorld = Vector3.SignedAngle(rb.transform.position, targetPoint - rb.transform.position, Vector3.up);
        float targetRelativeToVelocity = Vector3.SignedAngle(mv.moveCommandDirection, targetPoint-rb.transform.position, Vector3.up);
        return new AimingInfo
        {
            angleRelativeToBody = angleToTarg,
            targetPositionInWorldCoordinates = targetPoint,
            angleToTarget = angleToTargWorld,
            angleRelativeToVelocity = targetRelativeToVelocity
        };
    }
    class MovementInfo
    {
        public Vector3 moveCommandDirection;
        public float forwardMoveRelativeToBody;
        public float rightMoveRelativeToBody;
        public float targetRelativeToMovementDir;
        public AimingInfo aimingInfo;
    }
    class AimingInfo
    {
        public float angleRelativeToBody;
        public Vector3 targetPositionInWorldCoordinates;
        public float angleToTarget;
        public float angleRelativeToVelocity;
    }



    public void StartRecordingPlan(GameObject gameObjectToRecord)
    {
        this.enabled = gameObject == gameObjectToRecord;
    }
    public void ToFreeLookMode()
    {
        this.enabled = false;
    }
}
