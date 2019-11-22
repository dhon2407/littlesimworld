using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class CookingStove : BreakableFurniture
{
    public GameObject FirstCookingCell;
    public GameObject SecondCookingCell;
    public GameObject ThirdCookingCell;
    public List<DragAndDropCell> CookingCells = new List<DragAndDropCell>();

    public GameObject FryingPan;
    public Item Water, Jelly, Chips, Meat, Bread, Burger, MeatStew, Eggs, Omlette, Croissant, Vegetable, Salad, Fish, 
        Fishfingers, FishAndChips, EggOnToast,VegeSandwich, FishOmelette, OmeletteWithHam, ChickenBreast,EggSalad, GourmetSandwich, EnglishBreakfast,RoastDinner,SaladSupreme;
    [Space]
    public float XpRewardForCookingWithNothing = 5;
    public float XpRewardForCookingFromOneIngredient = 10;
    public float XpRewardForCookingFromTwoIngredient = 20;
    [Space]
    public Transform WhereToInstantiateFood;

    public int RequiredLvlToCookFromOneCell = 3;
    public int RequiredLvlToCookFromTwoCells = 5;
    public int RequiredLvlToCookFromThreeCells = 7;


    public Canvas CookingOptionsCanvas;
    public Transform ZoneToStand;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void Update()
    {

        if (Vector2.Distance(transform.position, GameLibOfMethods.player.transform.position) > 1)
        {
            DisableChoices();
        }

        if (PlayerStatsManager.Cooking.Instance.Level >= RequiredLvlToCookFromOneCell)
        {
            FirstCookingCell.SetActive(true);
        }
        else if (FirstCookingCell.activeSelf)
        {
            FirstCookingCell.SetActive(false);
        }

        if (!SecondCookingCell.activeSelf && FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() && PlayerStatsManager.Cooking.Instance.Level >= RequiredLvlToCookFromTwoCells)
        {
            SecondCookingCell.SetActive(true);
        }
        else if (SecondCookingCell.activeSelf && !FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem())
        {
            SecondCookingCell.SetActive(false);
        }


        if (!ThirdCookingCell.activeSelf && SecondCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() && PlayerStatsManager.Cooking.Instance.Level >= RequiredLvlToCookFromThreeCells)
        {
            ThirdCookingCell.SetActive(true);
        }
        else if (ThirdCookingCell.activeSelf && !SecondCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem())
        {
            ThirdCookingCell.SetActive(false);
        }

        if(CookingOptionsCanvas.isActiveAndEnabled)
        foreach(DragAndDropCell cell in CookingCells)
        {
           
            if (cell.GetComponentInChildren<TextMeshProUGUI>())
            {
              TextMeshProUGUI  text = cell.GetComponentInChildren<TextMeshProUGUI>();
                text.text = "";
            }
          
        }
       // if(FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem())
        foreach(DragAndDropCell invCell in AtommInventory.DadCells)
        {
            invCell.GetComponent<Image>().color = Color.white;
            if (invCell.GetItem())
            {
                foreach (DragAndDropCell cookingCell in CookingCells)
                {
                    if (cookingCell.GetItem() &&
                        cookingCell.GetItem().GetComponent<ItemSlot>().ID == invCell.GetItem().GetComponent<ItemSlot>().ID)
                    {
                            invCell.GetComponent<Image>().color = Color.green;
                        break;
                    }
                    else
                    {
                        invCell.GetComponent<Image>().color = Color.white;
                    }
                } 
            }
        }
        for (int i = 0; i < CookingCells.Count; i++)
        {
        
                if(CookingCells[i].GetItem() && CookingCells[i].GetItem().GetComponent<ItemSlot>())
                {

                DragAndDropCell first = CookingCells.Where(obj => obj.GetItem() && obj.GetItem().GetComponent<ItemSlot>().ID == CookingCells[i].GetItem().GetComponent<ItemSlot>().ID).FirstOrDefault();
                DragAndDropCell second = CookingCells.Where(obj => obj.GetItem() && obj.GetItem().GetComponent<ItemSlot>().ID == CookingCells[i].GetItem().GetComponent<ItemSlot>().ID && obj != first).LastOrDefault();
                if(first && second && first.GetItem().GetComponent<ItemSlot>().ID == second.GetItem().GetComponent<ItemSlot>().ID)
                {
                    ClearCookingCells();
                }

            }
            
        }

        }
    public IEnumerator RefreshCells()
    {

        yield return new WaitForEndOfFrame();
        foreach (DragAndDropCell cell in CookingCells)
        {
            cell.UpdateMyItem();
        }
    }
    public void ActivateChoices()
    {
        if (!CookingOptionsCanvas.gameObject.activeSelf)
        {

            CookingOptionsCanvas.gameObject.SetActive(true);
            ClearCookingCells();
            AtommInventory.Instance.Showinventory();
        }



    }
    public void DisableChoices()
    {
        if (CookingOptionsCanvas.gameObject.activeSelf)
        {
            CookingOptionsCanvas.gameObject.SetActive(false);
            ClearCookingCells();
            AtommInventory.Instance.HideInventory();
            Debug.Log("Closed");
        }

    }
    public void Switch()
    {
        CookingOptionsCanvas.gameObject.SetActive(!CookingOptionsCanvas.gameObject.activeSelf);
        ClearCookingCells();
        //AtommInventory.Instance.Showinventory();
        Debug.Log("Switched");
    }

    // Update is called once per frame
    private void AddCookedItemToInventory(Item ingridient)
    {
        if (ingridient)
        {
            AtommInventory.Slot temp = AtommInventory.inventory.Where(obj => obj.itemName == ingridient.ItemName).FirstOrDefault();
            if (temp != null)
            {


                if (ingridient.CooksInto)
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + ingridient.CooksInto.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(temp);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                    PlayerStatsManager.Cooking.Instance.AddXP(XpRewardForCookingFromOneIngredient);
                }
                else
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + Jelly.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    PlayerStatsManager.Cooking.Instance.AddXP(XpRewardForCookingWithNothing);
                }





            }
            else
            {
                GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + Jelly.ItemName);
                GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                PlayerStatsManager.Cooking.Instance.AddXP(XpRewardForCookingWithNothing);
            }
        }
        ClearCookingCells();


    }
    public void CheckCookingCells()
    {
        if (!GameLibOfMethods.doingSomething && GameLibOfMethods.canInteract && !GameLibOfMethods.cantMove)
        {

        
        GameLibOfMethods.TempPos = GameLibOfMethods.player.transform.position;
        FirstCookingCell.GetComponentInChildren<DragAndDropCell>().UpdateMyItem();
        SecondCookingCell.GetComponentInChildren<DragAndDropCell>().UpdateMyItem();
        ThirdCookingCell.GetComponentInChildren<DragAndDropCell>().UpdateMyItem();

        if (FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() && SecondCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() && ThirdCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem())
        {
            GameLibOfMethods.doingSomething = true;
            StartCoroutine(InteractionChecker.Instance.WalkTo(ZoneToStand.position, CookFromThreeCells));
            
            return;
        }

        if (FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() && SecondCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem())
        {
            GameLibOfMethods.doingSomething = true;
            StartCoroutine(InteractionChecker.Instance.WalkTo(ZoneToStand.position, CookFromTwoCells));
            return;
        }
        if (FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() == null || FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem())
        {
            GameLibOfMethods.doingSomething = true;
            StartCoroutine( InteractionChecker.Instance.WalkTo(ZoneToStand.position, CookFromFirstCell));
            return;
        }
        }
    }
    private void AddCookedItemToInventory(Item firstIngridient,Item secondIngridient)
    {
        if (firstIngridient && secondIngridient)
        {
            AtommInventory.Slot firstItem = AtommInventory.inventory.Where(obj => obj.itemName == firstIngridient.ItemName).FirstOrDefault();
            AtommInventory.Slot secondItem = AtommInventory.inventory.Where(obj => obj.itemName == secondIngridient.ItemName).LastOrDefault();
            if (firstItem != null && secondItem != null)
            {

                List<Item> itemsInCells = new List<Item>();
                itemsInCells.Add(firstIngridient);
                itemsInCells.Add(secondIngridient);
                
                if (firstIngridient == Meat && secondIngridient == Meat)
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + MeatStew.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                     AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                   
                    AtommInventory.Refresh();
                }
                if (firstIngridient == Eggs && secondIngridient == Eggs)
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + Omlette.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (firstIngridient == Bread && secondIngridient == Bread)
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + Croissant.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (firstIngridient == Vegetable && secondIngridient == Vegetable)
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + Salad.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (firstIngridient == Fish && secondIngridient == Fish)
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + Fishfingers.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Bread) && itemsInCells.Contains(Meat))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + Burger.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Bread) && itemsInCells.Contains(Fish))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + FishAndChips.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Eggs) && itemsInCells.Contains(Bread))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + EggOnToast.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Bread) && itemsInCells.Contains(Vegetable))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + VegeSandwich.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Meat) && itemsInCells.Contains(Fish))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + MeatStew.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Fish) && itemsInCells.Contains(Eggs))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + FishOmelette.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Fish) && itemsInCells.Contains(Vegetable))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + FishAndChips.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Eggs) && itemsInCells.Contains(Meat))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + OmeletteWithHam.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Meat) && itemsInCells.Contains(Vegetable))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + ChickenBreast.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Eggs) && itemsInCells.Contains(Vegetable))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + EggSalad.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }


                PlayerStatsManager.Cooking.Instance.AddXP(XpRewardForCookingFromTwoIngredient);



            }
            else
            {
                GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + Jelly.ItemName);
                GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                PlayerStatsManager.Cooking.Instance.AddXP(XpRewardForCookingWithNothing);
            }
            ClearCookingCells();
        }


    }
    private void AddCookedItemToInventory(Item firstIngridient, Item secondIngridient, Item thirdIngredient)
    {
        if (firstIngridient && secondIngridient)
        {
            AtommInventory.Slot firstItem = AtommInventory.inventory.Where(obj => obj.itemName == firstIngridient.ItemName).FirstOrDefault();
            AtommInventory.Slot secondItem = AtommInventory.inventory.Where(obj => obj.itemName == secondIngridient.ItemName && obj != firstItem).LastOrDefault();
            AtommInventory.Slot thirdItem = AtommInventory.inventory.Where(obj => obj.itemName == thirdIngredient.ItemName && obj != secondItem && obj != firstItem).LastOrDefault();
            if (firstItem != null && secondItem != null && thirdItem != null)
            {

                List<Item> itemsInCells = new List<Item>();
                itemsInCells.Add(firstIngridient);
                itemsInCells.Add(secondIngridient);
                itemsInCells.Add(thirdIngredient);

              
                if (itemsInCells.Contains(Bread) && (itemsInCells.Contains(Meat) || itemsInCells.Contains(Fish)) && (itemsInCells.Contains(Meat) || itemsInCells.Contains(Fish)))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + GourmetSandwich.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.inventory.Remove(thirdItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Meat) && itemsInCells.Contains(Eggs) && itemsInCells.Contains(Bread))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + EnglishBreakfast.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.inventory.Remove(thirdItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Meat) && itemsInCells.Contains(Meat) && itemsInCells.Contains(Vegetable))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + RoastDinner.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.inventory.Remove(thirdItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }
                if (itemsInCells.Contains(Vegetable) && itemsInCells.Contains(Eggs) && itemsInCells.Contains(Bread))
                {
                    GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + SaladSupreme.ItemName);
                    GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                    AtommInventory.inventory.Remove(firstItem);
                    AtommInventory.inventory.Remove(secondItem);
                    AtommInventory.inventory.Remove(thirdItem);
                    AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                    AtommInventory.Refresh();
                }


                PlayerStatsManager.Cooking.Instance.AddXP(XpRewardForCookingFromTwoIngredient);



            }
            else
            {
                GameObject loadedItem = Resources.Load<GameObject>("Prefabs/" + Jelly.ItemName);
                GameObject item = Instantiate<GameObject>(loadedItem, WhereToInstantiateFood.transform.position, GameLibOfMethods.player.transform.rotation, null);
                AtommInventory.GatherItem(item.GetComponent<AtommItem>());
                PlayerStatsManager.Cooking.Instance.AddXP(XpRewardForCookingWithNothing);
            }
            ClearCookingCells();
        }


    }
    public void CookFromFirstCell()
    {
        FirstCookingCell.GetComponentInChildren<DragAndDropCell>().UpdateMyItem();
        SecondCookingCell.GetComponentInChildren<DragAndDropCell>().UpdateMyItem();
        if (FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() &&
            FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem().GetComponent<ItemSlot>().itemSO.CooksInto)
        {
            StartCoroutine(CookFromOne(FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem().GetComponent<ItemSlot>().itemSO));
            FirstCookingCell.GetComponentInChildren<DragAndDropCell>().RemoveItem();
        }
        else
        {
            if(!FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem())
            StartCoroutine(CookFromOne(null));
            else
                GameLibOfMethods.AddChatMessege("This item cannot be cooked!");

            ClearCookingCells();
        }
    }
    public void CookFromTwoCells()
    {
        FirstCookingCell.GetComponentInChildren<DragAndDropCell>().UpdateMyItem();
        SecondCookingCell.GetComponentInChildren<DragAndDropCell>().UpdateMyItem();
        if (FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() &&
            SecondCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() &&
            FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem().GetComponent<ItemSlot>().itemSO.CooksInto &&
            SecondCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem().GetComponent<ItemSlot>().itemSO.CooksInto)
        {
            StartCoroutine(CookFromTwo(FirstCookingCell.GetComponentInChildren<DragAndDropCell>(), SecondCookingCell.GetComponentInChildren<DragAndDropCell>()));
        }
        else
        {
            GameLibOfMethods.AddChatMessege("This item cannot be cooked!");
            ClearCookingCells();
        }
        }
    public void CookFromThreeCells()
    {
        FirstCookingCell.GetComponentInChildren<DragAndDropCell>().UpdateMyItem();
        SecondCookingCell.GetComponentInChildren<DragAndDropCell>().UpdateMyItem();
        ThirdCookingCell.GetComponentInChildren<DragAndDropCell>().UpdateMyItem();
        if (FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() && SecondCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() &&
            ThirdCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem() &&
            FirstCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem().GetComponent<ItemSlot>().itemSO.CooksInto &&
            SecondCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem().GetComponent<ItemSlot>().itemSO.CooksInto &&
            ThirdCookingCell.GetComponentInChildren<DragAndDropCell>().GetItem().GetComponent<ItemSlot>().itemSO.CooksInto)
        {
            StartCoroutine(CookFromThree(FirstCookingCell.GetComponentInChildren<DragAndDropCell>(), SecondCookingCell.GetComponentInChildren<DragAndDropCell>(), ThirdCookingCell.GetComponentInChildren<DragAndDropCell>()));
        }
        else
        {
            GameLibOfMethods.AddChatMessege("This item cannot be cooked!");
            ClearCookingCells();
        }

    }
   
    
   
    public IEnumerator CookFromOne(Item ingridient)
    {
        if (!GameLibOfMethods.doingSomething)
        {
            if (ingridient)
            {
                AtommInventory.Slot temp = AtommInventory.inventory.Where(obj => obj.itemName == ingridient.ItemName).FirstOrDefault();
                if (temp != null)
                {


                    if (ingridient.CooksInto)
                    {
                        GameLibOfMethods.canInteract = false;
                        GameLibOfMethods.cantMove = true;
                        GameLibOfMethods.Walking = true;
                        GameLibOfMethods.player.GetComponent<SpriteControler>().FaceUP();
                        GameLibOfMethods.lastInteractable = gameObject;



                        GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;







                        GameLibOfMethods.lastInteractable.GetComponent<CookingStove>().FryingPan.SetActive(false);
                        StartCoroutine(GameLibOfMethods.DoAction(delegate { AddCookedItemToInventory(ingridient); }, 10, false, false));
                        DayNightCycle.Instance.currentTimeSpeedMultiplier = 5;
                        GameLibOfMethods.animator.SetBool("Cooking", true);
                    }
                    else
                    {
                        GameLibOfMethods.AddChatMessege("This item cannot be cooked!");
                        ClearCookingCells();
                        yield break;
                    }





                }

                yield return new WaitForEndOfFrame();


                yield break;
            }
            else
            {
                GameLibOfMethods.canInteract = false;
                GameLibOfMethods.cantMove = true;
                GameLibOfMethods.Walking = true;
                GameLibOfMethods.player.GetComponent<SpriteControler>().FaceUP();
                GameLibOfMethods.lastInteractable = gameObject;



                GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;







                GameLibOfMethods.lastInteractable.GetComponent<CookingStove>().FryingPan.SetActive(false);
                StartCoroutine(GameLibOfMethods.DoAction(delegate { AddCookedItemToInventory(Jelly); }, 10, false, false));
                DayNightCycle.Instance.currentTimeSpeedMultiplier = 5;
                GameLibOfMethods.animator.SetBool("Cooking", true);
            }
            

            
        }



    }
    public IEnumerator CookFromTwo(DragAndDropCell firstCell, DragAndDropCell secondCell)
    {
        if (!GameLibOfMethods.doingSomething)
        {
            if (firstCell.GetItem() && secondCell.GetItem())
            {
                yield return new WaitForEndOfFrame();
                AtommInventory.Slot firstItem = AtommInventory.inventory.Where(obj => obj.ID == firstCell.GetItem().GetComponent<ItemSlot>().ID).FirstOrDefault();
                AtommInventory.Slot secondItem = AtommInventory.inventory.Where(obj => obj.ID == secondCell.GetItem().GetComponent<ItemSlot>().ID && obj != firstItem).LastOrDefault();
                if (firstItem != null && secondItem !=null)
                {


                   
                        GameLibOfMethods.canInteract = false;
                        GameLibOfMethods.cantMove = true;
                        GameLibOfMethods.Walking = true;
                        GameLibOfMethods.player.GetComponent<SpriteControler>().FaceUP();
                        GameLibOfMethods.lastInteractable = gameObject;



                        GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;







                        GameLibOfMethods.lastInteractable.GetComponent<CookingStove>().FryingPan.SetActive(false);
                        StartCoroutine(GameLibOfMethods.DoAction(delegate { AddCookedItemToInventory(firstCell.GetItem().GetComponent<ItemSlot>().itemSO,secondCell.GetItem().GetComponent<ItemSlot>().itemSO); }, 10, false, false));
                        DayNightCycle.Instance.currentTimeSpeedMultiplier = 5;
                        GameLibOfMethods.animator.SetBool("Cooking", true);
                    
                    





                }

                yield return new WaitForEndOfFrame();


                yield break;
            }
            else
            {
                GameLibOfMethods.canInteract = false;
                GameLibOfMethods.cantMove = true;
                GameLibOfMethods.Walking = true;
                GameLibOfMethods.player.GetComponent<SpriteControler>().FaceUP();
                GameLibOfMethods.lastInteractable = gameObject;



                GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;







                GameLibOfMethods.lastInteractable.GetComponent<CookingStove>().FryingPan.SetActive(false);
                StartCoroutine(GameLibOfMethods.DoAction(delegate { AddCookedItemToInventory(Jelly); }, 10, false, false));
                DayNightCycle.Instance.currentTimeSpeedMultiplier = 5;
                GameLibOfMethods.animator.SetBool("Cooking", true);
            }



        }



    }
    public IEnumerator CookFromThree(DragAndDropCell firstCell, DragAndDropCell secondCell, DragAndDropCell thirdCell)
    {
        if (!GameLibOfMethods.doingSomething)
        {
            if (firstCell.GetItem() && secondCell.GetItem() && thirdCell.GetItem())
            {
                yield return new WaitForEndOfFrame();
                AtommInventory.Slot firstItem = AtommInventory.inventory.Where(obj => obj.ID == firstCell.GetItem().GetComponent<ItemSlot>().ID).FirstOrDefault();
                AtommInventory.Slot secondItem = AtommInventory.inventory.Where(obj => obj.ID == secondCell.GetItem().GetComponent<ItemSlot>().ID && obj != firstItem).LastOrDefault();
                AtommInventory.Slot thirdItem = AtommInventory.inventory.Where(obj => obj.ID == thirdCell.GetItem().GetComponent<ItemSlot>().ID && obj != firstItem).LastOrDefault();
                if (firstItem != null && secondItem != null && thirdItem != null)
                {



                    GameLibOfMethods.canInteract = false;
                    GameLibOfMethods.cantMove = true;
                    GameLibOfMethods.Walking = true;
                    GameLibOfMethods.player.GetComponent<SpriteControler>().FaceUP();
                    GameLibOfMethods.lastInteractable = gameObject;



                    GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;







                    GameLibOfMethods.lastInteractable.GetComponent<CookingStove>().FryingPan.SetActive(false);
                    StartCoroutine(GameLibOfMethods.DoAction(delegate { AddCookedItemToInventory(firstCell.GetItem().GetComponent<ItemSlot>().itemSO,
                        secondCell.GetItem().GetComponent<ItemSlot>().itemSO,
                        thirdCell.GetItem().GetComponent<ItemSlot>().itemSO); }, 10, false, false));
                    DayNightCycle.Instance.currentTimeSpeedMultiplier = 5;
                    GameLibOfMethods.animator.SetBool("Cooking", true);







                }

                yield return new WaitForEndOfFrame();


                yield break;
            }
            else
            {
                GameLibOfMethods.canInteract = false;
                GameLibOfMethods.cantMove = true;
                GameLibOfMethods.Walking = true;
                GameLibOfMethods.player.GetComponent<SpriteControler>().FaceUP();
                GameLibOfMethods.lastInteractable = gameObject;



                GameLibOfMethods.player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;







                GameLibOfMethods.lastInteractable.GetComponent<CookingStove>().FryingPan.SetActive(false);
                StartCoroutine(GameLibOfMethods.DoAction(delegate { AddCookedItemToInventory(Jelly); }, 10, false, false));
                DayNightCycle.Instance.currentTimeSpeedMultiplier = 5;
                GameLibOfMethods.animator.SetBool("Cooking", true);
            }



        }



    }
    public void ClearCookingCells()
    {
        FirstCookingCell.GetComponentInChildren<DragAndDropCell>().RemoveItem();
        SecondCookingCell.GetComponentInChildren<DragAndDropCell>().RemoveItem();
        ThirdCookingCell.GetComponentInChildren<DragAndDropCell>().RemoveItem();
        AtommInventory.Refresh();
        foreach (DragAndDropCell invCell in AtommInventory.DadCells)
        {
                        invCell.GetComponent<Image>().color = Color.white;
        }
    }
    public void ClearCookingCellsIfYouAreNotDoingAnything()
    {
        if (!GameLibOfMethods.doingSomething && !GameLibOfMethods.cantMove)
        {
        FirstCookingCell.GetComponentInChildren<DragAndDropCell>().RemoveItem();
        SecondCookingCell.GetComponentInChildren<DragAndDropCell>().RemoveItem();
        ThirdCookingCell.GetComponentInChildren<DragAndDropCell>().RemoveItem();
        AtommInventory.Refresh();
            foreach (DragAndDropCell invCell in AtommInventory.DadCells)
            {
                invCell.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
