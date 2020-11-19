using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMode : MonoBehaviour
{
    public GameObject target;
    public Vector3 relativePosition;
    private Camera _camera;

    public float cameraHeight;

    // Start is called before the first frame update
    void Start()
    {
        _camera = this.gameObject.GetComponent<Camera>();



    }

    // Update is called once per frame
    void Update()
    {
        var pos = target.transform.position + relativePosition;
        pos.y = cameraHeight;
        _camera.transform.position = pos;
    }
}
