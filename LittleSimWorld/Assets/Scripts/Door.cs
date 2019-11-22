using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
    
{
    public bool isOpen;
    public Sprite OpenDoor;
    public Sprite ClosedDoor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InteractWithDoor()
    {
        

        if (!isOpen)
        {
            isOpen = true;
            GetComponent<Collider2D>().isTrigger = true;
            GetComponent<SpriteRenderer>().sprite = OpenDoor;
            return;
        }
        if (isOpen)
        {
            isOpen = false;
            GetComponent<Collider2D>().isTrigger = false;
            GetComponent<SpriteRenderer>().sprite = ClosedDoor;
            return;
        }
    }
}
