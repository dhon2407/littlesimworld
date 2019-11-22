using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;
public class LightTrigger : MonoBehaviour
{
    public UnityEngine.Experimental.Rendering.Universal.Light2D LightSource;
    public float LightIntensity = 0.85f;
    public float DistanceToEnable;
    public float TimeToEnable;
    public float TimeToDisable;
    public Animator anim;
    public bool DependsOnSunIntensity = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (DependsOnSunIntensity)
        {
            LightSource.intensity = LightIntensity - DayNightCycle.Instance.light2d.intensity;
        }
       
       /* if (Vector2.Distance(GameLibOfMethods.player.transform.position, LightSource.transform.position) < LightSource.range)
        {
            LightSource.renderMode = LightRenderMode.ForcePixel;
            
        }
        else
        {
            LightSource.renderMode = LightRenderMode.ForceVertex;
        }*/
        if (DayNightCycle.Instance.time >= TimeToEnable || DayNightCycle.Instance.time < TimeToDisable)
        {
            anim.SetBool("Enabled", true);
        }
       else
        {
            anim.SetBool("Enabled", false);
        }
    }
    /*private void OnBecameInvisible()
    {
        LightSource.renderMode = LightRenderMode.ForceVertex;
    }*/
}
