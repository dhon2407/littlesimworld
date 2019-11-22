using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] class ShopUpgrades
{
    public GameObject upgradableGO;
    
    //These values will be loaded at runtime, from scriptable object
    
    [HideInInspector] public GameObject upgradesInto;
    [HideInInspector] public UpgradesShop.ShopUpgrades nextUpgrade;
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
                                    
            nextUpgrade = new UpgradesShop.ShopUpgrades();
            
            nextUpgrade.LoadDataFromScriptableObject(nextUpgradeSO);            
        }
    }
}
