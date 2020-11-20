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
        rb.transform.RotateAround(rb.position, rb.transform.up, -45);
    }
    private AnimationEvent animationEvent;
    public void TurnRight45(AnimationEvent animationEvent)
    {
        rb.transform.RotateAround(rb.position, rb.transform.up, 45);
        anim.SetFloat("Horizontal", 0f);
    }
    private IEnumerable TurnRight()
    {
        if (animationEvent != null)
        {
            for (float i = 0; i <= 45; i++)
            {
                var degrees = 45;
                var power = Mathf.Lerp(0, degrees, animationEvent.animationState.time / animationEvent.animationState.length);
                Debug.Log("power: " + power);
                
                yield return null;
            }
        }
    }
}
