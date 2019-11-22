using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class Roof : MonoBehaviour
{
    public Collider2D roofCOllider;
    public bool Visible = false;
    public SlidingDoor slidingDoor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (roofCOllider.IsTouching(GameLibOfMethods.player.GetComponent<Collider2D>()) || slidingDoor.isDoorOpened)
        {
            StartCoroutine(FadeOut());

        }
        else
        {
            StartCoroutine(FadeIn());
        }
       
    }
    public IEnumerator FadeOut()
    {
        if (Visible)
        {
            Visible = false;
            GameLibOfMethods.isPlayerInside = true;
            float percentage = 0;
            while (true)
            {
                percentage += 0.04f;
                GetComponent<Tilemap>().color = new Color(GetComponent<Tilemap>().color.r, GetComponent<Tilemap>().color.g, GetComponent<Tilemap>().color.b,
                    Mathf.SmoothStep(1, 0, percentage));


                if (percentage >= 1f)
                {
                    
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
    public IEnumerator FadeIn()
    {
        
        if (!Visible)
        {
            Visible = true;
            float percentage = 0;
            GameLibOfMethods.isPlayerInside = false;
            while (true)
            {
                percentage += 0.04f;
                GetComponent<Tilemap>().color = new Color(GetComponent<Tilemap>().color.r, GetComponent<Tilemap>().color.g, GetComponent<Tilemap>().color.b, Mathf.SmoothStep(0, 1, percentage));


                if (percentage >= 1f)
                {
                    
                    yield break;
                }
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
