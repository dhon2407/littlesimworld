using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private int gold = 0;

    private int time = 0;

    private int collect = 0;

    private bool highscore = false;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.SetInt("gold", gold);
            PlayerPrefs.SetInt("collect", collect);
            PlayerPrefs.SetInt("time", time);
            PlayerPrefs.SetInt("highscore", highscore ? 1 : 0);
            PlayerPrefs.Save();

            Application.LoadLevel(2);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            gold += 10;
            
            GameObject.Find("Gold").GetComponent<Text>().text = "Press G to increase gold: " + gold + " gold";
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            time += 10;

            GameObject.Find("Time").GetComponent<Text>().text = "Press T to increase time: " + time + "s";
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            collect++;

            GameObject.Find("Collect").GetComponent<Text>().text = "Press C to collect something: " + collect;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            highscore = true;

            GameObject.Find("HighScore").GetComponent<Text>().text = "Press H for Highscore: " + highscore;
        }
    }
}