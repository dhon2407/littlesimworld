using UnityEngine;
using UnityEngine.UI;
using System.Text;
using TMPro;

namespace CharacterStats
{

    public class StatsTooltip : MonoBehaviour

    {
        public static StatsTooltip Instance;
        public GuiPopUpAnim PopUpScript;
        public Image image;
        public float MinWidth = 0.1f;
        public float MaxWidth = 0.92f;
        public float MinHeight = 0.2f;
        public float MaxHeight = 1;
        [SerializeField] TextMeshProUGUI statsText;


        private void Awake()
        {
           
        }
        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
            //PopUpScript.CloseWindow();
            if (!PopUpScript)
            {
                PopUpScript = gameObject.GetComponent<GuiPopUpAnim>();
            }

            //HideDescription();
        }

        private void LateUpdate()
        {
            transform.position = new Vector2 (Mathf.Clamp(Input.mousePosition.x , Screen.width * MinWidth, Screen.width * MaxWidth) ,
                Mathf.Clamp(Input.mousePosition.y, Screen.height * MinHeight, Screen.height * MaxHeight));
        }


        public void ShowDescription(string Description)
        {


            //ConsumableItem item = FindObjectOfType<AtommInventory>().inventory[inventoryNumber].consumableItem;

            transform.position = new Vector2(Mathf.Clamp(Input.mousePosition.x, Screen.width * MinWidth, Screen.width * MaxWidth),
                 Mathf.Clamp(Input.mousePosition.y, Screen.height * MinHeight, Screen.height * MaxHeight));

            PopUpScript.OpenWindow();

            statsText.text = Description;

        }

        public void HideDescription()
        {
            PopUpScript.CloseWindow();
        }
    }
}