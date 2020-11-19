﻿using System;
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

    void FixedUpdate()
    {

      
        var camera = Camera.main;
        
        var targetPoint = screenPointToWorldPoint(Input.mousePosition);
        targetPoint.y = rb.transform.position.y;
        //charController.transform.LookAt(targetPoint);
        //charController.transform.rotation.Set(0f, charController.transform.rotation.y, 0f, 0f);
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
            acceleration *= 0.95f;
        }
        acceleration.y = 0f;

        acceleration = Vector3.ClampMagnitude(acceleration, maxSpeed);
        
        rb.MovePosition(transform.position+acceleration);

        //Debug.Log(Vector3.Magnitude(acceleration));
        //charController.Move(acceleration);
        var speedRelativeToMax = Vector3.Magnitude(acceleration / maxSpeed);
        Debug.Log(speedRelativeToMax * 100f);

        var moveForward = Vector3.Dot(acceleration, rb.transform.forward);
        var moveRight = Vector3.Dot(acceleration, rb.transform.right);
        
        Vector3 deltaVec = targetPoint - rb.transform.position;
        Quaternion rotation = Quaternion.LookRotation(deltaVec);
   
        var moveSpeed = Vector3.Magnitude(acceleration);
        //Debug.Log(moveSpeed);


        moveForward /= moveSpeed;
        moveRight /= moveSpeed;

        anim.SetFloat("SpineAngle", Mathf.Clamp(rotation.y * 100f,-45,45));
        anim.SetFloat("Forward", speedRelativeToMax,0.3f,Time.deltaTime);
       



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
