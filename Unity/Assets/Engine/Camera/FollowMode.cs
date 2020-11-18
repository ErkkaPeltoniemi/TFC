using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMode : MonoBehaviour
{
    public GameObject target;
    public Vector3 relativePosition;
    private Camera _camera;


    // Start is called before the first frame update
    void Start()
    {
        _camera = this.gameObject.GetComponent<Camera>();



    }

    // Update is called once per frame
    void Update()
    {
        _camera.transform.position = target.transform.position + relativePosition;
        
    }
}
