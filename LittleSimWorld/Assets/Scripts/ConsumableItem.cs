using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStats;
using Timers;
using System.Linq;

    [CreateAssetMenu] [System.Serializable]
    public class ConsumableItem : Item
    {
        public float Hunger;
        public float Mood;
        public float Health;
        public float Energy;
   
    public float Thirst;
    
        
        
        public enum ConsumableType
    {
        Food,
        Drink
    }

        [Space]
        public float HungerPercentBonus;
        public float MoodPercentBonus;
        public float HealthPercentBonus;
        public float EnergyPercentBonus;
    public float ThirstPercentBonus;
   

        public override bool Use(float ItemID)

        {


        
        var temp = AtommInventory.inventory.Where
            (obj => obj.ID == ItemID).FirstOrDefault();

        if (temp == null)
        {
            return false;
        }

            temp.quantity -= 1;
        if(temp.quantity <= 0)
        {
            AtommInventory.inventory.Remove(temp);
        }

        if (UsageSound)
            AtommInventory.Instance.SpawnFX(UsageSound);
        AtommInventory.Refresh();

        PlayerStatsManager.Instance.AddFood(Hunger);
            PlayerStatsManager.Instance.AddFood(PlayerStatsManager.Instance.MaxFood * HungerPercentBonus);

            PlayerStatsManager.Instance.AddEnergy(Energy);
            PlayerStatsManager.Instance.AddEnergy(PlayerStatsManager.Instance.MaxEnergy * EnergyPercentBonus);

            PlayerStatsManager.Instance.Heal(Health);
            PlayerStatsManager.Instance.Heal(PlayerStatsManager.Instance.MaxHealth * HealthPercentBonus);

            PlayerStatsManager.Instance.AddMood(Mood);
            PlayerStatsManager.Instance.AddMood(PlayerStatsManager.Instance.MaxMood * MoodPercentBonus);

        PlayerStatsManager.Instance.AddThirst(Thirst);
        PlayerStatsManager.Instance.AddThirst(PlayerStatsManager.Instance.MaxThirst * ThirstPercentBonus);

       

        GameLibOfMethods.AddChatMessege("Consumed " + ItemName);
        
        return true;



    }
        /*public IEnumerator IECooldown()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                
                yield break;
            }
            
        }*/

    }

