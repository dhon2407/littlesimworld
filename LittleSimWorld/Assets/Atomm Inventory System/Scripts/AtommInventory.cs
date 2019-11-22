using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CharacterStats;
using System.Linq;


[System.Serializable]
public class AtommInventory : MonoBehaviour
{
    public KeyCode action, interaction, skillsToggle;
    public AudioClip open, close, itemPickUp, itemDrop;
    public float PickUpRadius = 10;
    public Actions invActions;
    public GameObject statsUI;
    public InteractionChecker interactionChecker;
    public static int InventoryCapacity = 9;
    public GameObject latestItemContainer;
    public ItemTooltip slotTooltip;
    public static AtommInventory Instance;

    [SerializeField]
    public static List<Slot> inventory = new List<Slot>();
    public static List<DragAndDropItem> DaDItems = new List<DragAndDropItem>();
    public static List<DragAndDropCell> DadCells = new List<DragAndDropCell>();
    Transform documents, docView, canvas, containerItems, containerDocs, canvasContainer;
     public static GameObject purchasableItemSlot, inv, documentSlot, docViewer, container, erit, fx, ShopWindow, upgradeSlot, UpgradesShop;
    public static GameObject itemSlot;
    public GameObject currentContainer, currentUpgradesShop;
    public GameObject curentContainerWindowSource;
    public AtommContainer lastContainer;
    public static Transform items;
    public static DragAndDropCell lastEmptyCell;
    PlayerLook pl;
    CanvasScaler cs;

    public SpriteRenderer FoodInHandRenderer;

    public static int slotCount = 5;


    private void Awake()
    {
        if(!Instance)
        Instance = this;
    }

    private void Start()
    {
        Debug.Log(gameObject.name);

        canvas = GameObject.Find("Canvas").transform;
        inv = Instantiate(Resources.Load<GameObject>("Core/AIS/AtommInventory"), canvas);
        documents = inv.transform.Find("Docs");
        items = inv.transform.Find("Items");
        itemSlot = Resources.Load<GameObject>("Core/AIS/AtommSlot");
        purchasableItemSlot = Resources.Load<GameObject>("ShopUi/ShopItem");
        documentSlot = Resources.Load<GameObject>("Core/AIS/AtommDocument");
        docViewer = Resources.Load<GameObject>("Core/AIS/AtommViewer");
        container = Resources.Load<GameObject>("Core/AIS/AtommContainer");
        erit = Resources.Load<GameObject>("Core/AIS/ERIT");
        fx = Resources.Load<GameObject>("Core/AIS/_FX");
        ShopWindow = Resources.Load<GameObject>("ShopUi/ShoppingWindow2");
        upgradeSlot = Resources.Load<GameObject>("Core/AIS/UpgradeSlot");
        UpgradesShop = Resources.Load<GameObject>("Core/AIS/UpgradesShop");
       




        inv.SetActive(true);
        statsUI.SetActive(false);
        //slotTooltip = Instantiate(Resources.Load<GameObject>("Core/AIS/SlotTooltip"), canvas);
        //slotTooltip.SetActive(false);

        // cs = canvas.GetComponent<CanvasScaler>();
        //cs.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        // cs.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;

        // CHECKS IF IS TEST PLAYER 

        foreach (DragAndDropCell cell in AtommInventory.items.GetComponentsInChildren<DragAndDropCell>())
        {
            DadCells.Add(cell);
        }
        
        AtommInventory.Refresh();

    }
    

    private void Update()
    {
         
        
        if(InteractionChecker.Instance.lastHighlightedObject != curentContainerWindowSource)
        {
            DestroyWindows();
        }
        if (Input.GetKeyDown(action))
            ActionInventory();

        /*if (Input.GetKeyDown(interaction) && GameLibOfMethods.canInterract)
            CheckRaycast();*/

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach(DragAndDropCell cell in DadCells)
            {
                cell.UpdateMyItem();
            }
        }
        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (inventory[0].consumableItem != null)
            {
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(inventory[0].consumableItem.Consume, inventory[0].consumableItem.ConsuptionTime, inventory[0].consumableItem));

            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (inventory[1].consumableItem != null)
            {
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(inventory[1].consumableItem.Consume, inventory[1].consumableItem.ConsuptionTime, inventory[1].consumableItem));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (inventory[2].consumableItem != null)
            {
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(inventory[2].consumableItem.Consume, inventory[2].consumableItem.ConsuptionTime, inventory[2].consumableItem));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (inventory[3].consumableItem != null)
            {
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(inventory[3].consumableItem.Consume, inventory[3].consumableItem.ConsuptionTime, inventory[3].consumableItem));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (inventory[4].consumableItem != null)
            {
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(inventory[4].consumableItem.Consume, inventory[4].consumableItem.ConsuptionTime, inventory[4].consumableItem));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (inventory[5].consumableItem != null)
            {
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(inventory[5].consumableItem.Consume, inventory[5].consumableItem.ConsuptionTime, inventory[5].consumableItem));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (inventory[6].consumableItem != null)
            {
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(inventory[6].consumableItem.Consume, inventory[6].consumableItem.ConsuptionTime, inventory[6].consumableItem));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (inventory[7].consumableItem != null)
            {
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(inventory[7].consumableItem.Consume, inventory[7].consumableItem.ConsuptionTime, inventory[3].consumableItem));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (inventory[8].consumableItem != null)
            {
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(inventory[8].consumableItem.Consume, inventory[8].consumableItem.ConsuptionTime, inventory[8].consumableItem));
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (inventory[9].consumableItem != null)
            {
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(inventory[9].consumableItem.Consume, inventory[9].consumableItem.ConsuptionTime, inventory[9].consumableItem));
            }
        }*/
    }

    public void CheckRaycast()
    {
        //gameObject.layer = LayerMask.NameToLayer(LayerMask.LayerToName(2));


        GameObject interactable = GameLibOfMethods.CheckInteractable();


        if (!AtommInventory.Instance.currentContainer && !AtommInventory.Instance.currentUpgradesShop)
        {


            if (interactable.GetComponent<AtommItem>() != null)
                GatherItem(interactable.GetComponent<AtommItem>());
            else if (interactable.GetComponent<AtommContainer>() != null)
            {
                ContainerActive(interactable.GetComponent<AtommContainer>());
                lastContainer = interactable.GetComponent<AtommContainer>();
            }

            else if (interactable.GetComponent<Shop>() != null)
            {
                if (interactable.GetComponent<Shop>().ShopOptionsUI)
                {
                    interactable.GetComponent<Shop>().ShopOptionsUI.SetActive(true);
                }
                else
                {
                    ShopActive(interactable.GetComponent<Shop>());
                }

            }

            else if (interactable.GetComponent<UpgradesShop>() != null)
            {
                UpgradesShopActive(interactable.GetComponent<UpgradesShop>());
                interactable.GetComponent<UpgradesShop>().Refresh();
            }
        }
        else
        {
            DestroyWindows();
        }
        //gameObject.layer = LayerMask.NameToLayer(LayerMask.LayerToName(0));
        return;


    }

    public void ContainerActive (AtommContainer atommC)
    {
        curentContainerWindowSource = atommC.gameObject;
        ActionInventory();
        statsUI.SetActive(false);
        StatsTooltip.Instance.HideDescription();
        currentContainer = Instantiate(container, canvas);
        canvasContainer = currentContainer.transform;
        containerItems = currentContainer.transform.Find("_I");
        containerDocs = currentContainer.transform.Find("_D");

        foreach(Slot slot in atommC.slots)
        {
            GameObject button = Instantiate(itemSlot, containerItems);
            if (slot.quantity != 1)
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = slot.quantity.ToString("");
            else
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            button.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<GameObject>("Prefabs/" + slot.itemName).GetComponent<SpriteRenderer>().sprite;
            button.GetComponent<Button>().onClick.AddListener(delegate { GatherItem(slot, atommC); });

            var loadedItemSO = Resources.Load<GameObject>("Prefabs/" + slot.itemName).GetComponent<AtommItem>().itemSO;

            button.GetComponent<ItemSlot>().itemSO = loadedItemSO;
        }

        
    }
    public void ShopActive(Shop shop)
    {
        curentContainerWindowSource = shop.gameObject;
        ActionInventory();
        statsUI.SetActive(false);
        StatsTooltip.Instance.PopUpScript.CloseWindow();

        currentUpgradesShop = Instantiate(ShopWindow, canvas);
        canvasContainer = currentUpgradesShop.transform;
        containerItems = currentUpgradesShop.GetComponent<ShoppingUI>().shopItemsContent;
        // containerDocs = currentUpgradesShop.transform.Find("_D");
        
        int tmpCount = 0;    //***

        foreach (Purchasable purchasable in shop.purchasableItems)
        {
            //Purchasable DiscountedPurchasable = new Purchasable(purchasable);
            Purchasable DiscountedPurchasable = new Purchasable();
            
            DiscountedPurchasable.InitializePurchasable(purchasable);
            GameObject button = Instantiate(purchasableItemSlot, containerItems);


            // Changed Version
            if (DiscountedPurchasable.quantity != 1)
            {
                //button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = DiscountedPurchasable.price.ToString("") + "£";
            }
            else
            {
                //button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = DiscountedPurchasable.price.ToString("") + "£";
                //button.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<GameObject>("Prefabs/" + DiscountedPurchasable.itemName).GetComponent<SpriteRenderer>().sprite;
                //button.GetComponent<Button>().onClick.AddListener(delegate { PurchaseItem(DiscountedPurchasable, shop); });

                button.GetComponent<ItemSlot>().itemSO = purchasable.consumableItem;
                button.GetComponent<ShoppingItem>().DiscountedPurchasable = DiscountedPurchasable;
                button.GetComponent<ShoppingItem>().itemId = tmpCount;

                tmpCount++;
            }
        }
        /*foreach (Book document in shop.books)
        {
            GameObject button = Instantiate(documentSlot, containerDocs);
            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = document.documentName;
            button.GetComponent<Button>().onClick.AddListener(delegate { GatherDoc(document); });
        }*/
    }
    public void UpgradesShopActive(UpgradesShop shop)
    {
        curentContainerWindowSource = shop.gameObject;
        shop.Refresh();
        ActionInventory();
        statsUI.SetActive(false);
        StatsTooltip.Instance.PopUpScript.CloseWindow();
        
        currentUpgradesShop = Instantiate(UpgradesShop, canvas);
        canvasContainer = currentUpgradesShop.transform;
        containerItems = currentUpgradesShop.transform.Find("_I");
        containerDocs = currentUpgradesShop.transform.Find("_D");
        
        Debug.Log("open shop");
        
        foreach (Upgrade purchasable in shop.purchasableUpgrades)
        {
            if (purchasable.upgradesInto && GameObject.Find("Upgradable " + purchasable.PathCore).transform.GetChild(0).gameObject.name == purchasable.upgradesInto.name)
            {
                Debug.Log("removing purchasable");
                shop.purchasableUpgrades.Remove(purchasable);
                RefreshUpgradesShop(shop);
                return;
            }
         
            //Upgrade DiscountedPurchasable = new Upgrade(purchasable);
            Upgrade upgrade = new Upgrade();
           // upgrade.SetPlayerStatsManager(playerStatsManager);
            upgrade.InitializeUpgrade(purchasable);
            GameObject button = Instantiate(upgradeSlot, containerItems);
            /*button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = DiscountedPurchasable.price.ToString("") + "£";
            button.transform.Find("Image").GetComponent<Image>().sprite = DiscountedPurchasable.icon;
            button.GetComponent<Button>().onClick.AddListener(delegate { PurchaseUpgrade(DiscountedPurchasable, shop); });*/
            
            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = upgrade.price.ToString("") + "£";
            button.transform.Find("Image").GetComponent<Image>().sprite = upgrade.icon;
            button.GetComponent<Button>().onClick.AddListener(delegate { PurchaseUpgrade(upgrade, shop); });
            
            Debug.Log("Upgrade price purchasable: " + upgrade.price);
            Debug.Log("and upgradable GO: " + upgrade.upgradableGO.name);
        }

        /*foreach (Book document in shop.books)
        {
            GameObject button = Instantiate(documentSlot, containerDocs);
            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = document.documentName;
            button.GetComponent<Button>().onClick.AddListener(delegate { GatherDoc(document); });
        }*/
       
    }

    public void DestroyWindows()
    {
       
        if (canvasContainer != null)
            Destroy(canvasContainer.gameObject);
        if (currentUpgradesShop)
        {
            Destroy(currentUpgradesShop);
        }
        if (currentContainer)
        {
            Destroy(currentContainer);
        }
        curentContainerWindowSource = null;
    }

    public void ActionInventory()
    {

        if (inv.activeSelf)
        {
            HideInventory();
        }
        else
        {
            Showinventory();
        }
       
    }
    public void Showinventory()
    {
        if (canvasContainer != null)
            Destroy(canvasContainer.gameObject);

        Refresh();

        StartCoroutine(RefreshCells());
        //inv.SetActive(true);


        slotTooltip.HideTooltip();
        if (inv.activeSelf && docView == null && canvasContainer == null)
        {

            //statsUI.SetActive(true);
        }

        else
        {

            //statsUI.SetActive(false);
            StatsTooltip.Instance.PopUpScript.CloseWindow();
        }






        if (!inv.activeSelf)
        {

            //Cursor.lockState = CursorLockMode.Locked;
            SpawnFX(close);
        }
        else
        {

            //Cursor.lockState = CursorLockMode.None;
            SpawnFX(open);
        }
    }
    public void HideInventory()
    {
        if (canvasContainer != null)
            Destroy(canvasContainer.gameObject);

        Refresh();

        StartCoroutine(RefreshCells());
        //inv.SetActive(false);


        ItemTooltip.Instance.HideTooltip();
        if (inv.activeSelf && docView == null && canvasContainer == null)
        {

            //statsUI.SetActive(true);
        }

        else
        {

            //statsUI.SetActive(false);
            StatsTooltip.Instance.PopUpScript.CloseWindow();
        }






        if (!inv.activeSelf)
        {

            //Cursor.lockState = CursorLockMode.Locked;
            SpawnFX(close);
        }
        else
        {

            //Cursor.lockState = CursorLockMode.None;
            SpawnFX(open);
        }
    }
    public static IEnumerator RefreshCells()
    {

        yield return new WaitForEndOfFrame();
        foreach (DragAndDropCell cell in DadCells)
        {
            cell.UpdateMyItem();
        }
    }

    public void Freeze(bool o)
    {
        
        Refresh();

        if (!o)
        {
           // Cursor.visible = false;
           // Cursor.lockState = CursorLockMode.Locked;
            SpawnFX(close);
        }
        else
        {
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
            SpawnFX(open);
        }
    }
    public void GatherItem(Slot item, AtommContainer atommC)
    {
        if (InventoryCapacity == inventory.Count)
        {
            GameLibOfMethods.CreateFloatingText("Not enough inventory space!", 2);
            return;
        }
        var loadedItemSO = Resources.Load<GameObject>("Prefabs/" + item.itemName).GetComponent<AtommItem>().itemSO;
        var temp = inventory.Where
            (obj => obj.itemName == item.itemName && obj.quantity < loadedItemSO.maxAmount).FirstOrDefault();


        if (inventory.Contains(temp))
        {
            if (temp.quantity + item.quantity <= loadedItemSO.maxAmount)
                temp.quantity += item.quantity;
            else
            {
                item.quantity = temp.quantity + item.quantity - loadedItemSO.maxAmount;
                temp.quantity = loadedItemSO.maxAmount;
                Slot tempSlot = new Slot(item);
                inventory.Add(tempSlot);

                for (int i = 0; i < DadCells.Count(); i++)    //iterate through cells of inventory
                {
                    if (DadCells[i].GetItem() == null)  // check for cell that is empty
                    {
                        lastEmptyCell = DadCells[i];   //assign empty cell as temporary DaD variable
                        tempSlot.positionOnActionBar = i;
                        Debug.Log("This item's position on action bar will be " + i);
                        break;                    //stop iteration so no more empty cells assigns as temporary DaD variable
                    }
                }

            }

        }
        else
        {
            Slot tempSlot = new Slot(item);
            inventory.Add(tempSlot);
            foreach (DragAndDropCell cell in DadCells)
            {
                cell.UpdateMyItem();
                if (!cell.GetItem())  // check for cell that is empty
                {
                    tempSlot.positionOnActionBar = cell.transform.GetSiblingIndex();
                    Debug.Log("This item's position on action bar will be " + cell.transform.GetSiblingIndex());
                    break;                    //stop iteration so no more empty cells assigns as temporary DaD variable
                }
            }
        }

        atommC.slots.Remove(item);
        Refresh();
        RefreshContainer(atommC);
    }
    public static void GatherItem(AtommItem item)
    {
        if (AtommInventory.InventoryCapacity == inventory.Count)
        {
            GameLibOfMethods.CreateFloatingText("Not enough inventory space!", 2);
            return;
        }
        var loadedItemSO = Resources.Load<GameObject>("Prefabs/" + item.itemSO.ItemName).GetComponent<AtommItem>().itemSO;
        var temp = inventory.Where
            (obj => obj.itemName == item.itemSO.ItemName && obj.quantity < loadedItemSO.maxAmount).FirstOrDefault();


        if (inventory.Contains(temp))
        {
            
            if (temp.quantity + item.quantity <= item.itemSO.maxAmount)
                temp.quantity += item.quantity;
            else
            {
                
                item.quantity = temp.quantity + item.quantity - item.itemSO.maxAmount;
                temp.quantity = loadedItemSO.maxAmount;
                Slot tempSlot = new Slot(item);
                inventory.Add(tempSlot);
                
                for (int i = 0; i < DadCells.Count(); i++)    //iterate through cells of inventory
                {
                    if (DadCells[i].GetItem() == null)  // check for cell that is empty
                    {
                        lastEmptyCell = DadCells[i];   //assign empty cell as temporary DaD variable
                        tempSlot.positionOnActionBar = i;
                        Debug.Log("There is same item in inventory, creating new item in empty slot. This item's position on action bar will be " + i);
                        break;                    //stop iteration so no more empty cells assigns as temporary DaD variable
                    }
                }

            }

        }
        else
        {

            Slot tempSlot = new Slot(item);
            inventory.Add(tempSlot);
            foreach(DragAndDropCell cell in DadCells)
            {
                cell.UpdateMyItem();
                if (!cell.GetItem())  // check for cell that is empty
                {
                    tempSlot.positionOnActionBar = cell.transform.GetSiblingIndex();
                    Debug.Log("This item's position on action bar will be " + cell.transform.GetSiblingIndex());
                    break;                    //stop iteration so no more empty cells assigns as temporary DaD variable
                }
            }
               

        }

        AtommInventory.Instance.SpawnFX(AtommInventory.Instance.itemPickUp);
        Destroy(item.gameObject);
        Refresh();
    }


    // Custom One For Now
    public void BuyItem(Purchasable item, bool isInInventory)//***
    {
        int q = item.quantity;

        PlayerStatsManager.Instance.Money -= item.quantity * item.price; //***

        PlayerStatsManager.Charisma.Instance.AddXP(item.price * 0.2f);

        

        if (isInInventory) // Item is available in inventory and some slot has capacity to hold more items
        {

            foreach (Slot slot in inventory)
            {

                foreach (DragAndDropCell cell in DadCells)
                {
                    string tempName = slot.itemName;
                    int temp = slot.quantity;
                    if (cell.myDadItem &&
                       cell.myDadItem.gameObject.GetComponent<ItemSlot>().ID == slot.ID &&
                       tempName == item.itemName && slot.quantity < slotCount && !cell.cellType.Equals(DragAndDropCell.CellType.TrashBin))
                    {

                        int freeSpace = slotCount - temp;

                        if (freeSpace >= q)
                        {
                            slot.quantity += q;
                            q = 0;

                            // Refresh Data
                            Refresh();
                            //Debug.LogError("Found My Desired Item  Finally");
                            // Return
                            return;
                        }
                        else
                        {
                            slot.quantity += freeSpace;
                            q -= freeSpace;

                        }
                        Refresh();

                        Debug.LogError("Found My Desired Item But Needs To Add New Cells");
                        break;
                    }

                }
            }
          
        }

        while (q > 0)
        {
            //Debug.LogError(item.quantity);

            //Purchasable t = new Purchasable(item);
            Purchasable t = new Purchasable();
          
            t.InitializePurchasable(item);
            if (item.consumableItem.stackableItem)
            {
                if (q > slotCount)
                {
                    t.quantity = slotCount;
                    q -= t.quantity;
                }
                else
                {
                    // Debug.LogError("here now!!!!");
                    t.quantity = q;
                    q = 0;
                }
            }
            else
            {
                t.quantity = 1;
                q -= t.quantity;
            }

            // Add New Slot in inventory
            Slot tempSlot = new Slot(t);
            inventory.Add(tempSlot);
            //Find Free Cell In Inventory

            // Fill its capacity


            foreach (DragAndDropCell cell in DadCells)
            {
                cell.UpdateMyItem();
                if (!cell.GetItem() && !cell.cellType.Equals(DragAndDropCell.CellType.TrashBin))  // check for cell that is empty
                {
                    tempSlot.positionOnActionBar = cell.transform.GetSiblingIndex();
                   // Debug.Log("This item's position on action bar will be " + cell.transform.GetSiblingIndex());
                    break;                    //stop iteration so no more empty cells assigns as temporary DaD variable
                }
            }

            Refresh();

            // Reduce items from remaining quantity

            // Repeat if quantity remains
        }


    }



    private void PurchaseItem(Purchasable item, Shop shop)
    {
        if (inventory.Count == InventoryCapacity || item.price > PlayerStatsManager.Instance.Money)
        {
            GameLibOfMethods.CreateFloatingText("Not enough money OR inventory space!",2);
            return;
        }
        PlayerStatsManager.Instance.Money -= item.price;
        //inventory.Add(new Slot(item));

        PlayerStatsManager.Charisma.Instance.AddXP(item.price * 0.2f);



        if (InventoryCapacity == inventory.Count)
        {
            GameLibOfMethods.CreateFloatingText("Not enough inventory space!", 2);
            return;
        }
        var loadedItemSO = Resources.Load<GameObject>("Prefabs/" + item.itemName).GetComponent<AtommItem>().itemSO;
        var temp = inventory.Where
            (obj => obj.itemName == item.itemName && obj.quantity < loadedItemSO.maxAmount).FirstOrDefault();


        if (inventory.Contains(temp))
        {
            if (temp.quantity + item.quantity <= item.consumableItem.maxAmount)
                temp.quantity += item.quantity;
            else
            {
                item.quantity = temp.quantity + item.quantity - item.consumableItem.maxAmount;
                temp.quantity = loadedItemSO.maxAmount;
                Slot tempSlot = new Slot(item);
                inventory.Add(tempSlot);

                for (int i = 0; i < DadCells.Count(); i++)    //iterate through cells of inventory
                {
                    if (DadCells[i].GetItem() == null)  // check for cell that is empty
                    {
                        lastEmptyCell = DadCells[i];   //assign empty cell as temporary DaD variable
                        tempSlot.positionOnActionBar = i;
                        Debug.Log("This item's position on action bar will be " + i);
                        break;                    //stop iteration so no more empty cells assigns as temporary DaD variable
                    }
                }

            }

        }
        else
        {
            Slot tempSlot = new Slot(item);
            inventory.Add(tempSlot);
            foreach (DragAndDropCell cell in DadCells)
            {
                cell.UpdateMyItem();
                if (!cell.GetItem())  // check for cell that is empty
                {
                    tempSlot.positionOnActionBar = cell.transform.GetSiblingIndex();
                    Debug.Log("This item's position on action bar will be " + cell.transform.GetSiblingIndex());
                    break;                    //stop iteration so no more empty cells assigns as temporary DaD variable
                }
            }
        }

        Refresh();

    }

    private void PurchaseUpgrade(Upgrade item, UpgradesShop shop)
    {
        if (item.price > PlayerStatsManager.Instance.Money)
        {
            GameLibOfMethods.CreateFloatingText("Not enough money!", 2);
            return;
        }
        PlayerStatsManager.Instance.Money -= item.price;

        PlayerStatsManager.Charisma.Instance.AddXP(item.price * 0.2f);

        //int currentUpgradeLevel = int.Parse(item.upgradableGO.transform.GetChild(0).name);
        int currentUpgradeLevel = item.shopUpgradeID;
        if (currentUpgradeLevel == 0)
            currentUpgradeLevel = 1;

       
            GameObject upgradedGO = Instantiate(item.upgradesInto, item.upgradableGO.transform);
        upgradedGO.name = upgradedGO.name.Replace("(Clone)", "");
        upgradedGO.transform.position = item.upgradableGO.transform.position;


        if (item.nextUpgrade != null)
        {

           // shop.UpgradableObjects.Add(item.nextUpgrade);
            
           // shop.UpgradableObjects[shop.UpgradableObjects.IndexOf(item.nextUpgrade)].upgradableGO = item.upgradableGO;
            
            shop.GetShopUpgradesList().Add(item.shopUpgradeScriptableObject.nextShopUpgradeSO);
            
            shop.GetShopUpgradesList().Remove(item.shopUpgradeScriptableObject);
            
        }

            if(item.upgradableGO.transform.GetChild(0))
        Destroy(item.upgradableGO.transform.GetChild(0).gameObject);

        //UpgradesShop.ShopUpgrades temp = shop.UpgradableObjects.Where(obj => obj.upgradesInto = item.upgradesInto).FirstOrDefault();
       // if (temp != null)
        //{
          //  shop.UpgradableObjects.Remove(temp);
      //  }
                
        shop.Refresh();
        RefreshUpgradesShop(shop);
        DestroyWindows();

    }



    

    public void PutItemInContainer(Slot item, AtommContainer atommC)
    {
        if (atommC.slots.Count == atommC.Capacity)
        {
            GameLibOfMethods.CreateFloatingText("Not enough container space!", 2);
            return;
        }
        var loadedItemSO = Resources.Load<GameObject>("Prefabs/" + item.itemName).GetComponent<AtommItem>().itemSO;
        var temp = atommC.slots.Where
            (obj => obj.itemName == item.itemName && obj.quantity < Resources.Load<GameObject>("Prefabs/" + obj.itemName).GetComponent<AtommItem>().itemSO.maxAmount).FirstOrDefault();


        if (atommC.slots.Contains(temp))
        {
            if (temp.quantity + item.quantity <= loadedItemSO.maxAmount)
                temp.quantity += item.quantity;
            else
            {
                item.quantity = temp.quantity + item.quantity - loadedItemSO.maxAmount;
                temp.quantity = loadedItemSO.maxAmount;
                atommC.slots.Add(item);
                
            }
               
        }
        else
        {
            atommC.slots.Add(item);
        }
        
        inventory.Remove(item);
        Refresh();
        RefreshContainer(atommC);
    }


    public void RefreshContainer (AtommContainer atommC)
    {

        curentContainerWindowSource = atommC.gameObject;
        foreach (Transform child in containerItems)
            Destroy(child.gameObject);


       

        foreach (Slot slot in atommC.slots)
        {
            GameObject button = Instantiate(itemSlot, containerItems);
            if (slot.quantity != 1)
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = slot.quantity.ToString("");
            else
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            button.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<GameObject>("Prefabs/" + slot.itemName).GetComponent<SpriteRenderer>().sprite;
            button.GetComponent<Button>().onClick.AddListener(delegate { GatherItem(slot, atommC); });
            button.GetComponent<ItemSlot>().itemSO = Resources.Load<GameObject>("Prefabs/" + slot.itemName).GetComponent<AtommItem>().itemSO;
        }
        /*
        atommC.slots.Clear();

        for (int i = 0; i < containerItems.childCount; i++)
        {
            var temp = containerItems.GetChild(i).gameObject.GetComponent<ItemSlot>().consumableItem.ItemName;
            //AtommInventory.Slot slot = new AtommInventory.Slot(newItem.GetComponent<AtommItem>());
            atommC.slots.Add(new Slot(Resources.Load<GameObject>("Prefabs/" + temp).GetComponent<AtommItem>()));
        }

        foreach (Transform child in containerItems)
            Destroy(child.gameObject);

        foreach (Slot slot in atommC.slots)
        {
            GameObject button = Instantiate(itemSlot, containerItems);
            if (slot.quantity != 1)
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = slot.quantity.ToString("");
            else
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            button.transform.Find("Image").GetComponent<Image>().sprite = slot.icon;
            button.GetComponent<Button>().onClick.AddListener(delegate { GatherItem(slot, atommC); });
            button.GetComponent<ItemSlot>().consumableItem = slot.consumableItem;

        }*/

    }
    public void RefreshShop(Shop shop)
    {

        foreach (Transform child in containerItems)
            Destroy(child.gameObject);

        foreach (Transform child in containerDocs)
            Destroy(child.gameObject);

        foreach (Purchasable purchasableSlot in shop.purchasableItems)
        {
            //Purchasable DiscountedPurchasable = new Purchasable(purchasableSlot);
            Purchasable DiscountedPurchasable = new Purchasable();
            
            DiscountedPurchasable.InitializePurchasable(purchasableSlot);
            GameObject button = Instantiate(itemSlot, containerItems);
            if (DiscountedPurchasable.quantity != 1)
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = DiscountedPurchasable.price.ToString() + "£";
            else
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = DiscountedPurchasable.price.ToString() + "£";
            button.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<GameObject>("Prefabs/" + DiscountedPurchasable.itemName).GetComponent<SpriteRenderer>().sprite;
            button.GetComponent<Button>().onClick.AddListener(delegate { PurchaseItem(DiscountedPurchasable, shop); });
            button.GetComponent<ItemSlot>().itemSO = purchasableSlot.consumableItem;

        }

        /*foreach (Book document in shop.documents)
        {
            GameObject button = Instantiate(documentSlot, containerDocs);
            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = document.documentName;
            button.GetComponent<Button>().onClick.AddListener(delegate { GatherDoc(document, shop); });
        }*/
    }
    public void RefreshUpgradesShop(UpgradesShop shop)
    {

        foreach (Transform child in containerItems)
            Destroy(child.gameObject);

        foreach (Transform child in containerDocs)
            Destroy(child.gameObject);

        foreach (Upgrade upgrade in shop.purchasableUpgrades)
        {
            if(GameObject.Find("Upgradable Bed").transform.GetChild(0).gameObject.name == upgrade.upgradesInto.name)
            {
                shop.purchasableUpgrades.Remove(upgrade);
                RefreshUpgradesShop(shop);
                return;
            }
            //Upgrade DiscountedPurchasable = new Upgrade(upgrade);
            Upgrade myUpgrade = new Upgrade();
           // myUpgrade.SetPlayerStatsManager(playerStatsManager);
            myUpgrade.InitializeUpgrade(upgrade);
            GameObject button = Instantiate(itemSlot, containerItems);
            /*button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = DiscountedPurchasable.price.ToString() + "£";
            button.transform.Find("Image").GetComponent<Image>().sprite = DiscountedPurchasable.icon;
            button.GetComponent<Button>().onClick.AddListener(delegate { PurchaseUpgrade(DiscountedPurchasable, shop); });*/
            
            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = myUpgrade.price.ToString() + "£";
            button.transform.Find("Image").GetComponent<Image>().sprite = myUpgrade.icon;
            button.GetComponent<Button>().onClick.AddListener(delegate { PurchaseUpgrade(myUpgrade, shop); });

        }

        /*foreach (Book document in shop.documents)
        {
            GameObject button = Instantiate(documentSlot, containerDocs);
            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = document.documentName;
            button.GetComponent<Button>().onClick.AddListener(delegate { GatherDoc(document, shop); });
        }*/
    }

    public static void Refresh()
    {

        foreach (DragAndDropCell cell in DadCells)
        {
            cell.UpdateMyItem();
        }

        foreach (Slot slot in inventory)
        {

            foreach (DragAndDropCell cell in DadCells)
            {
                string temp = slot.quantity.ToString();
                
                if (temp == "1")
                    temp = "";
                if (cell.myDadItem &&
                   cell.myDadItem.gameObject.GetComponent<ItemSlot>().ID == slot.ID)
                {
                    slot.positionOnActionBar = cell.transform.GetSiblingIndex();
                    Debug.Log("Updated item's position on action bar to  " + slot.positionOnActionBar + ". Its ID is: " + slot.ID);
                    break;
                }

            }
        }

        foreach (DragAndDropCell cell in DadCells)
        {
            cell.UpdateMyItem();
            cell.RemoveItem();
            foreach(Transform child in cell.transform)
            {
                Destroy(child.gameObject);
            }
            cell.UpdateMyItem();
        }



        foreach (Slot slot in inventory)
        {
            
            

            GameObject button = Instantiate(itemSlot, GameLibOfMethods.player.transform);     
               
            if (slot.quantity != 1)
            {
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = slot.quantity.ToString("");
            }

            else
            {
                button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "";
            }
            button.GetComponent<ItemSlot>().ID = slot.ID;
            button.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load<GameObject>("Prefabs/" + slot.itemName).GetComponent<SpriteRenderer>().sprite;
            button.name = slot.itemName;
            button.GetComponent<ItemSlot>().itemSO = Resources.Load<GameObject>("Prefabs/" + slot.itemName).GetComponent<AtommItem>().itemSO;
            button.GetComponent<Button>().onClick.AddListener(delegate { Action(slot); });
            
            DadCells[slot.positionOnActionBar].AddItem(button.GetComponent<DragAndDropItem>());

        }

        StaticCoroutine.Start(RefreshCells());


    }

    public static void Action(Slot slot)
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            DropItem(slot);
        }
        else if(!GameLibOfMethods.animator.GetBool("Walking"))
        {

            //interactionChecker.CheckInteractableType(slot.itemName);
            Item tempCons = Resources.Load<GameObject>("Prefabs/" + slot.itemName).GetComponent<AtommItem>().itemSO;
            
            if (tempCons != null)
            {
                
                GameLibOfMethods.StaticCoroutine.Start(GameLibOfMethods.DoAction(delegate { tempCons.Use(slot.ID); }, tempCons.UsageTime, tempCons, tempCons.AnimationToPlayName));
                
                
               
            }



            /*if (slot.Consumable)
                inventory.Remove(slot);
                */
        }

            
       
        Refresh();
    }
    public static void DropItem(Slot slot)
    {
        if (Resources.Load<GameObject>("Prefabs/" + slot.itemName) == null)
        {
            GameObject newItem = Instantiate(erit, GameLibOfMethods.player.transform.position + (GameLibOfMethods.facingDir * 0.2f), GameLibOfMethods.player.transform.rotation);
            newItem.GetComponent<AtommItem>().quantity = slot.quantity;
            inventory.Remove(slot);
        }
        else
        {
            GameObject newItem = Instantiate(Resources.Load<GameObject>("Prefabs/" + slot.itemName), GameLibOfMethods.player.transform.position + (GameLibOfMethods.facingDir * 0.2f), GameLibOfMethods.player.transform.rotation);
            newItem.name = slot.itemName;
            newItem.GetComponent<AtommItem>().quantity = slot.quantity;
            inventory.Remove(slot);
        }
        
        Refresh();
        AtommInventory.Instance.SpawnFX(AtommInventory.Instance.itemDrop);
    }

  
    

    public void SpawnFX (AudioClip clip)
    {
        if (clip == null)
            return;
        GameObject newFX = Instantiate(fx, this.transform);
        newFX.GetComponent<AudioSource>().clip = clip;
        newFX.GetComponent<AudioSource>().Play();
        Destroy(newFX, clip.length);
    }
    public void SpawnFX(AudioClip clip, float startingVolume)
    {
        if (clip == null)
            return;
        GameObject newFX = Instantiate(fx, this.transform);
        newFX.GetComponent<AudioSource>().clip = clip;
        newFX.GetComponent<AudioSource>().Play();
        Destroy(newFX, clip.length);
    }

    public bool LookFor(string item)
    {
        foreach (Slot slot in inventory)
            if (slot.itemName == item)
                return true;

        return false;
    }

    public bool LookFor(string item, int quantity)
    {
        foreach (Slot slot in inventory)
            if (slot.itemName == item)
                return true;

        return false;
    }

    public static bool LookForAndRemove(float itemID)
    {
        foreach (Slot slot in inventory)
            if (slot.ID == itemID)
            { inventory.Remove(slot); return true; }

        return false;
    }

    public static bool LookForAndRemove(string item, int quantity)
    {
        foreach (Slot slot in inventory)
        {
            if (slot.itemName == item && slot.quantity >= quantity)
            {
                slot.quantity -= quantity;
                if (slot.quantity <= 0)
                    inventory.Remove(slot);
                return true;
            }
        }
        return false;
    }

    [System.Serializable]
    public class Slot
    {
        [SerializeField]
        public string itemName;
        [SerializeField]
        public int quantity;
        [SerializeField]
        public string iconPath;
       // public ConsumableItem consumableItem;
        /*[SerializeField]
        public ItemSO justItem;*/
        [SerializeField]
        public int positionOnActionBar;

        [SerializeField]
        public float ID =   Random.Range(1000, 10000000);
        

        [SerializeField]
        public Slot(AtommItem item)
        {

            this.itemName = item.itemSO.ItemName;
            this.quantity = item.quantity;
            this.iconPath = item.iconPath;
            this.ID = Random.Range(1000, 10000000);
           // this.consumableItem = item.consumableItem;
          //  this.justItem = item.justItem;

        }
        [SerializeField]
        public Slot(Purchasable item)
        {
            this.itemName = item.itemName;
            this.quantity = item.quantity;
            this.ID = Random.Range(1000, 10000000);
            //   this.consumableItem = item.consumableItem;
        }
        [SerializeField]
        public Slot(ConsumableItem consumableSO)
        {
         //   this.consumableItem = consumableSO;
            this.itemName = consumableSO.ItemName;
            this.quantity = consumableSO.sellingQuantity;
            this.iconPath = consumableSO.iconPath;
            this.ID = Random.Range(1000, 10000000);
        }
        public Slot(Slot slot)
        {
            //   this.consumableItem = consumableSO;
            this.itemName = slot.itemName;
            this.quantity = slot.quantity;
            this.iconPath = slot.iconPath;
            this.positionOnActionBar = slot.positionOnActionBar;
            this.ID = slot.ID;
        }


   
    }

   
    [System.Serializable]
    public class Purchasable
    {
        public string itemName;
        public int quantity;
        public int price;
        public Item consumableItem;
        
        public Purchasable()
        {
            
        }
        
       /* public Purchasable(Purchasable item)
        {
            this.consumableItem = item.consumableItem;
            this.itemName = item.itemName;
            this.quantity = item.quantity;
            this.price = Mathf.RoundToInt(item.price * PlayerStatsManager.Instance.PriceMultiplier);
            

        }
        public Purchasable(Item consumableSO)
        {
            this.consumableItem = consumableSO;
            this.itemName = consumableSO.ItemName;
            this.price = Mathf.RoundToInt(consumableSO.price * PlayerStatsManager.Instance.PriceMultiplier);
            this.quantity = consumableSO.sellingQuantity;
        }*/

       
        public void InitializePurchasable(Item consumableSO)
        {
            this.consumableItem = consumableSO;
            this.itemName = consumableSO.ItemName;
            //this.price = Mathf.RoundToInt(consumableSO.price * PlayerStatsManager.Instance.PriceMultiplier);
            this.price = Mathf.RoundToInt(consumableSO.price * PlayerStatsManager.Instance.PriceMultiplier);
            this.quantity = consumableSO.sellingQuantity;
        }

        public void InitializePurchasable(Purchasable item)
        {
            this.consumableItem = item.consumableItem;
            this.itemName = item.itemName;
            this.quantity = item.quantity;
            //this.price = Mathf.RoundToInt(item.price * PlayerStatsManager.Instance.Instance.PriceMultiplier);
            this.price = Mathf.RoundToInt(item.price * PlayerStatsManager.Instance.PriceMultiplier);
        }

    }
    public class Upgrade
    {
        public GameObject upgradableGO;
        public GameObject upgradesInto;
        public int price;
        public Sprite icon;
        public UpgradesShop.ShopUpgrades nextUpgrade;
        public ShopUpgradeScriptableObject shopUpgradeScriptableObject;
        public string PathCore;
        public int shopUpgradeID;

        /*public Upgrade(Upgrade upgrade)
        {
            this.upgradableGO = upgrade.upgradableGO;
            this.upgradesInto = upgrade.upgradesInto;
            this.price = Mathf.RoundToInt(upgrade.price * PlayerStatsManager.Instance.PriceMultiplier);
            this.icon = upgrade.icon;
            if(upgrade.nextUpgrade != null)
            this.nextUpgrade = upgrade.nextUpgrade;
            this.PathCore = upgrade.PathCore;
        }*/

        public Upgrade()
        {
            
        }
        
        /*public Upgrade(ShopUpgrades upgrade)
        {
            this.upgradableGO = upgrade.upgradableGO;
            this.upgradesInto = upgrade.upgradesInto;
            this.price = upgrade.price;
            this.icon = upgrade.icon;
            if(upgrade.nextUpgrade != null)
                this.nextUpgrade = upgrade.nextUpgrade;
            this.PathCore = upgrade.PathCore;
        }*/

      
        
        public void InitializeUpgrade(Upgrade upgrade)
        {
            this.upgradableGO = upgrade.upgradableGO;
            this.upgradesInto = upgrade.upgradesInto;
            this.shopUpgradeScriptableObject = upgrade.shopUpgradeScriptableObject;
            //this.price = Mathf.RoundToInt(upgrade.price * PlayerStatsManager.Instance.PriceMultiplier);
            this.price = Mathf.RoundToInt(upgrade.price * PlayerStatsManager.Instance.PriceMultiplier);
            this.icon = upgrade.icon;
            if(upgrade.nextUpgrade != null)
                this.nextUpgrade = upgrade.nextUpgrade;
            this.PathCore = upgrade.PathCore;
        }

        public void InitializeUpgrade(UpgradesShop.ShopUpgrades upgrade)
        {
            this.upgradableGO = upgrade.upgradableGO;
            this.upgradesInto = upgrade.upgradesInto;
            this.price = upgrade.price;
            this.icon = upgrade.icon;
            this.shopUpgradeScriptableObject = upgrade.currentUpgradeSO;
            if(upgrade.nextUpgrade != null)
                this.nextUpgrade = upgrade.nextUpgrade;
            this.PathCore = upgrade.PathCore;
            shopUpgradeID = upgrade.upgradeLevel;
        }
        public void InitializeUpgrade(ShopUpgradeScriptableObject upgrade)
        {
            
            this.upgradesInto = upgrade.upgradesInto;
            this.price = upgrade.price;
            this.icon = upgrade.icon;
            this.shopUpgradeScriptableObject = upgrade;
            if (upgrade.nextShopUpgrade != null)
                this.nextUpgrade = upgrade.nextShopUpgrade;
            this.PathCore = upgrade.PathCore;
            shopUpgradeID = upgrade.upgradeLevel;
            this.upgradableGO = GameObject.Find("Upgradable " + this.PathCore);
        }

        /*public Upgrade(ShopUpgradeScriptableObject upgrade)
        {
            this.upgradableGO = upgrade.gameObjectGoingToBeUpgraded;
            this.upgradesInto = upgrade.gameObjectIntoWhichItUpgrades;
            this.price = upgrade.price;
            this.icon = upgrade.icon;
            if(upgrade.nextShopUpgrade != null)
                this.nextUpgrade = upgrade.nextShopUpgrade;
            this.PathCore = upgrade.pathToUpgradesFolder;
        }*/

    }
    public class StaticCoroutine : MonoBehaviour
    {
        private static StaticCoroutine m_instance;

        // OnDestroy is called when the MonoBehaviour will be destroyed.
        // Coroutines are not stopped when a MonoBehaviour is disabled, but only when it is definitely destroyed.
        private void OnDestroy()
        { m_instance.StopAllCoroutines(); }

        // OnApplicationQuit is called on all game objects before the application is closed.
        // In the editor it is called when the user stops playmode.
        private void OnApplicationQuit()
        { m_instance.StopAllCoroutines(); }

        // Build will attempt to retrieve the class-wide instance, returning it when available.
        // If no instance exists, attempt to find another StaticCoroutine that exists.
        // If no StaticCoroutines are present, create a dedicated StaticCoroutine object.
        private static StaticCoroutine Build()
        {
            if (m_instance != null)
            { return m_instance; }

            m_instance = (StaticCoroutine)FindObjectOfType(typeof(StaticCoroutine));

            if (m_instance != null)
            { return m_instance; }

            GameObject instanceObject = new GameObject("StaticCoroutine");
            instanceObject.AddComponent<StaticCoroutine>();
            m_instance = instanceObject.GetComponent<StaticCoroutine>();

            if (m_instance != null)
            { return m_instance; }

            Debug.LogError("Build did not generate a replacement instance. Method Failed!");

            return null;
        }

        // Overloaded Static Coroutine Methods which use Unity's default Coroutines.
        // Polymorphism applied for best compatibility with the standard engine.
        public static void Start(string methodName)
        { Build().StartCoroutine(methodName); }
        public static void Start(string methodName, object value)
        { Build().StartCoroutine(methodName, value); }
        public static void Start(IEnumerator routine)
        { Build().StartCoroutine(routine); }
    }
}
