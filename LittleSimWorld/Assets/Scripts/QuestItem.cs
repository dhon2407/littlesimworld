using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStats;
using System.Linq;

public class QuestItem : Mission
{


    public ConsumableItem itemToCopmplete;

    
    // Start is called before the first frame update
    void Start()
    {
        GetRewardButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0; i < AtommInventory.inventory.Count; i++)
        {
            if (AtommInventory.inventory[i].itemName== itemToCopmplete.ItemName)
            {
                
                    Accomplish();
                    

                  
      
                
            }
        }

           
        
    }
    
   
   
}
