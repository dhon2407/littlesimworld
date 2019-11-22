﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{

    public float slowDownFactor;
    public float slowDownLength;
    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

    // Update is called once per frame
    void Update()
    {
       // Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
      //  Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);

       
    }


    public void SlowMotion()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
