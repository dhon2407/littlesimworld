using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GlowEffect : MonoBehaviour
{
    
    private Image GlowingImage;
    [Header("Glow effect")]
    public float approachSpeed = 0.02f;
    public float growthBound = 2f;
    public float shrinkBound = 0.5f;
    public float currentRatio = 1;

    public bool keepGoing = false;
    private bool closeEnough = false;
    public Color GlowingColor;
    private Color StartingColor;
    // Start is called before the first frame update
    void Start()
    {
        GlowingImage = gameObject.GetComponent<Image>();
        StartCoroutine(GlowOfOutline());
        StartingColor = GlowingImage.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator GlowOfOutline()
    {
        // Run this indefinitely
        
        while (true)
        {
            if (keepGoing)
            {

            
                
            // Get bigger for a few seconds
            while (currentRatio != growthBound)
            {
                // Determine the new ratio to use
                currentRatio = Mathf.MoveTowards(currentRatio, growthBound, approachSpeed);

                    // Update our text element
                    
                GlowingImage.color = Color.Lerp(StartingColor, GlowingColor, currentRatio);

                    //Debug.Log("growing");
                yield return new WaitForFixedUpdate();
            }

            // Shrink for a few seconds
            while (currentRatio != shrinkBound)
            {
                // Determine the new ratio to use
                currentRatio = Mathf.MoveTowards(currentRatio, shrinkBound, approachSpeed);

                    // Update our text element
                    GlowingImage.color = Color.Lerp(StartingColor, GlowingColor, currentRatio);
                    //Debug.Log("shrinking");
                    yield return new WaitForFixedUpdate();
            }
            }
            else
            {
                GlowingImage.color = StartingColor;
                yield return new WaitForFixedUpdate();
            }
            //yield return new WaitForFixedUpdate();
        }
    }
}
