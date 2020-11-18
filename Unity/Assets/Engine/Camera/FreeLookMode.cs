using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookMode : MonoBehaviour
{
    // Start is called before the first frame update
    public float cameraSpeed;
    private Camera _camera;
    void Start()
    {
        _camera = this.gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboard();
     
    }

    private void HandleKeyboard()
    {
        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            dir += Vector3.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir += -Vector3.up;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir += -Vector3.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += Vector3.right;
        }

        _camera.transform.Translate(dir * Time.deltaTime * cameraSpeed, Space.Self);
    }
}
