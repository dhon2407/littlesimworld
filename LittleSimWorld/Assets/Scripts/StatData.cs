using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
namespace CharacterStats
{
    public class StatData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        [SerializeField] string description;
        public bool showXp;
        public TextMeshProUGUI XpText;
      
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (showXp)
                StatsTooltip.Instance.ShowDescription(description + XpText.text);
            else
                StatsTooltip.Instance.ShowDescription(description);
        }
        private void Update()
        {
            
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            StatsTooltip.Instance.HideDescription();
        }
        private void OnDestroy()
        {
            StatsTooltip.Instance.HideDescription();
        }
    }
}