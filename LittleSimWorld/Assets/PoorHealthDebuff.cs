using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoorHealthDebuff : Buffs
{
    public static PoorHealthDebuff poorHealthDebuffInstance;
    // Start is called before the first frame update
    void Start()
    {
        maxDuration = 999999;
        PlayerStatsManager.Instance.MovementSpeed -= MovementSpeedModifier;
        poorHealthDebuffInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerStatsManager.Instance.Health / PlayerStatsManager.Instance.MaxHealth > MinHealthPercentageForPoorHealth || VeryPoorHealthDebuff.veryPoorHealthDebuffInstance)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        PlayerStatsManager.Instance.MovementSpeed += MovementSpeedModifier;
    }
}
