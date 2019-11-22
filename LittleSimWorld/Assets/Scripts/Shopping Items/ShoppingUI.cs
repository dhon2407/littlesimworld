using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

public class ShoppingUI : MonoBehaviour
{
    public GameObject mouseOverDetailsPrefab;
    public static MouseOverDetails detailsInstance;

    private bool isEnable;

    public Transform shopItemsContent;
    

    // Start is called before the first frame update
    void Start()
    {



        if (detailsInstance == null)
        {
            detailsInstance = Instantiate(mouseOverDetailsPrefab, transform).GetComponent<MouseOverDetails>();
        }

       
    }


    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }


  
}
