using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCharacter : MonoBehaviour
{
    float playerSpeed = 2f;
    Vector3 point;
    Plane plane;
    // Start is called before the first frame update
    void Start()
    {
        point = Vector3.zero;
        plane = new Plane(Vector3.up, 0);
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

        if (Input.GetKey(KeyCode.W))
        {
            var move = camera.transform.up * Time.deltaTime* playerSpeed;
            move.y = 0f;
            this.gameObject.transform.position += move;
        }

        if (Input.GetKey(KeyCode.S))
        {
            var move = camera.transform.up * Time.deltaTime * playerSpeed;
            move.y = 0f;
            this.gameObject.transform.position -= move;
        }
        if (Input.GetKey(KeyCode.A))
        {
            var move = camera.transform.right * Time.deltaTime * playerSpeed;
            move.y = 0f;
            this.gameObject.transform.position -= move;
        }
        if (Input.GetKey(KeyCode.D))
        {
            var move = camera.transform.right * Time.deltaTime * playerSpeed;
            move.y = 0f;
            this.gameObject.transform.position += move;
        }

        var targetPoint = screenPointToWorldPoint(Input.mousePosition);
        var yAngle = Vector3.SignedAngle(transform.position, targetPoint, Vector3.up);


        transform.LookAt(targetPoint);
        transform.rotation.Set(0f, transform.rotation.y, 0f, 0f);
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
