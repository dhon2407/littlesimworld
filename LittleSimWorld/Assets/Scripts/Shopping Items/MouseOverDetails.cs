using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MouseOverDetails : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI ingredient;
    public TextMeshProUGUI details;
    public Image image;

    private Sprite defaultSprite;

    private void Start()
    {
        defaultSprite = image.sprite;
        Reset();
    }

    public void UpdateDetails(string name, string ingredient, string details, Sprite icon)
    {
        this.itemName.text = name;
        this.ingredient.text = ingredient;
        this.details.text = details;
        image.sprite = icon;

    }


    public void Reset()
    {
        this.itemName.text = "";
        this.ingredient.text = "";
        this.details.text = "";
        image.sprite = defaultSprite;

    }



}
