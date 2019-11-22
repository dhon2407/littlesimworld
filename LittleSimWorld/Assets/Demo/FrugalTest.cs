using UnityEngine;
using System.Collections;

public class FrugalTest : MonoBehaviour, IMissionCheck
{
    bool IMissionCheck.MissionStatus()
    {
        return PlayerPrefs.GetInt("collect") == 0 && PlayerPrefs.GetInt("gold") == 0;
    }
}