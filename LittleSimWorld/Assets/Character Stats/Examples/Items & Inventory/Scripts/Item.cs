using UnityEngine;


	[CreateAssetMenu][System.Serializable]
	public class Item : ScriptableObject
	{
		public string ItemName;
		public string iconPath;
        public float MaxCooldown;
        public float currentCooldown = 0;
        public int maxAmount = 1;
    public bool stackableItem;
    [Space]
    public AudioClip UsageSound;
    public string Description;
    public int price;
    public int sellingQuantity = 1;
    public float UsageTime = 10;
    public string AnimationToPlayName = "Eating";
    [Space]
    public Item CooksInto;



    public virtual bool Use(float ItemID)

    {
        return true;
    }


    }
