using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : BreakableFurniture
        
{
    public Canvas LaptopOptionsCanvas;
    public bool IsFacingDown;

    void Update()
    {
    }
    public void ActivateChoices()
    {
        LaptopOptionsCanvas.gameObject.SetActive(true);
    }
    public void DisableChoices()
    {
        LaptopOptionsCanvas.gameObject.SetActive(false);
    }
}
