using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAnimations : MonoBehaviour
{
    Animator anim;
    Rigidbody rb;

    private float left;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnLeft45(AnimationEvent animationEvent)
    {
    
    }
    private AnimationEvent animationEvent;
    public void TurnRight45(AnimationEvent animationEvent)
    {
    }

}
