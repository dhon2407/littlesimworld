using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;

namespace CharacterStats
{
	public class ItemTooltip : MonoBehaviour
	{
		public static ItemTooltip Instance;
        public GuiPopUpAnim PopUpScript;
        [SerializeField] TextMeshProUGUI nameText;
		[SerializeField] TextMeshProUGUI slotTypeText;
		[SerializeField] TextMeshProUGUI statsText;
        public float MinWidth = 0.1f;
        public float MaxWidth = 0.92f;
        public float MinHeight = 0.2f;
        public float MaxHeight = 1;
        public Image image;

		private StringBuilder sb = new StringBuilder();

		private void Awake()
		{
			if (Instance == null) {
				Instance = this;
			} else {
				Destroy(this);
			}
		}
        private void Start()
        {
           // HideTooltip();
        }


        private void Update()
        {

            transform.position = new Vector2(Mathf.Clamp(Input.mousePosition.x, Screen.width * MinWidth, Screen.width  * MaxWidth),
                Mathf.Clamp(Input.mousePosition.y, Screen.height * MinHeight, Screen.height * MaxHeight));
        }
        public void ShowTooltip(Item item)
		{
            transform.position = new Vector2(Mathf.Clamp(Input.mousePosition.x, Screen.width * MinWidth, Screen.width * MaxWidth),
                Mathf.Clamp(Input.mousePosition.y, Screen.height * MinHeight, Screen.height * MaxHeight));

            //ConsumableItem item = FindObjectOfType<AtommInventory>().inventory[inventoryNumber].consumableItem;



            PopUpScript.OpenWindow();

            nameText.text = item.ItemName;
            slotTypeText.text = "Consumable";

			sb.Length = 0;

            statsText.text = item.Description;
			/*AddStatText(item.Energy, " Energy");
			AddStatText(item.Health, " Health");
			AddStatText(item.Hunger, " Hunger");
			AddStatText(item.Mood, " Mood");

			AddStatText(item.EnergyPercentBonus * 100, "% Energy");
			AddStatText(item.HealthPercentBonus * 100, "% Health");
			AddStatText(item.HungerPercentBonus * 100, "% Hunger");
			AddStatText(item.MoodPercentBonus * 100, "% Mood");

			statsText.text = sb.ToString();*/

            
		}

		public void HideTooltip()
		{
            PopUpScript.CloseWindow();
        }

		private void AddStatText(float statBonus, string statName)
		{
			if (statBonus != 0)
			{
				if (sb.Length > 0)
					sb.AppendLine();

				if (statBonus > 0)
					sb.Append("+");

				sb.Append(statBonus);
				sb.Append(statName);
			}
		}
	}
}
