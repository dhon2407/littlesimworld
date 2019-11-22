using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buffs : MonoBehaviour
{
    public float XPmodifier = 0;
    public float MovementSpeedModifier = 0;
    public float maxDuration = 10;
    public float currentDuration = 0;
    public Image CooldownImage;
    public GameObject poorHealthDebuff;
    public GameObject veryPoorHealthDebuff;
    public float MinHealthPercentageForPoorHealth = 0.5f;
    public float MinHealthPercentageForVeryPoorHealth = 0.25f;

    public static GameObject Instance;

    private void Awake()
    {
        Instance = gameObject;
    }
    void Start()
    {
        
    }

    void Update()
    {
        Instance = gameObject;
        if (PlayerStatsManager.Instance.Health / PlayerStatsManager.Instance.MaxHealth < MinHealthPercentageForPoorHealth && !PoorHealthDebuff.poorHealthDebuffInstance && !VeryPoorHealthDebuff.veryPoorHealthDebuffInstance)
        {
           Instantiate(poorHealthDebuff, Instance.transform);
        }
        if (PlayerStatsManager.Instance.Health / PlayerStatsManager.Instance.MaxHealth < MinHealthPercentageForVeryPoorHealth && !VeryPoorHealthDebuff.veryPoorHealthDebuffInstance)
        {
            Instantiate(veryPoorHealthDebuff, Instance.transform);
        }
    }
}
