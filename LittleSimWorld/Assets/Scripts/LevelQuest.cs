using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelQuest : Mission

     
{
    public int RequieredStrLevel = 2;
    public int RequiredIntLevel = 2;
    public int RequiredFitLevel = 2;
    public int RequiredChrLevel = 2;
    public int RequiredCookingLevel = 2;
    public int RequiredRepairLevel = 2;

    // Start is called before the first frame update
    private void Start()
    {
        GetRewardButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStatsManager.Strength.Instance.Level >= RequieredStrLevel ||
            PlayerStatsManager.Intelligence.Instance.Level >= RequiredIntLevel ||
            PlayerStatsManager.Fitness.Instance.Level >= RequiredFitLevel ||
            PlayerStatsManager.Charisma.Instance.Level >= RequiredChrLevel||
            PlayerStatsManager.Cooking.Instance.Level >= RequiredCookingLevel||
            PlayerStatsManager.Repair.Instance.Level >= RequiredRepairLevel)

        {
            if (accomplished == false)
            {
              
                    

                Accomplish();
                

               
               
               
            }
        }
        
    }
}
