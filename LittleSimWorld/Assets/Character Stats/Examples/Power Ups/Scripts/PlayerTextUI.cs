using UnityEngine;
using UnityEngine.UI;

namespace CharacterStats
{
	public class PlayerTextUI : MonoBehaviour
	{
		[SerializeField] Player player;
		[SerializeField] Text speedText;
		[SerializeField] Text jumpText;

		void OnValidate()
		{
			if (player == null)
				player = FindObjectOfType<Player>();
		}

		void Update()
		{
			speedText.text = player.MovementSpeed.Value.ToString();
			jumpText.text = player.JumpForce.Value.ToString();
		}
	}
}
