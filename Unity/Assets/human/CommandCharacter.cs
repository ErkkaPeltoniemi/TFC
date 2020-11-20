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
    int turns = 0;
    void Update()
    {
        var camera = Camera.main;

        var mv = GetMovementInfo();
        anim.SetFloat("FeetAngle", mv.aimingInfo.angleRelativeToBody);
        Debug.Log($"m: {mv.moveCommandDirection} f: {mv.forwardMoveRelativeToBody} r: {mv.rightMoveRelativeToBody}, a: {mv.aimingInfo.angleToTarget}");

        var targetPoint = screenPointToWorldPoint(Input.mousePosition);
        targetPoint.y = rb.transform.position.y;

        bool moveCommand = false;

        if (Input.GetKey(KeyCode.W))
        {
            acceleration +=  (camera.transform.up * Time.deltaTime * mass);
            moveCommand = true;
        }

        if (Input.GetKey(KeyCode.S))
        {
            acceleration +=  (-camera.transform.up * Time.deltaTime * mass);
            moveCommand = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            acceleration += (camera.transform.right * Time.deltaTime * mass);
            moveCommand = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            acceleration += (-camera.transform.right * Time.deltaTime * mass);
            moveCommand = true;
        }
        if (!moveCommand)
        {
            acceleration *= 0.90f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if(Input.GetKeyDown(KeyCode.D))
            {
                anim.SetFloat("Horizontal", -1f);
            }
            

        } else
        {
        }

        acceleration.y = 0f;

        acceleration = Vector3.ClampMagnitude(acceleration, maxSpeed);
        
        rb.MovePosition(transform.position+acceleration);

        //Debug.Log(Vector3.Magnitude(acceleration));
        //charController.Move(acceleration);
        var speedRelativeToMax = Vector3.Magnitude(acceleration / maxSpeed);
        //Debug.Log(speedRelativeToMax * 100f);

        var moveForward = Vector3.Dot(acceleration, rb.transform.forward);
        var moveRight = Vector3.Dot(acceleration, rb.transform.right);
        
        Vector3 deltaVec = targetPoint - rb.transform.position;
        Quaternion rotation = Quaternion.LookRotation(deltaVec);
   
        var moveSpeed = Vector3.Magnitude(acceleration);
        //Debug.Log(moveSpeed);


        moveForward /= moveSpeed;
        moveRight /= moveSpeed;

        anim.SetFloat("SpineAngle", mv.aimingInfo.angleRelativeToBody);
        anim.SetFloat("Forward", speedRelativeToMax,0.3f,Time.deltaTime);
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

        return moveDir.normalized;
    }

    private MovementInfo GetMovementInfo()
    {
        var moveDir = GetCurrentMoveCommand();

        var moveForwardRelativeToBody = Vector3.Dot(moveDir, rb.transform.forward);
        var moveRightRelativeToBody = Vector3.Dot(moveDir, rb.transform.right);

        return new MovementInfo
        {
            forwardMoveRelativeToBody = moveForwardRelativeToBody,
            rightMoveRelativeToBody = moveRightRelativeToBody,
            moveCommandDirection = moveDir,
            aimingInfo = GetCurrentAimingInfo()
        };
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
