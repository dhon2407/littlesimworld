using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public float maxDoorPosition;
    public float doorSpeed = 0.1f;
    public Transform leftDoor;
    public Transform rightDoor;
    public float distanceFromPlayerToOpen = 2;
    private Vector2 endLeftPosition;
    private Vector2 endRightPosition;
    private Vector2 startLeftPosition;
    private Vector2 startRightPosition;
    [Space]
    public float openTimeInSeconds = 21600;
    public float closeingTimeInSeconds = 79200;
    public SpriteRenderer OpenedOrClosedRenderer;

    public Sprite OpenedSprite;
    public Sprite ClosedSprite;

    public BoxCollider2D ShopZone;
    public GameObject ClosingMessege;
    public Animator anim;
    public bool isDoorOpened = false;


    // Start is called before the first frame update
    void Start()
    {
        startLeftPosition = leftDoor.transform.position;
        startRightPosition = rightDoor.transform.position;

        endLeftPosition = new Vector2(leftDoor.position.x - maxDoorPosition, leftDoor.position.y);
        endRightPosition = new Vector2(rightDoor.position.x + maxDoorPosition, rightDoor.position.y);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector2.Distance(GameLibOfMethods.player.transform.position, transform.position) < distanceFromPlayerToOpen &&
            DayNightCycle.Instance.time >= openTimeInSeconds && DayNightCycle.Instance.time < closeingTimeInSeconds )
        {
            OpenDoor();
            if (ClosingMessege)
                ClosingMessege.SetActive(false);
        }
        else if (ShopZone != null && !ShopZone.IsTouching(GameLibOfMethods.player.GetComponent<Collider2D>()))
        {
            CloseDoor();
            if (ClosingMessege)
                ClosingMessege.SetActive(false);
        }
        else if (ShopZone != null && ShopZone.IsTouching(GameLibOfMethods.player.GetComponent<Collider2D>()) && DayNightCycle.Instance.time >= closeingTimeInSeconds)
        {
            if(ClosingMessege)
            ClosingMessege.SetActive(true);
        }

        if (DayNightCycle.Instance.time >= openTimeInSeconds && DayNightCycle.Instance.time < closeingTimeInSeconds)
        {
            if (OpenedOrClosedRenderer != null)
                OpenedOrClosedRenderer.sprite = OpenedSprite;
        }
        else
        {
            if (OpenedOrClosedRenderer != null)
                OpenedOrClosedRenderer.sprite = ClosedSprite;
        }

        if (DayNightCycle.Instance.time >= openTimeInSeconds && DayNightCycle.Instance.time < closeingTimeInSeconds)
        {
            if (anim != null && anim.GetFloat("OpenedClosed") < 1)
                anim.SetFloat("OpenedClosed", anim.GetFloat("OpenedClosed") + 0.02f);
        }
        else
        {
            if (anim != null && anim.GetFloat("OpenedClosed") > 0)
                anim.SetFloat("OpenedClosed", anim.GetFloat("OpenedClosed") - 0.02f);
        }
    }

    public void OpenDoor()
    {
        
        leftDoor.position = Vector2.Lerp(leftDoor.position, endLeftPosition, doorSpeed);
        
        rightDoor.position = Vector2.Lerp(rightDoor.position, endRightPosition, doorSpeed);

        isDoorOpened = true;
      


    }
    public void CloseDoor()
    {
        leftDoor.position = Vector2.Lerp(leftDoor.position, startLeftPosition, doorSpeed);

        rightDoor.position = Vector2.Lerp(rightDoor.position, startRightPosition, doorSpeed);

        isDoorOpened = false;
      
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GameLibOfMethods.canInteract = true;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && DayNightCycle.Instance.time >= closeingTimeInSeconds)
        {
            GameLibOfMethods.canInteract = false;
        }
    }
}
