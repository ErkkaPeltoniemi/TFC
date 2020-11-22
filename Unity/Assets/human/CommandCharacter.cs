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


    private Vector3 acceleration;
    public float maxSpeed;
    public float mass;
    private Rigidbody rb;
    private Vector3 previousMoveCommand = Vector3.zero;

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

    void Update()
    {
        var camera = Camera.main;
        var mv = GetMovementInfo();

        anim.SetFloat("FeetAngle", mv.aimingInfo.angleRelativeToBody);
        anim.SetFloat("SpineAngle", mv.aimingInfo.angleRelativeToBody);

        if (mv.moveCommandDirection != previousMoveCommand)
        {
            Debug.Log($"mv: {mv.moveCommandDirection} f: {mv.forwardMoveRelativeToBody}, r: {mv.rightMoveRelativeToBody}");
            if (mv.moveCommandDirection == Vector3.zero)
            {
                anim.SetFloat("Forward", 0f, 0.05f, Time.deltaTime);
                anim.SetFloat("Right", 0f, 0.05f, Time.deltaTime);
            }
            else
            {
                anim.SetFloat("Forward", mv.forwardMoveRelativeToBody);
                anim.SetFloat("Right", mv.rightMoveRelativeToBody);
            }
            anim.SetFloat("TotalMovement", mv.forwardMoveRelativeToBody + mv.rightMoveRelativeToBody);
        }

        previousMoveCommand = mv.moveCommandDirection;
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
            aimingInfo = GetCurrentAimingInfo()
        };

        if (moveDir != Vector3.zero)
        {
            mv.forwardMoveRelativeToBody = Vector3.Dot(moveDir, rb.transform.forward); 
            mv.rightMoveRelativeToBody = Vector3.Dot(moveDir, rb.transform.right); 
        }else
        {
            mv.forwardMoveRelativeToBody = 0f;
            mv.rightMoveRelativeToBody = 0f;
        }

        


        return mv;
    }
    
    private AimingInfo GetCurrentAimingInfo()
    {
        var targetPoint = screenPointToWorldPoint(Input.mousePosition);
        targetPoint.y = rb.transform.position.y;

        Vector3 deltaVec = targetPoint - rb.transform.position;
        Quaternion rotation = Quaternion.LookRotation(deltaVec);

        float angleToTarg = Vector3.SignedAngle(rb.transform.forward, targetPoint - rb.transform.position, rb.transform.up);
        float angleToTargWorld = Vector3.SignedAngle(rb.transform.position, targetPoint - rb.transform.position, Vector3.up);
        return new AimingInfo
        {
            angleRelativeToBody = angleToTarg,
            targetPositionInWorldCoordinates = targetPoint,
            angleToTarget = angleToTargWorld
        };
    }
    class MovementInfo
    {
        public Vector3 moveCommandDirection;
        public float forwardMoveRelativeToBody;
        public float rightMoveRelativeToBody;
        public AimingInfo aimingInfo;
    }
    class AimingInfo
    {
        public float angleRelativeToBody;
        public Vector3 targetPositionInWorldCoordinates;
        public float angleToTarget;
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
