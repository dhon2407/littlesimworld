using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
[System.Serializable]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string CurrentSaveName;
    public float PlayTime;
    public TMPro.TMP_InputField saveName;
    public bool IsStartingNewGame = false;
    public List<string> UpgradableGOsNames = new List<string>();
    //public List<string> UpgradeTiers = new List<string>();

    [SerializeField] public SerializableDictMisc.StringFloatDictionary playerSaveTemplateDictionary;
    
    private Save CreateSaveGameObject()
    {
        Save save = new Save();
        
        //save.SetSaveDictionary(playerSaveTemplateDictionary);
        
        int i = 0;
        foreach (AtommInventory.Slot slot in AtommInventory.inventory)
        {


            save.itemsInInventory.Add(slot);
                i++;

        }

        save.Health = PlayerStatsManager.Instance.Health;
        save.maxHealth = PlayerStatsManager.Instance.MaxHealth;

        save.Energy = PlayerStatsManager.Instance.Energy;
        save.maxEnergy = PlayerStatsManager.Instance.MaxEnergy;

        save.Mood = PlayerStatsManager.Instance.Mood;
        save.maxMood = PlayerStatsManager.Instance.MaxMood;

        save.Food = PlayerStatsManager.Instance.Food;
        save.maxFood = PlayerStatsManager.Instance.MaxFood;

        save.Thirst = PlayerStatsManager.Instance.Thirst;
        save.maxThirst = PlayerStatsManager.Instance.MaxThirst;

        save.Hygiene = PlayerStatsManager.Instance.Hygiene;
        save.MaxHygiene = PlayerStatsManager.Instance.MaxHygiene;

        save.Bladder = PlayerStatsManager.Instance.Bladder;
        save.MaxBladder = PlayerStatsManager.Instance.MaxBladder;

        save.Money = PlayerStatsManager.Instance.Money;
        save.XPmultiplayer = PlayerStatsManager.Instance.XPMultiplier;
        save.PriceMultiplayer = PlayerStatsManager.Instance.PriceMultiplier;

        /*foreach(KeyValuePair<string,float> dic in PlayerStatsManager.Instance.playerStatsDictionary)
        {                                                                                                                           //Kira
            save.playerSaveDictionary[dic.Key] = dic.Value;
        }*/

        save.intelligenceSkill = PlayerStatsManager. Intelligence.Instance;
        

        save.strengthSkill = PlayerStatsManager.Strength.Instance;
       

        save.fitnesSkill = PlayerStatsManager.Fitness.Instance;
       
        save.charismaSkill = PlayerStatsManager.Charisma.Instance;
        

        save.cookingSkill = PlayerStatsManager.Cooking.Instance;
        
        save.repairSkill = PlayerStatsManager.Repair.Instance;

        save.repairSpeed = PlayerStatsManager.Instance.RepairSpeed;

        save.time = DayNightCycle.Instance.time;
        save.days = DayNightCycle.Instance.days;
        save.season = DayNightCycle.Instance.season;
        save.WeekDay = DayNightCycle.Instance.WeekDay;

        save.RealPlayTime = PlayTime;


        save.SavedCurrentJob = JobsPopUp.CurrentJob;

        save.playerX = GameLibOfMethods.player.transform.position.x;
        save.playerY = GameLibOfMethods.player.transform.position.y;

        //UpgradeTiers.Clear();
        foreach (string name in UpgradableGOsNames)
        {
            save.Upgrades.Add(name);
        }
        for(int p = 0; p < UpgradableGOsNames.Count; p++)
        {
            save.UpgradesTier.Add(GameObject.Find(UpgradableGOsNames[p]).transform.GetChild(0).gameObject.name);
            Debug.Log(UpgradableGOsNames[p] + save.UpgradesTier[p]);
        }
     
        

        save.CompletedMissions = new List<string>( MissionHandler.CompletedMissions);
        save.CurrentMissions = new List<string>( MissionHandler.CurrentMissions);

        save.moneyInBank = Bank.Instance.MoneyInBank;
        save.percentagePerDay = Bank.Instance.PercentagePerDay;
         


        return save;
    }
    
   /* private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveGame();
        }
    }*/
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);

            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            LoadGame();
        }
        SceneManager.activeSceneChanged +=
         delegate {

             if (!IsStartingNewGame)
                 LoadGame();
             else
             {
                 SaveGame();
             }

         };
    }

    private void FixedUpdate()
    {
        PlayTime += Time.deltaTime;
    }

    public void SaveGame()
    {
        // 1
        Save save = CreateSaveGameObject();

        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath +"/" + CurrentSaveName + ".save");
        file.Position = 0;
        bf.Serialize(file, save);
        file.Close();

        


        Debug.Log("Game Saved");
    }
    public void LoadGame()
    {
        // 1
        if (File.Exists(Application.persistentDataPath + "/" + CurrentSaveName + ".save"))
        {


            // 2
            BinaryFormatter bf = new BinaryFormatter();
            
            FileStream file = File.Open(Application.persistentDataPath + "/" + CurrentSaveName + ".save", FileMode.Open);
            file.Position = 0;
            Save save = (Save)bf.Deserialize(file);
            

            // 3
            for (int i = 0; i < save.itemsInInventory.Count; i++)
            {
                AtommInventory.inventory.Add(save.itemsInInventory[i]);
            }

           /* PlayerStatsManager.Instance.Health = save.playerSaveDictionary["Health"];*/
            
            PlayerStatsManager.Instance.Health = save.Health;
            PlayerStatsManager.Instance.MaxHealth = save.maxHealth;

            PlayerStatsManager.Instance.Energy = save.Energy;
            PlayerStatsManager.Instance.MaxEnergy = save.maxEnergy;

            PlayerStatsManager.Instance.Mood = save.Mood;
            PlayerStatsManager.Instance.MaxMood = save.maxMood;


            PlayerStatsManager.Instance.Food = save.Food;
            PlayerStatsManager.Instance.MaxFood = save.maxFood;

            PlayerStatsManager.Instance.Thirst = save.Thirst;
            PlayerStatsManager.Instance.MaxThirst = save.maxThirst;

            PlayerStatsManager.Instance.Bladder = save.Bladder;
            PlayerStatsManager.Instance.MaxBladder = save.MaxBladder;

            PlayerStatsManager.Instance.Hygiene = save.Hygiene;
            PlayerStatsManager.Instance.MaxHygiene = save.MaxHygiene;




            PlayerStatsManager.Instance.Money = save.Money;
            PlayerStatsManager.Instance.XPMultiplier = save.XPmultiplayer;
            PlayerStatsManager.Instance.PriceMultiplier = save.PriceMultiplayer;
            
            PlayerStatsManager.Intelligence.Instance = save.intelligenceSkill;

            PlayerStatsManager.Strength.Instance = save.strengthSkill;

            PlayerStatsManager.Fitness.Instance = save.fitnesSkill;

            PlayerStatsManager.Charisma.Instance = save.charismaSkill;

            PlayerStatsManager.Cooking.Instance = save.cookingSkill;

            PlayerStatsManager.Repair.Instance = save.repairSkill;

            DayNightCycle.Instance.time = save.time;
            DayNightCycle.Instance.days = save.days;
            DayNightCycle.Instance.WeekDay = save.WeekDay;
            DayNightCycle.Instance.season = save.season;

            PlayTime = save.RealPlayTime;

            GameLibOfMethods.player.transform.position = new Vector2(save.playerX, save.playerY);

            
            if(save.CompletedMissions != null)
            MissionHandler.CompletedMissions = new List<string>(save.CompletedMissions);
            if (save.CurrentMissions != null)
                MissionHandler.CurrentMissions = new List<string>(save.CurrentMissions);
            
            MissionHandler.missionHandler.ReactivateOld();
           
            foreach( string mission in MissionHandler.CurrentMissions)
            {
                Debug.Log(mission);
            }

            JobsPopUp.CurrentJob = save.SavedCurrentJob;

            for (int p = 0; p < UpgradableGOsNames.Count; p++)
            {
               // UpgradeTiers.Add(save.UpgradesTier[p]);
            }

            for (int i = 0; i < UpgradableGOsNames.Count; i++)
            {
                //Debug.Log("Upgradable Bed".Replace("Upgradable ", ""));
                if (Resources.Load<GameObject>("Upgrades/"+ UpgradableGOsNames[i].Replace("Upgradable ","") + "/" + save.UpgradesTier[i]))
                {
                    
                    Destroy(GameObject.Find(UpgradableGOsNames[i]).transform.GetChild(0).gameObject);
                    GameObject temp = Instantiate(Resources.Load<GameObject>("Upgrades/" + UpgradableGOsNames[i].Replace("Upgradable ", "") + "/" + save.UpgradesTier[i]),
                    GameObject.Find(UpgradableGOsNames[i]).transform);
                    temp.name = save.UpgradesTier[i];
                }
            }
            

            Bank.Instance.MoneyInBank = save.moneyInBank;
            Bank.Instance.PercentagePerDay = save.percentagePerDay;
            Bank.Instance.UpdateBalance();




            Debug.Log("Game Loaded");
            file.Close();
        }
        else
        {
            Debug.Log("No game saved!");
        }
        
        
    }
    public void ChangeCurrentSave()
    {

        CurrentSaveName = saveName.text;
    }
    public void NewGame()
    {
        IsStartingNewGame = true;
        ChangeCurrentSave();
        Save save = new Save();

        //save.SetSaveDictionary(playerSaveTemplateDictionary);
        
        // 2
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + CurrentSaveName + ".save");
        file.Position = 0;
        bf.Serialize(file, save);
        file.Close();
        MainMenu.Instance.LoadMainSceneGame(1);



    }
   
}
