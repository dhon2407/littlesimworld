using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portrait : MonoBehaviour
{
    public Camera PortraitCamera;
    // Start is called before the first frame update
    void Start()
    {
        UpdatePortrait();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdatePortrait()
    {
        PortraitCamera.Render();
    }
}
