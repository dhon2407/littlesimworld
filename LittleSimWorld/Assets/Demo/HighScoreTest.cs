using UnityEngine;
using System.Collections;

public class HighScoreTest : MonoBehaviour, IMissionCheck
{
    bool IMissionCheck.MissionStatus()
    {
        return PlayerPrefs.GetInt("highscore") == 1;
    }
}