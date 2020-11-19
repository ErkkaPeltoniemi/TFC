using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCharacter : MonoBehaviour
{
    float playerSpeed = 1.55f;
    float playerMaxSpeed = 0.005f;
    float sideStepSpeed = 0.5f;
    Vector3 point;
    Plane plane;
    CharacterController charController;
    Animator anim;

    public float mass;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        point = Vector3.zero;
        plane = new Plane(Vector3.up, 0);
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        mass = 0.5f;
        

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
        var move = Vector3.zero;

        var targetPoint = screenPointToWorldPoint(Input.mousePosition);
        targetPoint.y = charController.transform.position.y;
        //charController.transform.LookAt(targetPoint);
        //charController.transform.rotation.Set(0f, charController.transform.rotation.y, 0f, 0f);

        if (Input.GetKey(KeyCode.W))
        {
            move += camera.transform.up * Time.deltaTime * playerSpeed;
        }

        if (Input.GetKey(KeyCode.S))
        {
            move += -camera.transform.up * Time.deltaTime * playerSpeed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += camera.transform.right * Time.deltaTime * playerSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move += -camera.transform.right * Time.deltaTime * playerSpeed;
        }

        move.y = 0f;

        var moveForward = Vector3.Dot(move, charController.transform.forward);
        var moveRight = Vector3.Dot(move, charController.transform.right);
        
        Vector3 deltaVec = targetPoint - charController.transform.position;
        Quaternion rotation = Quaternion.LookRotation(deltaVec);
   
        var moveSpeed = Vector3.Magnitude(move);


        moveForward /= moveSpeed;
        moveRight /= moveSpeed;

        charController.Move(move * Math.Max(sideStepSpeed, Math.Abs(moveForward)));

        //Debug.Log($"f: {(moveForward * 100f).ToString("000")} r: {(moveRight * 100f).ToString("000")}");




        anim.SetFloat("SpineAngle", Mathf.Clamp(rotation.y * 100f,-45,45));
        anim.SetFloat("Forward", moveForward);
        anim.SetFloat("Right", -moveRight);



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
