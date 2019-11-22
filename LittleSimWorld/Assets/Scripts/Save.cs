using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[System.Serializable]
public class Save
{
    [SerializeField]
    public List<AtommInventory.Slot> itemsInInventory = new List<AtommInventory.Slot>();

    // [SerializeField] public SerializableDictMisc.StringFloatDictionary playerSaveDictionary = new SerializableDictMisc.StringFloatDictionary();                                 //Kira

    public  float Health = 100;
    public float maxHealth = 100;

    public float Energy = 100;
    public float maxEnergy = 100;

    public float Mood = 100;
    public float maxMood = 100;

    public float Food = 100;
    public float maxFood = 100;
    public float FoodDrainSpeed = 10;

    public float Thirst = 100;
    public float maxThirst = 100;
    public float ThirstDrainSpeed = 10;

    
    public float Bladder = 100;
    public float MaxBladder = 100;
    public float BladderDrainSpeed = 10;

    public float Hygiene = 100;
    public float MaxHygiene = 100;
    public float HygieneDrainSpeed = 10;

    public float Money = 2000;
    public float XPmultiplayer = 1;
    public float PriceMultiplayer = 1;

    public PlayerStatsManager.Skills intelligenceSkill;

    public PlayerStatsManager.Skills strengthSkill;

    public PlayerStatsManager.Skills fitnesSkill;

    public PlayerStatsManager.Skills charismaSkill;

    public PlayerStatsManager.Skills cookingSkill;

    public PlayerStatsManager.Skills repairSkill;

    public float time;
    public int days;
    public int season;
    public int WeekDay;

    public float playerX;
    public float playerY;

    public List<string> Upgrades = new List<string>();
    public List<string> UpgradesTier = new List<string>();

    public float RealPlayTime;

    public List<string> CompletedMissions = new List<string>();
    public List<string> CurrentMissions = new List<string>();

    public JobsPopUp.Jobs SavedCurrentJob;

    public float repairSpeed;


    public double moneyInBank;
    public float percentagePerDay;

    /*public void SetSaveDictionary(SerializableDictMisc.StringFloatDictionary dictionary)
    {        
        foreach (string key in dictionary.Keys)
        {
            playerSaveDictionary.Add(key, dictionary[key]);
        }
    }*/
}
