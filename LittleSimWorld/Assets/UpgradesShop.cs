using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStats;
public class UpgradesShop : MonoBehaviour
{
    /* Here is a list of some things which in my opinion I should change, and why:
   
    -There are a little bit too many references, e.g: upgrade.upgradableGO.transform.GetChild(0).name
    -Gameobject name and currentUpgradeLevel should not be related
    -UpgradesShop shouldn't handle object loading and checking if the object is not null and valid, e.g: if (temp != null && upgrade.upgradesInto.gameObject.name == temp.name)
    -UpgradesShop shouldn't handle AudioPlaying
    -Variables don't have to be public if there is no need to -> use [SerializeField] and a Get method if something needs to be accessed
    
    */
    
   // public List<ShopUpgrades> UpgradableObjects;// = new List<ShopUpgrades>();the only public reference which must be put from the inspector is the Gameobject which you wan to upgrade
    public List<AtommInventory.Upgrade> purchasableUpgrades = new List<AtommInventory.Upgrade>();

    //public AudioClip open, close;

    [SerializeField] private List<ShopUpgradeScriptableObject> shopUpgrades;//these are the upgrades which should be applied to the ShopUpgrades
    
    //[SerializeField] private ShopUpgradeChecker shopUpgradeChecker;
    //[SerializeField] private ShopUpgradeSoundHandler shopUpgradeSoundHandler;

    private GameObject tempGameObjectToLoad;
    private int upgradeLevel;

    private GameObject tempLoadedObject;
    [SerializeField] private AudioClip open, close;

    [SerializeField] private AudioSource source;

    //AudioSource source;


    private void OnValidate()
    {
        //Refresh();
    }

    public void Refresh()
    {
           
           /* purchasableUpgrades.Clear();
        foreach (ShopUpgrades upgrade in upgrades)
        {
            int currentUpgradeLevel = int.Parse(upgrade.upgradableGO.transform.GetChild(0).name);
            if (currentUpgradeLevel == 0)
                currentUpgradeLevel = 1;
            Debug.Log( "Current upgrade level:" + currentUpgradeLevel);
            GameObject temp = Resources.Load<GameObject>("Upgrades/" + upgrade.PathCore + "/" + (currentUpgradeLevel + 1));
            if (temp != null && upgrade.upgradesInto.gameObject.name == temp.name)
            {
                purchasableUpgrades.Add(new AtommInventory.Upgrade(upgrade));
                break;
            }
            else
            {
                Debug.Log("No " + upgrade.PathCore + " upgrade found in "+ "Upgrades/" + upgrade.PathCore + "/" + (currentUpgradeLevel + 1));
            }
        }*/
           
            purchasableUpgrades.Clear();
            HandleShopUpgrades();
    }
    public GameObject LoadUpgrade(string pathOfTheGameobjectToLoad)
    {
        tempGameObjectToLoad = Resources.Load<GameObject>(pathOfTheGameobjectToLoad);

        if (tempGameObjectToLoad == null)
        {
            Debug.LogError("No upgrade found at the specified PATH: " + pathOfTheGameobjectToLoad);

            return null;
        }

        return tempGameObjectToLoad;
    }
    /* void HandleShopUpgrades()
     {
         for(int i = 0; i < UpgradableObjects.Count; i++)
         {
             if (shopUpgrades[i] != null)
             {
                 UpgradableObjects[i].LoadDataFromScriptableObject(shopUpgrades[i]);

                 upgradeLevel = UpgradableObjects[i].upgradeLevel;

                 if (upgradeLevel == 0)
                 {
                     upgradeLevel++;
                 }

                 tempLoadedObject =
                     LoadUpgrade("Upgrades/" + UpgradableObjects[i].PathCore + "/" + (upgradeLevel + 1));                   //working version

                 if (LoadedUpgradeIsTheCorrectUpgrade(tempLoadedObject.name, upgradeLevel + 1))
                 {
                     //purchasableUpgrades.Add(new AtommInventory.Upgrade(upgrades[i]));
                     AtommInventory.Upgrade upgrade = new AtommInventory.Upgrade();

                     upgrade.InitializeUpgrade(UpgradableObjects[i]);
                     purchasableUpgrades.Add(upgrade);

                     break;
                 }
             }
         }
     }*/
    void HandleShopUpgrades()
    {
        for (int i = 0; i < shopUpgrades.Count; i++)
        {
            if(shopUpgrades[i] != null && shopUpgrades[i].PathCore != "")
            {

           
            GameObject UpgradableObjectParent =  GameObject.Find("Upgradable " + shopUpgrades[i].PathCore);
            Debug.Log(UpgradableObjectParent);
            if(shopUpgrades[i].upgradeLevel == int.Parse(UpgradableObjectParent.transform.GetChild(0).gameObject.name.ToString()) + 1)
            {
                AtommInventory.Upgrade upgrade = new AtommInventory.Upgrade();

                upgrade.InitializeUpgrade(shopUpgrades[i]);
                purchasableUpgrades.Add(upgrade);
            }
            else
            {
                //shopUpgrades.Remove(shopUpgrades[i]);
            }
                /* UpgradableObjects[i].LoadDataFromScriptableObject(shopUpgrades[i]);

                 upgradeLevel = UpgradableObjects[i].upgradeLevel;

                 if (upgradeLevel == 0)
                 {
                     upgradeLevel++;
                 }

                 tempLoadedObject =
                     LoadUpgrade("Upgrades/" + UpgradableObjects[i].PathCore + "/" + (upgradeLevel + 1));

                 if (LoadedUpgradeIsTheCorrectUpgrade(tempLoadedObject.name, upgradeLevel + 1))
                 {
                     //purchasableUpgrades.Add(new AtommInventory.Upgrade(upgrades[i]));
                     AtommInventory.Upgrade upgrade = new AtommInventory.Upgrade();

                     upgrade.InitializeUpgrade(UpgradableObjects[i]);
                     purchasableUpgrades.Add(upgrade);

                     break;
                 }*/
            }
        }
    }
    public bool LoadedUpgradeIsTheCorrectUpgrade(string upgradeName, int id)
    {
        if (int.Parse(upgradeName) == id)
        {
            return true;
        }

        return false;
    }

    private void Start()
    {
        //source = GetComponent<AudioSource>();
    }

    public List<ShopUpgradeScriptableObject> GetShopUpgradesList()
    {
        return shopUpgrades;
    }
   

    public void Action()
    {
        /*if (source.clip == open)
        {
            source.clip = close; source.Play();
        }
        else
        {
            source.clip = open; source.Play();
        }*/
        PlayOpenCloseSound();
    }
    public void PlayOpenCloseSound()
    {
        if (source.clip == open)
        {
            source.clip = close;
            source.Play();
        }
        else
        {
            source.clip = open;
            source.Play();
        }
    }
    [Serializable]
    public class ShopUpgrades
    {
        public GameObject upgradableGO;

        //These values will be loaded at runtime, from scriptable object

        [HideInInspector] public GameObject upgradesInto;
        [HideInInspector] public ShopUpgrades nextUpgrade;
        [HideInInspector] public ShopUpgradeScriptableObject currentUpgradeSO;
        [HideInInspector] public ShopUpgradeScriptableObject nextUpgradeSO;
        [HideInInspector] public int price;
        [HideInInspector] public Sprite icon;
        [HideInInspector] public string PathCore;
        [HideInInspector] public int upgradeLevel;
        [HideInInspector] public bool isAlreadyBought;

        public void LoadDataFromScriptableObject(ShopUpgradeScriptableObject shopUpgradeScriptableObject)
        {
            currentUpgradeSO = shopUpgradeScriptableObject;
            upgradesInto = shopUpgradeScriptableObject.upgradesInto;
            nextUpgrade = shopUpgradeScriptableObject.nextShopUpgrade;
            price = shopUpgradeScriptableObject.price;
            icon = shopUpgradeScriptableObject.icon;
            PathCore = shopUpgradeScriptableObject.PathCore;
            upgradeLevel = shopUpgradeScriptableObject.upgradeLevel;

            if (shopUpgradeScriptableObject.nextShopUpgradeSO != null)
            {
                nextUpgradeSO = shopUpgradeScriptableObject.nextShopUpgradeSO;

                nextUpgrade = new ShopUpgrades();

                nextUpgrade.LoadDataFromScriptableObject(nextUpgradeSO);
            }
        }
    }
}