using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShoppingItems 
{
    public int id;
    public string name;
    public string ingredient;
    public string description;
    public float cost;
    public int quantity;
    public float currentCoolDown;
    public Sprite texture;


    

    public ShoppingItems(int id, string name, string ingredient, string description, float cost, int quantity, float currentCoolDown, Sprite texture )
    {
        this.id = id;
        this.name = name;
        this.ingredient = ingredient;
        this.description = description;
        this.cost = cost;
        this.quantity = quantity;
        this.currentCoolDown = currentCoolDown;
        this.texture = texture;
    }

}
