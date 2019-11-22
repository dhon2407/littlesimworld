using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeryPoorHealthDebuff : Buffs
{
    public static VeryPoorHealthDebuff veryPoorHealthDebuffInstance;
    // Start is called before the first frame update
    void Start()
    {
        maxDuration = 999999;
        PlayerStatsManager.Instance.MovementSpeed -= MovementSpeedModifier;
        veryPoorHealthDebuffInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStatsManager.Instance.Health / PlayerStatsManager.Instance.MaxHealth > MinHealthPercentageForVeryPoorHealth)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        PlayerStatsManager.Instance.MovementSpeed += MovementSpeedModifier;
    }
}
