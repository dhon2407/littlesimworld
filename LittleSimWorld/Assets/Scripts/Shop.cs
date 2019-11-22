using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterStats;
public class Shop : MonoBehaviour
{
    public List<Item> SellingItems;

    [HideInInspector]
    public List<AtommInventory.Purchasable> purchasableItems;

    public AudioClip open, close;

    public GameObject ShopOptionsUI;

    public Transform playerWorkPlace;

    public string WorkingAnimationName;

    AudioSource source;

    public Collider2D ShopZone;

    public Outline shopOutline;

    public PlayerStatsManager playerStatsManager;

    public enum JobType
    {
        CookingJob
    }
    public JobType jobType = JobType.CookingJob;

    private void OnValidate()
    {
        /*purchasableItems.Clear();
        foreach(Item item in SellingItems)
        {
            AtommInventory.Purchasable purchasable = new AtommInventory.Purchasable();
            purchasable.SetPlayerStatsManager(playerStatsManager);
            purchasable.InitializePurchasable(item);
            purchasableItems.Add(purchasable);
            //purchasableItems.Add(new AtommInventory.Purchasable(item));
        }*/
    }

    private void Start()
    {
        source = GetComponent<AudioSource>();
        shopOutline = gameObject.GetComponent<Outline>();
    }
    private void Update()
    {
        if (ShopOptionsUI)
        {
            if ( !shopOutline.Enabled|| GameLibOfMethods.doingSomething || !GameLibOfMethods.canInteract || GameLibOfMethods.cantMove)
            {
                ShopOptionsUI.SetActive(false);
            }
        }
          
    }

    public void Action()
    {
        if (source.clip == open)
        { source.clip = close; source.Play(); }
        else
        { source.clip = open; source.Play(); }
    }
    public void OpenThisShop()
    {
        AtommInventory.Instance.ShopActive(this);
    }

    public void WorkHere()
    {
        GameLibOfMethods.TempPos = GameLibOfMethods.player.transform.position;
        GameLibOfMethods.cantMove = true;
        GameLibOfMethods.canInteract = false;
        switch (jobType)
        {
            case JobType.CookingJob:
                JobsPopUp.Instance.SetJobToCooker();
                StartCoroutine(InteractionChecker.Instance.JumpTo(playerWorkPlace.position, delegate { StartCoroutine(JobsPopUp.Instance.DoJob("Cooking")); }));
                break;
        }
        
        
    }
}