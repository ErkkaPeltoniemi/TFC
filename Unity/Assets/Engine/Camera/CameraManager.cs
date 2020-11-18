using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera _camera;
    private FollowMode followMode;
    private FreeLookMode freeLookMode;

    // Start is called before the first frame update
    void Start()
    {
        _camera = this.gameObject.GetComponent<Camera>();
        followMode = GetComponent<FollowMode>();
        freeLookMode = GetComponent<FreeLookMode>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToFreeLookMode()
    {
        freeLookMode.enabled = true;
        followMode.enabled = false;
    }
    public void ToFollowMode(GameObject gameObject)
    {
        freeLookMode.enabled = false;
        followMode.enabled = true;
        followMode.target = gameObject;
    }
}
