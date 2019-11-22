using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDependentDoor : MonoBehaviour
{
    public bool isOpen;
    public Sprite OpenedDoor;
    public Sprite ClosedDoor;
    public float openTimeInSeconds = 21600;
    public float closeingTimeInSeconds = 79200;

    private void Update()
    {
        
         if(DayNightCycle.Instance.time >= openTimeInSeconds  && DayNightCycle.Instance.time < closeingTimeInSeconds)
        {
            OpenDoor();
        }
        else if(DayNightCycle.Instance.time >= openTimeInSeconds && DayNightCycle.Instance.time >= closeingTimeInSeconds)
        {
            CloseDoor();
        }
    }
    public void CloseDoor()
    {
            isOpen = false;
            GetComponent<Collider2D>().isTrigger = false;
            GetComponent<SpriteRenderer>().sprite = ClosedDoor;
            return;
    }
    public void OpenDoor()
    {
        isOpen = true;
        GetComponent<Collider2D>().isTrigger = true;
        GetComponent<SpriteRenderer>().sprite = OpenedDoor;
        return;
    }
}
