using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BuyItem : MonoBehaviour
{
    public Image icon;
    public Text itemName;
    public Text quantity;
    public Text itemCost;

    private string _name;
    private Sprite _texture;
    private float _itemCost;

  

    public AtommInventory.Purchasable shoppingItem;

    private void Start()
    {
        shoppingItem.quantity = 1;
        _itemCost = shoppingItem.quantity * shoppingItem.price;
    }

   


    public void OnButtonClick()
    {
        int i = BuyingList.FindIndexInList(shoppingItem);
        BuyingList.DecrementCost(i);
    }



    public void Decrement(int itemIndex)
    {
        if (shoppingItem.quantity > 1)
        {
            shoppingItem.quantity--;
            _itemCost = shoppingItem.quantity * shoppingItem.price;
            UpdateUI();
            BuyingList.CalculateTotalCost();
        }
        else
        {
            
            BuyingList.DeleteFromList(itemIndex);
            BuyingList.CalculateTotalCost();
            Destroy(gameObject);
        }
    }

    public void Increment()
    {
        shoppingItem.quantity++;
        _itemCost = shoppingItem.quantity * shoppingItem.price;
        UpdateUI();
        BuyingList.CalculateTotalCost();
    }


    


    public float TotalCost
    {
        get
        {
            return _itemCost;
        }
        
    }


    public void UploadData()
    {
        _texture = Resources.Load<GameObject>("Prefabs/" + shoppingItem.itemName).GetComponent<SpriteRenderer>().sprite; ;
        _name = shoppingItem.itemName;
        shoppingItem.quantity = 1;
        _itemCost = shoppingItem.price;
        UpdateUI();
    }


    private void UpdateUI()
    {
        icon.sprite = _texture;
        itemName.text = _name;
        quantity.text = shoppingItem.quantity.ToString();
        itemCost.text = "£" + _itemCost.ToString();

    }
}
