using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpriteControler : MonoBehaviour
{
    public Rigidbody2D playerRB;

    public SpriteRenderer Head;
    public SpriteRenderer Body;
    public Transform RightHand;
    public Transform LeftHand;
    [Space]
    public Sprite HeadDown, HeadRight, HeadLeft, HeadUp;
    public Sprite BodyDown, BodyRight, BodyLeft, BodyUp;
    public Sprite handSprite;
    public Transform RightHandDown, LeftHandDown, RightHandRight, LeftHandRight, RightHandLeft, LeftHandLeft, RightHandUp, LeftHandUp;
    public static SpriteControler Instance;
    [Space]
    public Animator anim;
    //[SerializeField] private bool isFacingSide = false;
    [Space]
    public AudioSource WalkingSource;
    public List<AudioClip> WalkingSounds;
    [Space]
    public AudioClip BumpingSound;
    public float MinBumpingVelocityMagnitude = 10;
    public float bumpingKnockbackPowerMultiplyer = 100;
    public float KnockbackTime = 0.5f;
    public float KnockbackCurrentTime;
    public bool BeignKnockbacked = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!GameLibOfMethods.cantMove && !GameLibOfMethods.doingSomething)
        {
            anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
            anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                DayNightCycle.Instance.ChangeSpeedToNormal();
            }
        }
        /*else
        {
            anim.SetFloat("Vertical", 0);
            anim.SetFloat("Horizontal", 0);
        }*/
    }
    private void FixedUpdate()
    {
        KnockbackCurrentTime += Time.deltaTime;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetAxisRaw("Vertical") < 0 && !GameLibOfMethods.cantMove && !GameLibOfMethods.doingSomething)
        {
            FaceDOWN();
        }
        if (Input.GetAxisRaw("Vertical") > 0 && !GameLibOfMethods.cantMove && !GameLibOfMethods.doingSomething)
        {
            FaceUP();
        }
        if (Input.GetAxisRaw("Horizontal") < 0 && !GameLibOfMethods.cantMove && !GameLibOfMethods.doingSomething)
        {
            FaceLEFT();
        }
        if (Input.GetAxisRaw("Horizontal") > 0 && !GameLibOfMethods.cantMove && !GameLibOfMethods.doingSomething)
        {
            FaceRIGHT();
        }
        

        if (playerRB.velocity.magnitude <= Vector2.one.magnitude && KnockbackCurrentTime > KnockbackTime)
        {
            BeignKnockbacked = false;
        }

        // Commented out this part to give way on the animation of the hands while walking - Vin
        /*
        if (GameLibOfMethods.facingDir == Vector3.up && !GameLibOfMethods.doingSomething && !GameLibOfMethods.cantMove && GameLibOfMethods.canInteract)
        {
            RightHand.position = Vector3.Lerp(RightHand.position, RightHandUp.position, Mathf.Abs(Input.GetAxis("Vertical")));
            LeftHand.position = Vector3.Lerp(LeftHand.position, LeftHandUp.position, Mathf.Abs(Input.GetAxis("Vertical")));
        }
        if (GameLibOfMethods.facingDir == Vector3.down && !GameLibOfMethods.doingSomething && !GameLibOfMethods.cantMove && GameLibOfMethods.canInteract)
        {
            RightHand.position = Vector3.Lerp(RightHand.position, RightHandDown.position, Mathf.Abs(Input.GetAxis("Vertical")));
            LeftHand.position = Vector3.Lerp(LeftHand.position, LeftHandDown.position, Mathf.Abs(Input.GetAxis("Vertical")));
        }
        if(GameLibOfMethods.facingDir == Vector3.right && !GameLibOfMethods.doingSomething && !GameLibOfMethods.cantMove && GameLibOfMethods.canInteract)
        {
            RightHand.position = Vector3.Lerp(RightHand.position, RightHandRight.position, Mathf.Abs(Input.GetAxis("Horizontal")));
            LeftHand.position = Vector3.Lerp(LeftHand.position, LeftHandRight.position, Mathf.Abs(Input.GetAxis("Horizontal")));  
        }
        if (GameLibOfMethods.facingDir == Vector3.left && !GameLibOfMethods.doingSomething && !GameLibOfMethods.cantMove && GameLibOfMethods.canInteract)
        {
            RightHand.position = Vector3.Lerp(RightHand.position, RightHandLeft.position, Mathf.Abs(Input.GetAxis("Horizontal")));
            LeftHand.position = Vector3.Lerp(LeftHand.position, LeftHandLeft.position, Mathf.Abs(Input.GetAxis("Horizontal")));
        }
        */
    }

    public void ResetPassOut() // Added at the end of PassOut animation
    {
        GameLibOfMethods.blackScreen.CrossFadeAlpha(1, 2, false);
        GameLibOfMethods.WakeUpAtHospital();
    }

    public void FaceUP()
    {
        Head.sprite = HeadUp;
        Body.sprite = BodyUp;

        // LeftHand.GetComponent<SpriteRenderer>().sortingOrder = Body.sortingOrder - 1;
        // RightHand.GetComponent<SpriteRenderer>().sortingOrder = Body.sortingOrder - 1;
        GameLibOfMethods.facingDir = Vector2.up;

        anim.SetFloat("FaceX", 0f);
        anim.SetFloat("FaceY", 1f);

        //isFacingSide = false;
        return;
    }
    public void FaceDOWN()
    {
        Head.sprite = HeadDown;
        Body.sprite = BodyDown;

        //LeftHand.GetComponent<SpriteRenderer>().sortingOrder = Body.sortingOrder + 1;
        //RightHand.GetComponent<SpriteRenderer>().sortingOrder = Body.sortingOrder + 1;
        GameLibOfMethods.facingDir = Vector2.down;

        anim.SetFloat("FaceX", 0f);
        anim.SetFloat("FaceY", 0f);

        //isFacingSide = false;
        return;
    }
    public void FaceRIGHT()
    {
        Head.sprite = HeadRight;
        Body.sprite = BodyRight;

        GameLibOfMethods.facingDir = Vector2.right;

        anim.SetFloat("FaceX", 1f);
        anim.SetFloat("FaceY", 0f);

        //isFacingSide = true;
        return;
    }
    public void FaceLEFT()
    {
        Head.sprite = HeadLeft;
        Body.sprite = BodyLeft;

        GameLibOfMethods.facingDir = Vector2.left;

        anim.SetFloat("FaceX", -1f);
        anim.SetFloat("FaceY", 0f);

        //isFacingSide = true;
        return;
    }
    public void PickRandomSound()
    {
        WalkingSource.clip = WalkingSounds[Random.Range(0, WalkingSounds.Count - 1)];
        WalkingSource.Play();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        float collisionForce = Vector2.Dot(col.contacts[0].normal, col.relativeVelocity) * playerRB.mass;
        if (collisionForce > MinBumpingVelocityMagnitude && KnockbackCurrentTime > 1)
        {
            /* BeignKnockbacked = true;
             Vector2 temp = new Vector2();
             for(int i = 0; i < col.contactCount; i++)
             {
                 temp += col.GetContact(i).relativeVelocity;
             }
             playerRB.AddForce(temp * bumpingKnockbackPowerMultiplyer);
             if(col.rigidbody)
             col.rigidbody.AddForce(-temp * bumpingKnockbackPowerMultiplyer);
             KnockbackCurrentTime = 0;
             Debug.Log(collisionForce);

             */
            AtommInventory.Instance.SpawnFX(BumpingSound);
        }

    }

}

