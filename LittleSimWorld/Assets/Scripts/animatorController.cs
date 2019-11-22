using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorController : MonoBehaviour

    
{
    public Animator anim;
    public float walkingSpeed = 0.01f;

    void Start()
    {
        
    }


    void Update()
    {
        anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
        anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));

        Vector3 temp = new Vector3(Input.GetAxis("Horizontal") * walkingSpeed,
                                   Input.GetAxis("Vertical") * walkingSpeed);

        gameObject.GetComponent<Rigidbody2D>().velocity = temp;
    }
}
