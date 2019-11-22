using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shower : BreakableFurniture
{
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (LoopingParticleSystem)
        {
            Emission = LoopingParticleSystem.emission;
            Emission.enabled = false;
            SoundSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
