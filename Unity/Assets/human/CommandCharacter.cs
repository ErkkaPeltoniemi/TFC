using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCharacter : MonoBehaviour
{
    float playerSpeed = 2f;
    Vector3 point;
    Plane plane;
    CharacterController charController;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        point = Vector3.zero;
        plane = new Plane(Vector3.up, 0);
        charController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

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

        if (Input.GetKey(KeyCode.W))
        {
            move += camera.transform.up * Time.deltaTime* playerSpeed;        
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
        charController.Move(move);

        var targetPoint = screenPointToWorldPoint(Input.mousePosition);
        targetPoint.y = charController.transform.position.y;
        var charPos = new Vector3(charController.transform.position.x, 0f, charController.transform.position.z);

        var moveSpeed = Vector3.Magnitude(move);
        var moveForward = Vector3.Dot(move, charController.transform.forward);
        moveForward /= moveSpeed;
        anim.SetFloat("Forward", moveForward);

        charController.transform.LookAt(targetPoint);
        charController.transform.rotation.Set(0f, charController.transform.rotation.y, 0f, 0f);
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
