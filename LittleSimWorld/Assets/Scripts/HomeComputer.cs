using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeComputer : BreakableFurniture
{
    public Canvas LaptopOptionsCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   /* void Update()
    {
        if(Vector2.Distance(GameLibOfMethods.player.transform.position, gameObject.transform.position) > 2 || GameLibOfMethods.doingSomething|| GameLibOfMethods.cantMove)
        {
            DisableChoices();
        }
    }
    public void ActivateChoices()
    {
        LaptopOptionsCanvas.gameObject.SetActive(true);
    }
    public void DisableChoices()
    {
        LaptopOptionsCanvas.gameObject.SetActive(false);
    }
    public void Study()
    {
        GameLibOfMethods.lastInteractable = gameObject;
        StartCoroutine(InteractionChecker.Instance.SitDown());
    }*/
}
