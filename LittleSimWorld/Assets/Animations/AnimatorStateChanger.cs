using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorStateChanger : MonoBehaviour
{
    public Animator anim;
    public float walkingSpeed;
    
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (Input.GetAxis("Horizontal") < 0)
            anim.SetBool("Left", true);
        else
            anim.SetBool("Left", false);

        if (Input.GetAxis("Vertical") > 0)
            anim.SetBool("Up", true);
        else
            anim.SetBool("Up", false);

        if (Input.GetAxis("Horizontal") > 0)
            anim.SetBool("Right", true);
        else
            anim.SetBool("Right", false);
        */
        Vector3 temp = new Vector3(Input.GetAxis("Horizontal") * walkingSpeed,
                                   Input.GetAxis("Vertical") * walkingSpeed) * PlayerStatsManager.Instance.MovementSpeed;
        temp = Vector3.ClampMagnitude(temp, walkingSpeed);
        if (!GameLibOfMethods.cantMove && !GameLibOfMethods.sitting && !SpriteControler.Instance.BeignKnockbacked)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = temp;
            if(temp.magnitude != 0)
            anim.SetBool("Walking", true);
            else
            {
                anim.SetBool("Walking", false);
            }
                
        }
        else
        {
            anim.SetBool("Walking", false);
        }
        
    }
}
