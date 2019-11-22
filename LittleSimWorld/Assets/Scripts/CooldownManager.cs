using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStats;

public class CooldownManager : MonoBehaviour
{
    public static List<Item> ItemsOnCooldown = new List<Item>();
    void Start()
    {

    }


    void Update()
    {
        if(ItemsOnCooldown.Count > 0)
        foreach (Item item in ItemsOnCooldown)
        {
            item.currentCooldown -= Time.deltaTime;
            if (item.currentCooldown <= 0)
            {
                ItemsOnCooldown.Remove(item);
            }
        }
    }

    public static void PutOnCooldown(Item item)
    {
        if(!ItemsOnCooldown.Contains(item))
        CooldownManager.ItemsOnCooldown.Add(item);
        item.currentCooldown = item.MaxCooldown;


    } 
}
