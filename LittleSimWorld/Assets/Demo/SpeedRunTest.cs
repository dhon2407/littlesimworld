using UnityEngine;
using System.Collections;

public class SpeedRunTest : MonoBehaviour, IMissionCheck
{
    bool IMissionCheck.MissionStatus()
    {
        return PlayerPrefs.GetInt("time") <= 30;
    }
}