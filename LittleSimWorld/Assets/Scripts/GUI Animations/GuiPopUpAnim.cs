using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiPopUpAnim : MonoBehaviour
{
    [Header("Game Object For Transition")]
    public RectTransform mainWindow;

    [Header("Position")]
    public Vector3 popInVector;
    public Vector3 popOutVector;

    [Header("Scale")]
    public Vector3 popInScale;
    public Vector3 popOutScale;

    [Header("Animation Type")]
    public LeanTweenType typeIn,typeOut;
    public bool JustMoveAnimation = false;

    [Header("Duration Of Animation")]
    public float delay;

    [Header("Settings")]
    public bool CheckIfSliding = true; //if this is checked, animation will play even if it is inside another animation.
    public bool CloseWindowOnEnable = true;
    [Header("Just For Testing")]
   
    public bool usingLayoutGroup;
    public bool isVisible = false;
    public bool isSliding = false;
    
    public HorizontalLayoutGroup hLayout;

    
    // Start is called before the first frame update
    void Start()
    {
        if (CloseWindowOnEnable)
        {
            CloseWindow();
        }
    }

    // Update is called once per frame
    void Update()
    {
     
       
    }




    //Public Functions
    public void CloseWindow()
    {
    if(this && !isSliding || !CheckIfSliding)
        StartCoroutine(PopOut());

    }
    public void OpenWindow()
    {
        if (!isSliding || !CheckIfSliding)
            StartCoroutine(PopIn());
    }
    public void SwitchWindow()
    {
         switch (transform.localScale == Vector3.zero || !isVisible)
        {
            case false:
                {
                    
                    CloseWindow();
                    break;
                }
            case true:
                {
                    
                        OpenWindow();
                    break;
                }
        }
    }



    /// <summary>
    /// Private Functions Below
    /// </summary>


    private void OnEnable()
    {
        if(CloseWindowOnEnable)
        StartCoroutine(PopIn());

    }

    private IEnumerator PopIn()
    {

        isSliding = true;
       
        mainWindow.localScale = popOutScale;
        mainWindow.localPosition = popOutVector;



        if (usingLayoutGroup)
        {
            // First Gonna Find Positioning According To Layout Group

            mainWindow.gameObject.SetActive(true);
            mainWindow.localPosition = popInVector;
            mainWindow.localScale = popInScale;
            mainWindow.GetComponent<CanvasGroup>().alpha = 0;



            hLayout.enabled = true;
            yield return new WaitForSeconds(0.01f);
            hLayout.enabled = false;


            popInVector = new Vector3(mainWindow.transform.localPosition.x, popInVector.y, popInVector.z);

            mainWindow.localScale = popOutScale;
            mainWindow.localPosition = new Vector3(popOutVector.x, popOutVector.y * 2, popOutVector.z);

        }
        if (usingLayoutGroup)
        {
            if (!JustMoveAnimation)
            {


                mainWindow.gameObject.SetActive(true);
                LeanTween.move(mainWindow, popInVector, delay);
                LeanTween.scale(mainWindow, popInScale, delay).setEase(typeIn);

                LeanTween.alphaCanvas(mainWindow.GetComponent<CanvasGroup>(), 1f, delay);
            }
            else
            {
                mainWindow.gameObject.SetActive(true);
                LeanTween.moveLocal(mainWindow.gameObject, popInVector, delay);
            }
        }
        else
        {
            if (!JustMoveAnimation)
            {
                LeanTween.move(mainWindow, popInVector, delay);
                LeanTween.scale(mainWindow, popInScale, delay).setEase(typeIn);

                LeanTween.alpha(mainWindow, 1f, delay);
            }
            else
            {
                LeanTween.moveLocal(mainWindow.gameObject, popInVector, delay);
            }
        }

            yield return new WaitForSecondsRealtime(delay);
            isVisible = true;
        isSliding = false;



    }


    private IEnumerator PopOut()
    {
        isSliding = true;
        if (!JustMoveAnimation)
        {
            LeanTween.move(mainWindow, popOutVector, delay);
            LeanTween.scale(mainWindow, popOutScale, delay).setEase(typeOut);
        }
        else
        {
            LeanTween.moveLocal(mainWindow.gameObject, popOutVector, delay);
        }

            if (usingLayoutGroup)
            {
            if (!JustMoveAnimation)
            {
                LeanTween.alphaCanvas(mainWindow.GetComponent<CanvasGroup>(), 0f, delay);
            }
            else
            {

            }
            }
            else
            {
            if (!JustMoveAnimation)
            {
                LeanTween.alpha(mainWindow, 0f, delay);
            }
            }

            yield return new WaitForSeconds(0.1f);
            if (usingLayoutGroup)
            {
                hLayout.enabled = true;
                yield return new WaitForSeconds(0.1f);
                hLayout.enabled = false;

            }
            yield return new WaitForSeconds(delay);
            if (usingLayoutGroup)
            {
                mainWindow.gameObject.SetActive(false);
            }
            yield return new WaitForSecondsRealtime(delay);
            isVisible = false;
        isSliding = false;


    }


  
}
