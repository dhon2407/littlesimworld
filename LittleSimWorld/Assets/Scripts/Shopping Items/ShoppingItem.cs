using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ShoppingItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    // public Item item;

    public int itemId;
    public Image icon;
    public Text itemName;
    public Text cost;
    public AtommInventory.Purchasable DiscountedPurchasable;
    public Shop shop;
    //private Item itemSO;
    

    // Start is called before the first frame update
    void Start()
    {
        
        //itemSO = GetComponent<ItemSlot>().itemSO;
       

        itemName.text = DiscountedPurchasable.itemName;
        cost.text = "£" + DiscountedPurchasable.price.ToString();
        icon.sprite = Resources.Load<GameObject>("Prefabs/"+ DiscountedPurchasable.itemName).GetComponent<SpriteRenderer>().sprite;

        gameObject.name = DiscountedPurchasable.itemName;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.LogError("Mouse Over is working");
        
       // ShoppingUI.detailsInstance.gameObject.SetActive(true);
      //  ShoppingUI.detailsInstance.GetComponent<MouseOverDetails>().UpdateDetails(itemSO.ItemName, itemSO.ingrediant ,itemSO.Description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
       // Debug.LogError("Mouse Out Is Working");
      //  ShoppingUI.detailsInstance.GetComponent<MouseOverDetails>().gameObject.SetActive(false);
    }

   

   
   


    public void OnButtonClick()
    {
       

        if (BuyingList.CheckInBuyingList(DiscountedPurchasable))
        {
            int i = BuyingList.FindIndexInList(DiscountedPurchasable);
            BuyingList.IncrementCost(i);

        }
        else
        {
            BuyingList.AddToList(DiscountedPurchasable);
        }
    }



}
