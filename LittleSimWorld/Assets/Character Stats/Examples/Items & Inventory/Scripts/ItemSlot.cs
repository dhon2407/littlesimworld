using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CharacterStats;


[System.Serializable]
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image image;
    public Item itemSO;
    public float ID;



    // For Custom ToolTip
    public MouseOverDetails mOverDetails;

    public bool isShop;


    private Item _item;
    public Item Item
    {
        get { return _item; }
        set
        {
            _item = value;

            if (_item == null)
            {
                image.enabled = false;
            }
            else
            {
                image.sprite = Resources.Load<Sprite>(_item.iconPath);
                image.enabled = true;
            }
        }
    }

    /*public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item != null && OnRightClickEvent != null)
                OnRightClickEvent(Item);
        }
    }*/

    private void Start()
    {
        if (isShop)
        {
            mOverDetails = FindObjectOfType<MouseOverDetails>();
        }
    }

    private void Update()
    {
        if (CooldownManager.ItemsOnCooldown.Contains(itemSO))
        {
            image.fillAmount = itemSO.currentCooldown / itemSO.MaxCooldown;
        }
    }

    protected virtual void OnValidate()
    {
        if (image == null)
            image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (isShop)
        {
            ShoppingItem si = GetComponent<ShoppingItem>();
            mOverDetails.UpdateDetails(itemSO.ItemName, "Ingredient: Food", itemSO.Description, si.icon.sprite);
        }
        else
        {
            ItemTooltip.Instance.ShowTooltip(itemSO);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isShop)
        {
            mOverDetails.Reset();
        }
        else
        {
            ItemTooltip.Instance.HideTooltip();
        }


    }
    private void OnDestroy()
    {

        if (isShop)
        {
            mOverDetails.Reset();
        }
        else
        {
            ItemTooltip.Instance.HideTooltip();
        }
    }
    private void OnDisable()
    {
        if (isShop)
        {
            mOverDetails.Reset();
        }
        else
        {
            ItemTooltip.Instance.HideTooltip();
        }

    }
}

