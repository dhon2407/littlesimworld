using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using TMPro;
using CharacterStats;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    public TextMeshProUGUI PhysicsLVLText;
    public TextMeshProUGUI PhysicsXPText;
    public TextMeshProUGUI StrengthLVLText;
    public TextMeshProUGUI StrengthXPText;
    public TextMeshProUGUI CharismaLVLText;
    public TextMeshProUGUI CharismaXPText;
    public TextMeshProUGUI FitnessLVLText;
    public TextMeshProUGUI FitnessXPText;
    public TextMeshProUGUI CookingLVLText;
    public TextMeshProUGUI CookingXPText;
    public TextMeshProUGUI RepairLVLText;
    public TextMeshProUGUI RepairXPText;

    public TextMeshProUGUI TotalLevelText;

    public ParticleSystem levelUpParticles;

    public bool passingOut = false;


    /*
     Ok, so instead of doing all of this variables public and static, I used a serializable Dictionary, so I can assign the values in the inspector(so the designer or anyone can easily change the values 
     I will leave an example in the Start() of accessing it
     */
    //[SerializeField]
    //public SerializableDictMisc.StringFloatDictionary playerStatsDictionary = new SerializableDictMisc.StringFloatDictionary();//here is how I declare a StringFloat serializable dictionary, I can do a StringInt too. For StringString or others I can easily implement them
    //You can see SerializableDiscMisc script for more details about implementation
    //I also set the values in the inspector, you can look over at the Scripts GameObject
    
   
    
    public  float Health = 100;
    public  float MaxHealth = 100;
    public float HealthDrainSpeedPerHour = 10;
    public float HealthDrainSpeedPerHourIfPunished = 30;

    public float Energy = 100;
    public float EnergyDrainSpeedPerHour = 10;
    public float MaxEnergy = 100;

    public  float Mood = 100;
    public float MoodDrainSpeedPerHour = 10;
    public float MaxMood = 100;

    public  float Food = 100;
    public  float MaxFood = 100;
    public float FoodDrainSpeedPerHour = 10;
    
    public  float Hygiene = 100;
    public float MaxHygiene = 100;
    public float HygieneDrainSpeedPerHour = 1;

    public float Thirst = 100;
    public float MaxThirst = 100;
    public float ThirstDrainSpeedPerHour = 1;

    public float Bladder = 100;
    public float MaxBladder = 100;
    public float BladderDrainSpeedPerHour = 1;

    public  float Money = 2000;
    public  float PriceMultiplier = 1;
    
    public  float XPMultiplier = 1;
    public  float BonusXPMultiplier = 0;
    
    public int TotalLevel = 0;

    public float MovementSpeed = 1;
    
    public float RepairSpeed = 1;
    

    private void Awake()
    {
        Instance = this;
       
    }
    private void Start()
    {
        //Example of using the player stats dictionary
        
       // float hp = Health;
       // float maxHp = maxHealth;
        
        //Debug.Log($"HP is {hp} and max health is {maxHp}");
    }

    private void Update()
    {
        //TotalLevel = (Intelligence.Instance.Level + Fitness.Instance.Level + Strength.Instance.Level + Charisma.Instance.Level + Cooking.Instance.Level + Repair.Instance.Level) - 6;
        
        TotalLevel = (Intelligence.Instance.Level + Fitness.Instance.Level +
                                               Strength.Instance.Level + Charisma.Instance.Level +
                                               Cooking.Instance.Level + Repair.Instance.Level) - 6;
        
        //TotalLevelText.text = "Total level: " + TotalLevel;
        
        TotalLevelText.text = "Total level: " + TotalLevel;

        if(Intelligence.Instance.Level - 1 == 0)
        {
            PhysicsLVLText.text = "-";
             PhysicsXPText.text = " XP: " + Mathf.RoundToInt(Intelligence.Instance.XP) + "/" + Mathf.Abs(Intelligence.Instance.RequiredXP);
        }
        else
        {
            PhysicsLVLText.text =  (Intelligence.Instance.Level - 1).ToString();
            PhysicsXPText.text = " XP: " + Mathf.RoundToInt(Intelligence.Instance.XP) + "/" + Mathf.Abs(Intelligence.Instance.RequiredXP);
        }
       
        if(Strength.Instance.Level - 1 == 0)
        {
            StrengthLVLText.text =   "-";
            StrengthXPText.text = " XP: " + Mathf.RoundToInt(Strength.Instance.XP) + "/" + Mathf.Abs(Strength.Instance.RequiredXP);
        }
        else
        {
            StrengthLVLText.text = (Strength.Instance.Level - 1).ToString();
            StrengthXPText.text = " XP: " + Mathf.RoundToInt(Strength.Instance.XP) + "/" + Mathf.Abs(Strength.Instance.RequiredXP);
        }
       
        if(Charisma.Instance.Level - 1 == 0)
        {
            CharismaLVLText.text = "-";
            CharismaXPText.text = " XP: " + Mathf.RoundToInt(Charisma.Instance.XP) + "/" + Mathf.Abs(Charisma.Instance.RequiredXP);
        }
        else
        {
            CharismaLVLText.text = (Charisma.Instance.Level - 1).ToString();
            CharismaXPText.text = " XP: " + Mathf.RoundToInt(Charisma.Instance.XP) + "/" + Mathf.Abs(Charisma.Instance.RequiredXP);
        }
        
        if(Fitness.Instance.Level - 1 == 0)
        {
            FitnessLVLText.text = "-";
            FitnessXPText.text = " XP: " + Mathf.RoundToInt(Fitness.Instance.XP) + "/" + Mathf.Abs(Fitness.Instance.RequiredXP);
        }
        else
        {
            FitnessLVLText.text = (Fitness.Instance.Level - 1).ToString();
            FitnessXPText.text = " XP: " + Mathf.RoundToInt(Fitness.Instance.XP) + "/" + Mathf.Abs(Fitness.Instance.RequiredXP);
        }
        if (Cooking.Instance.Level - 1 == 0)
        {
            CookingLVLText.text = "-";
            CookingXPText.text = " XP: " + Mathf.RoundToInt(Cooking.Instance.XP) + "/" + Mathf.Abs(Cooking.Instance.RequiredXP);
        }
        else
        {
            CookingLVLText.text =(Cooking.Instance.Level - 1).ToString();
            CookingXPText.text = " XP: " + Mathf.RoundToInt(Cooking.Instance.XP) + "/" + Mathf.Abs(Cooking.Instance.RequiredXP);
        }
        if (Repair.Instance.Level - 1 == 0)
        {
            RepairLVLText.text = "-";
            RepairXPText.text = " XP: " + Mathf.RoundToInt(Repair.Instance.XP) + "/" + Mathf.Abs(Repair.Instance.RequiredXP);
        }
        else
        {
            RepairLVLText.text = (Repair.Instance.Level - 1).ToString();
            RepairXPText.text = " XP: " + Mathf.RoundToInt(Repair.Instance.XP) + "/" + Mathf.Abs(Repair.Instance.RequiredXP);
        }


        if (Food > 0)
        {
            if (GameLibOfMethods.isSleeping)
            {
                Food -= (((FoodDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier)/2;
            }
            else
            {
                Food -= ((FoodDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier;
            }
            
        }
        if(Food <= 0 && !GameLibOfMethods.passedOut && GameLibOfMethods.player.gameObject.activeSelf)
        {
            Health -= (((HealthDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
        }

        if (Health > 0)
        {
            if (GameLibOfMethods.isSleeping)
            {
                Health -= (((HealthDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier) / 2;
            }
            else
            {
                Health -= ((HealthDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier;
            }
        }
        if (Health <= 0 && !GameLibOfMethods.passedOut && GameLibOfMethods.player.gameObject.activeSelf)
        {
            passingOut = true;
            GameLibOfMethods.PassOut();
        }

        if (Bladder > 0)
        {
            if (GameLibOfMethods.isSleeping)
            {
                Bladder -= (((BladderDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier) / 2;
            }
            else
            {
                Bladder -= ((BladderDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier;
            }
        }
        if (Bladder <= 0 && !GameLibOfMethods.passedOut && GameLibOfMethods.player.gameObject.activeSelf)
        {
            Bladder = MaxBladder;
            Hygiene = 0;
            GameLibOfMethods.animator.SetBool("PissingInPants", true);
        }

        if (Hygiene > 0)
        {
            if (GameLibOfMethods.isSleeping)
            {
                Hygiene -= (((HygieneDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier) / 2;
            }
            else
            {
                Hygiene -= ((HygieneDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier;
            }
        }
        if (Hygiene <= 0 && !GameLibOfMethods.passedOut && GameLibOfMethods.player.gameObject.activeSelf)
        {
            Health -= (((HealthDrainSpeedPerHourIfPunished) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
        }
        if (Thirst > 0)
        {
            if (GameLibOfMethods.isSleeping)
            {
                Thirst -= (((ThirstDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier) / 2;
            }
            else
            {
                Thirst -= ((ThirstDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier;
            }
        }
        if (Thirst <= 0 && !GameLibOfMethods.passedOut && GameLibOfMethods.player.gameObject.activeSelf)
        {
            Health -= (((HealthDrainSpeedPerHourIfPunished) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
        }
        if (Energy > 0)
        {
            if (GameLibOfMethods.isSleeping)
            {
                Energy -= (((EnergyDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier) / 2;
            }
            else
            {
                Energy -= ((EnergyDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier;
            }
        }
        if (Energy <= 0 && !GameLibOfMethods.passedOut && GameLibOfMethods.player.gameObject.activeSelf)
        {
            GameLibOfMethods.animator.SetBool("PassOutToSleep", true);

        }
        if (Mood > 0)
        {
            if (GameLibOfMethods.isSleeping)
            {
                Mood -= (((MoodDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier) / 2;
            }
            else
            {
                Mood -= ((MoodDrainSpeedPerHour) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier;
            }
        }

        if (PlayerStatsManager.Instance.Mood <= 0 && !GameLibOfMethods.passedOut && GameLibOfMethods.player.gameObject.activeSelf)
        {
            Health -= (((HealthDrainSpeedPerHourIfPunished) * (Time.deltaTime / DayNightCycle.Instance.speed)) * DayNightCycle.Instance.currentTimeSpeedMultiplier);
        }

    }
   
    [System.Serializable]
    public class Skills
    {
        public  int Level = 1;
        public float XP = 0;
        public float RequiredXP = 100;
        public int MaxLVL = 10;
        public static Skills Instance = new Skills();
        virtual public string SkillName{ get; set; }


        public virtual void AddXP(float amount)
        {
            /*XP += Mathf.RoundToInt(amount * (XPmultiplayer + BonusXPMultiplayer));
            if (XP >= RequiredXP && Level + 1 <= MaxLVL)
            {
                XP = 0;
                Level += 1;
                OnLevelUP();
                GameLibOfMethods.CreateFloatingText("Leveled UP!", 3);
            }*/
        }
        
        public virtual void Effect()
        {
            PlayerStatsManager.Instance.levelUpParticles.Play();
            Debug.Log("Nothing");
        }

    }
    [System.Serializable]
    public class Intelligence : PlayerStatsManager.Skills
    {
        new public static PlayerStatsManager.Skills Instance = new Intelligence();
        override public string SkillName
        {
            get { return SkillName = "Intelligence"; }
        }

        public override void Effect()
        {
            PlayerStatsManager.Instance.levelUpParticles.Play();
            PlayerStatsManager.Instance.XPMultiplier += 0.02f;
            GameLibOfMethods.CreateFloatingText("Now you receive " + (PlayerStatsManager.Instance.XPMultiplier + PlayerStatsManager.Instance.BonusXPMultiplier) + "% XP!", 2);
        }
      
        
        public override void AddXP(float amount)
        {
            Intelligence.Instance.XP += (amount * (PlayerStatsManager.Instance.XPMultiplier + PlayerStatsManager.Instance.BonusXPMultiplier));
            UIManager.Instance.LevelingSkill.text = SkillName + ": " + Level;
            UIManager.Instance.XPbar.fillAmount = XP / RequiredXP;
            UIManager.Instance.CurrentSkillImage.sprite = UIManager.Instance.Intelligence;
            GameLibOfMethods.StaticCoroutine.Start(UIManager.Instance.ShowLevelingSkill());
            BubbleSpawner.SpawnBubble();
            if (Intelligence.Instance.XP >= Intelligence.Instance.RequiredXP && Intelligence.Instance.Level + 1 <= Intelligence.Instance.MaxLVL)
            {
                Intelligence.Instance.XP = 0;
                Intelligence.Instance.Level += 1;
                Effect();
                GameLibOfMethods.CreateFloatingText("Leveled UP!", 3);
               
            }
        }

    }
    [System.Serializable]
    public class Strength : PlayerStatsManager.Skills
    {
        new public static PlayerStatsManager.Skills Instance = new Strength();

        override public string SkillName
        {
            get { return SkillName = "Strength"; }
        }
        public override void Effect()
        {
            PlayerStatsManager.Instance.levelUpParticles.Play();
            AtommInventory.InventoryCapacity += 1;
            GameLibOfMethods.CreateFloatingText("Now you have " + AtommInventory.InventoryCapacity.ToString() + " slots in your inventory!", 2);
        }
        
         public override void AddXP(float amount)
        {
            Strength.Instance.XP += (amount * (PlayerStatsManager.Instance.XPMultiplier + PlayerStatsManager.Instance.BonusXPMultiplier));
            UIManager.Instance.LevelingSkill.text = SkillName + ": " + Level;
            UIManager.Instance.XPbar.fillAmount = XP / RequiredXP;
            UIManager.Instance.CurrentSkillImage.sprite = UIManager.Instance.Strength;
            GameLibOfMethods.StaticCoroutine.Start(UIManager.Instance.ShowLevelingSkill());
            BubbleSpawner.SpawnBubble();
            if (Strength.Instance.XP >= Strength.Instance.RequiredXP && Strength.Instance.Level + 1 <= Strength.Instance.MaxLVL)
            {
                Strength.Instance.XP = 0;
                Strength.Instance.Level += 1;
                Effect();
                GameLibOfMethods.CreateFloatingText("Leveled UP!", 3);
                
            }
        }

    }
    [System.Serializable]
    public class Fitness : PlayerStatsManager.Skills
    {
        new public static PlayerStatsManager.Skills Instance = new Fitness();

        override public string SkillName
        {
            get { return SkillName = "Fitness"; }
        }

        public override void Effect()
        {
            PlayerStatsManager.Instance.levelUpParticles.Play();
            PlayerStatsManager.Instance.MaxEnergy += 5;
            GameLibOfMethods.CreateFloatingText("Now you have " + PlayerStatsManager.Instance.MaxEnergy + " max energy!", 4);
        }
        public override void AddXP(float amount)
        {
            Fitness.Instance.XP += (amount * (PlayerStatsManager.Instance.XPMultiplier + PlayerStatsManager.Instance.BonusXPMultiplier));
            UIManager.Instance.LevelingSkill.text = SkillName + ": " + Level;
            UIManager.Instance.XPbar.fillAmount = XP / RequiredXP;
            UIManager.Instance.CurrentSkillImage.sprite = UIManager.Instance.Fitness;
            GameLibOfMethods.StaticCoroutine.Start(UIManager.Instance.ShowLevelingSkill());
            BubbleSpawner.SpawnBubble();
            if (Fitness.Instance.XP >= Fitness.Instance.RequiredXP && Fitness.Instance.Level + 1 <= Fitness.Instance.MaxLVL)
            {
                Fitness.Instance.XP = 0;
                Fitness.Instance.Level += 1;
                Effect();
                GameLibOfMethods.CreateFloatingText("Leveled UP!", 3);
               
            }
        }

    }
    [System.Serializable]
    public class Charisma : PlayerStatsManager.Skills
    {

        new public static PlayerStatsManager.Skills Instance = new Charisma();
        override public string SkillName
        {
            get { return SkillName = "Charisma"; }
        }

        public override void Effect()
        {
            PlayerStatsManager.Instance.levelUpParticles.Play();
            PlayerStatsManager.Instance.PriceMultiplier -= 0.02f;
            GameLibOfMethods.CreateFloatingText("Items now costs only " + PlayerStatsManager.Instance.PriceMultiplier.ToString() + "% of their price!", 2);

        }

        public override void AddXP(float amount)
        {
            Charisma.Instance.XP += (amount * (PlayerStatsManager.Instance.XPMultiplier + PlayerStatsManager.Instance.BonusXPMultiplier));
            UIManager.Instance.LevelingSkill.text = SkillName + ": " + Level;
            UIManager.Instance.XPbar.fillAmount = XP / RequiredXP;
            UIManager.Instance.CurrentSkillImage.sprite = UIManager.Instance.Charisma;
            GameLibOfMethods.StaticCoroutine.Start(UIManager.Instance.ShowLevelingSkill());
            BubbleSpawner.SpawnBubble();
            if (Charisma.Instance.XP >= Charisma.Instance.RequiredXP && Charisma.Instance.Level + 1 <= Charisma.Instance.MaxLVL)
            {
                Charisma.Instance.XP = 0;
                Charisma.Instance.Level += 1;
                Effect();
                PlayerStatsManager.Instance.PriceMultiplier -= 0.02f;
                GameLibOfMethods.CreateFloatingText("Items now costs only " + PlayerStatsManager.Instance.PriceMultiplier.ToString() + "% of their price!", 2);
                GameLibOfMethods.CreateFloatingText("Leveled UP!", 3);
              
            }
        }

    }
    [System.Serializable]
    public class Cooking : PlayerStatsManager.Skills
    {

        new public static PlayerStatsManager.Skills Instance = new Cooking();
        override public string SkillName
        {
            get { return SkillName = "Cooking"; }
        }

        public override void Effect()
        {
            PlayerStatsManager.Instance.levelUpParticles.Play();
            GameLibOfMethods.AddChatMessege(SkillName + " level UP!");
        }

        public override void AddXP(float amount)
        {
            Instance.XP += (amount * (PlayerStatsManager.Instance.XPMultiplier + PlayerStatsManager.Instance.BonusXPMultiplier));
            UIManager.Instance.LevelingSkill.text = SkillName + ": " + Level;
            UIManager.Instance.XPbar.fillAmount = XP / RequiredXP;
            UIManager.Instance.CurrentSkillImage.sprite = UIManager.Instance.Cooking;
            GameLibOfMethods.StaticCoroutine.Start(UIManager.Instance.ShowLevelingSkill());
            BubbleSpawner.SpawnBubble();
            if (XP >= RequiredXP && Instance.Level + 1 <= Instance.MaxLVL)
            {
                Instance.XP = 0;
                Level += 1;
                Effect();
                GameLibOfMethods.CreateFloatingText("Leveled UP!", 3);
               
            }
        }

    }
    [System.Serializable]
    public class Repair : PlayerStatsManager.Skills
    {

        new public static PlayerStatsManager.Skills Instance = new Repair();
        override public string SkillName
        {
            get { return SkillName = "Repair"; }
        }

        public override void Effect()
        {
            PlayerStatsManager.Instance.RepairSpeed += 0.05f;
            GameLibOfMethods.AddChatMessege(SkillName + " level UP!");
            PlayerStatsManager.Instance.levelUpParticles.Play();
        }

        public override void AddXP(float amount)
        {
            Instance.XP += (amount * (PlayerStatsManager.Instance.XPMultiplier + PlayerStatsManager.Instance.BonusXPMultiplier));
            UIManager.Instance.LevelingSkill.text = SkillName + ": " + Level;
            UIManager.Instance.XPbar.fillAmount = XP / RequiredXP;
            UIManager.Instance.CurrentSkillImage.sprite = UIManager.Instance.Repair;
            GameLibOfMethods.StaticCoroutine.Start( UIManager.Instance.ShowLevelingSkill());
            BubbleSpawner.SpawnBubble();
            if (XP >= RequiredXP && Instance.Level + 1 <= Instance.MaxLVL)
            {
                Instance.XP = 0;
                Level += 1;
                Effect();
                GameLibOfMethods.CreateFloatingText("Leveled UP!", 3);
                
            }
        }

    }



    /*public static void SubstractEnergy(float amount)
    {
        Energy -= amount;
    }

    public static void AddEnergy(float amount)
    {
        if (Energy + amount < maxEnergy)
            Energy += amount;
        else
            Energy = maxEnergy;
    }

    public static void SetStamina(float amount)
    {
        Energy = amount;
    }*/

    public void SubstractEnergy(float amount)
    {
        Energy -= amount;
    }

    public void AddEnergy(float amount)
    {
        if (Energy + amount < MaxEnergy)
            Energy += amount;
        else
            Energy = MaxEnergy;
    }

    public void SetStamina(float amount)
    {
        Energy = amount;
    }

    /*public static void AddFood(float amount)
    {
        if (Food + amount < maxFood)
            Food += amount;
        else
            Food = maxFood;
    }
    public static void SetFood(float amount)
    {
        Food = amount;
    }
    public static void SubstractFood(float amount)
    {
        Food -= amount;
    }*/
    
    public void AddFood(float amount)
    {
        if (Food + amount < MaxFood)
            Food += amount;
        else
            Food = MaxFood;
    }
    public void SetFood(float amount)
    {
        Food = amount;
    }
    public void SubstractFood(float amount)
    {
        Food -= amount;
    }
    
    /*public static void SubstractMoney(int amount)
    {
        if (Money - amount > 0)
            Money -= amount;
        else
            Money = 0;
    }*/
    
    public void SubstractMoney(int amount)
    {
        if (Money - amount > 0)
            Money -= amount;
        else
            Money = 0;
    }
    
    /*public static void Heal(float amount)
    {
        if (Health + amount < maxHealth)
            Health += amount;
        else
            Health = maxHealth;
    }*/

    public void Heal(float amount)
    {
        if (Health + amount < MaxHealth)
        {
            Health += amount;
        }
        else
        {
            Health = MaxHealth;
        }
    }
    
    /*public static void AddMood(float amount)
    {
        if (Mood + amount < maxMood)
            Mood += amount;
        else
            Mood = maxMood;
    }*/

    public void AddMood(float amount)
    {
        if (Mood + amount < MaxMood)
        {
            Mood += amount;
        }
        else
        {
            Mood = MaxMood;
        }
    }

    /*public static void AddMoney(int amount)
    {
        Money += amount;
      
    }*/

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    /*public static void Addhygiene(float amount)
    {
        if (hygiene + amount < maxhygiene)
            hygiene += amount;
        else
            hygiene = maxhygiene;
    }*/

    public void AddHygiene(float amount)
    {
        if (Hygiene + amount < MaxHygiene)
        {
            Hygiene += amount;
        }
        else
        {
            Hygiene = MaxHygiene;
        }
    }
    
    /*public static void AddBladder(float amount)
    {
        if (Bladder + amount < maxBladder)
            Bladder += amount;
        else
            Bladder = maxBladder;
    }*/
    
    public void AddBladder(float amount)
    {
        if (Bladder + amount < PlayerStatsManager.Instance.MaxBladder)
            Bladder += amount;
        else
            Bladder = PlayerStatsManager.Instance.MaxBladder;
    }
    
    /*public static void AddSanity(float amount)
    {
        if (sanity + amount < maxSanity)
            sanity += amount;
        else
            sanity = maxSanity;
    }*/
    
    
    /*public static void AddThirst(float amount)
    {
        if (thirst + amount < maxThirst)
            thirst += amount;
        else
            thirst = maxThirst;
    }*/

    public void AddThirst(float amount)
    {
        if (Thirst + amount < MaxThirst)
            Thirst += amount;
        else
            Thirst = MaxThirst;
    }


}

