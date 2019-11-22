using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUpgradeSoundHandler : MonoBehaviour
{
    [SerializeField] private AudioClip open, close;

    [SerializeField] private AudioSource source;

    public void PlayOpenCloseSound()
    {
        if (source.clip == open)
        {
            source.clip = close; 
            source.Play();
        }
        else
        {
            source.clip = open; 
            source.Play();
        }
    }
}
