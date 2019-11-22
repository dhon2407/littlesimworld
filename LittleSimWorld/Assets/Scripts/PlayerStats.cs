using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int currentLevelVit;

    public int currentLevelInt;

    public int currentExpInt;

    public int currentExpVit;

    public int[] toLevelUpVit;

    public int[] toLevelUpInt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(currentExpVit >= toLevelUpVit[currentLevelVit])
        {
            currentLevelVit++;
        }
        if(currentExpInt >= toLevelUpInt[currentLevelInt])
        {
            currentLevelInt++;
        }
    }

    public void AddExpVit(int expToAddVit)
    {
        //currentExpVit += expToAddVit;
    }

    public void AddExpInt(int expToAddInt)
    {
        currentExpInt += expToAddInt;
    }
}
