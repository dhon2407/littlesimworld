using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CharacterStats;

[System.Serializable]
public class AtommItem : MonoBehaviour
{
    public int quantity;
    public string iconPath ;
    public Item itemSO;

   
    private void OnValidate()
    {
        
    }
}

