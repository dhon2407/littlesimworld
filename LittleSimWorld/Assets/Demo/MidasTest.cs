using UnityEngine;
using System.Collections;

public class MidasTest : MonoBehaviour, IMissionCheck
{
    bool IMissionCheck.MissionStatus()
    {
        return PlayerPrefs.GetInt("gold") >= 20;
    }
}