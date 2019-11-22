using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using CharacterStats;

public class SlotTooltip : MonoBehaviour, IPointerEnterHandler
{
   
    public static GameObject canvas;
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;
    public ConsumableItem consumable;

    void Start()
    {
        AtommInventory temp = FindObjectOfType<AtommInventory>();
       

        ItemName = GameObject.Find("Item Name Text").GetComponent<TextMeshProUGUI>();
        ItemDescription = GameObject.Find("Item Stats Text").GetComponent<TextMeshProUGUI>();

        ItemName.text = consumable.ItemName;
        ItemDescription.text = consumable.Description;
        
    }

    void Update()
    {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {

        ItemTooltip.Instance.ShowTooltip(consumable);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ItemTooltip.Instance.HideTooltip();
    }
}
