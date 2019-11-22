using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtommContainer : MonoBehaviour
{
    public List<AtommInventory.Slot> slots;

    public int Capacity = 10;

    public AudioClip open, close;

    public Transform containerInstance;

    public GameObject WhereToDropItems;



    AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Action()
    {
        if (source.clip == open)
        { source.clip = close; source.Play(); }
        else
        { source.clip = open; source.Play(); }
    }
}