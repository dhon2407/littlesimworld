using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyingList : MonoBehaviour
{
    public GameObject buyItemPrefab;
    public TextMeshProUGUI totalAmount;

    public static List<BuyItem> prefabs;

    public static bool canInitialize;
    public static bool updateDataNow;

    private static AtommInventory.Purchasable currentItem;

    private static float totalCost;


    // For Inventory
    int freeCells;

    // Start is called before the first frame update
    void Start()
    {
        canInitialize = false;
        updateDataNow = false;
        prefabs = new List<BuyItem>();
    }

    private void Update()
    {
        if (canInitialize)
        {
            canInitialize = false;
            InstantiatePrefab();
        }

        if (updateDataNow)
        {
            updateDataNow = false;
            UpdateTotalAmountUI();
        }
    }

    public static bool CheckInBuyingList(AtommInventory.Purchasable item)
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            BuyItem tmp = prefabs[i];
            if (tmp.shoppingItem.itemName == item.itemName)
            {
                return true;
            }
        }
        return false;


    }


    public static int FindIndexInList(AtommInventory.Purchasable item)
    {
        for (int i = 0; i < prefabs.Count; i++)
        {
            if (prefabs[i].shoppingItem.itemName == item.itemName)
            {
                return i;
            }

        }
        return -1;
    }

    public static void AddToList(AtommInventory.Purchasable item)
    {
        currentItem = item;
        canInitialize = true;

    }




    void InstantiatePrefab()
    {
        GameObject go = Instantiate(buyItemPrefab, transform);

        BuyItem a = go.GetComponent<BuyItem>();
        a.shoppingItem = currentItem;

        prefabs.Add(go.GetComponent<BuyItem>());

        prefabs[prefabs.Count - 1].UploadData();

        CalculateTotalCost();

        updateDataNow = true;
    }



    public static void DeleteFromList(int itemIndex)
    {
        prefabs.RemoveAt(itemIndex);
    }

    public static void ClearList()
    {
        prefabs.Clear();
    }

    public static void IncrementCost(int itemIndex)
    {

        prefabs[itemIndex].Increment();

    }

    public static void DecrementCost(int itemIndex)
    {
        prefabs[itemIndex].Decrement(itemIndex);
    }

    public static void CalculateTotalCost()
    {
        totalCost = 0;
        for (int i = 0; i < prefabs.Count; i++)
        {
            totalCost += prefabs[i].TotalCost;
        }
        updateDataNow = true;

    }


    private void UpdateTotalAmountUI()
    {
        totalAmount.text = "Total: " + "£" + totalCost.ToString();
    }


    public void BuyAllInCart()
    {

        freeCells = 0;

        foreach(DragAndDropCell cell in AtommInventory.DadCells)
        {
            if (cell.GetItem() == null && !cell.cellType.Equals(DragAndDropCell.CellType.TrashBin))
            {
                freeCells++;
            }
        }


        if (prefabs.Count > (AtommInventory.InventoryCapacity - 1))
        {
            GameLibOfMethods.CreateFloatingText("Not enough space in inventory 11", 2);
            return;
        }

        bool canProceedToPayment = true;
        bool availableInInventory = false;

     

        // Test Run Start Before Purchasing
        if (CanPurchaseItems()) // Do I have Enough Money?
        {
            for (int i = 0; i < prefabs.Count; i++)
            {
                availableInInventory = false;

                int q = prefabs[i].shoppingItem.quantity; // quantity to be added in inventory
                //Debug.LogError("quantity to be bought is: " + q);
                foreach (AtommInventory.Slot slot in AtommInventory.inventory)
                {

                    foreach (DragAndDropCell cell in AtommInventory.DadCells)
                    {
                        
                        string tmpName = slot.itemName;
                        int temp = slot.quantity;
                        if (cell.myDadItem &&
                           cell.myDadItem.gameObject.GetComponent<ItemSlot>().ID == slot.ID &&
                           tmpName == prefabs[i].shoppingItem.itemName &&
                           slot.quantity < AtommInventory.slotCount &&
                           !cell.cellType.Equals(DragAndDropCell.CellType.TrashBin) &&
                           prefabs[i].shoppingItem.consumableItem.stackableItem)
                        {

                            int s = AtommInventory.slotCount - slot.quantity;
                            Debug.LogError("Item is already available" + "q:" + q + " s:" + s);
                            if (q <= s)
                            {
                                q = 0;
                            }
                            else
                            {
                                q -= s;
                            }

                            availableInInventory = true;

                            break;
                        }

                    }
                }



                if (!CanAdjustInFreeCells(prefabs[i].shoppingItem, q, availableInInventory)) // If we can not adjust number of items in remaining free cells
                {
                    canProceedToPayment = false; // Hence can not proceed to payment
                    break;                            //break; // Break the loop
                }


            }

            if (!canProceedToPayment) // If we can not proceed to payment
            {
                GameLibOfMethods.CreateFloatingText("Not enough space in inventory", 2);
            }

            else
            {
                for (int i = 0; i < prefabs.Count; i++)
                {

                    AtommInventory.Purchasable si = prefabs[i].shoppingItem;

                    if (availableInInventory)
                    {
                        AtommInventory.Instance.BuyItem(si, true);
                    }
                    else
                    {
                        AtommInventory.Instance.BuyItem(si, false);
                    }



                }

                foreach (BuyItem a in prefabs)
                {
                    Destroy(a.gameObject);
                }

                prefabs.Clear();
                totalCost = 0;
                updateDataNow = true;

            }
        }



        // END


    }


    // Important Ones

    bool CanPurchaseItems()
    {
        if (totalCost > PlayerStatsManager.Instance.Money)
        {
            GameLibOfMethods.CreateFloatingText("Not enough money", 2);
            return false;
        }
        return true;
    }





    bool CanAdjustInFreeCells(AtommInventory.Purchasable item, int quantity, bool availableInInventory)
    {

        bool canAdjust = false;
        if (availableInInventory && quantity == 0)
        {
            canAdjust = true;
            return canAdjust;
        }


        if (freeCells < 1)
        {
            return canAdjust;
        }
        else
        {
            int adjustableAmount = 0;
            if (item.consumableItem.stackableItem)
            {
                adjustableAmount = freeCells * AtommInventory.slotCount;
            }
            else
            {
                adjustableAmount = freeCells;
            }


            int consumedCells = Mathf.Abs(quantity / AtommInventory.slotCount);

            if (quantity % freeCells > 0)
            {
                consumedCells++;
            }
            

            if(freeCells < consumedCells)
            {
                return canAdjust;
            }

            
            if (quantity <= adjustableAmount)
            {
                freeCells -= consumedCells;
                canAdjust = true; 
            }

            return canAdjust;


           
        }


    }
}





