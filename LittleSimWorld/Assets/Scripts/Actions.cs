using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Timers;
using UnityEngine.Events;

public class Actions : MonoBehaviour
{
    public float progressSpeed;
    public GameObject floatingText;
    public static Actions Instance;

   
    private void Awake()
    {
        if(!Instance)
        Instance = this;
    }
    private void Start()
    {

    }
    public void WorkOut()
    {
        PlayerStatsManager.Strength.Instance.AddXP(10);
        PlayerStatsManager.Instance.AddEnergy(-10);
        GameLibOfMethods.AddChatMessege("You lifted weights and got 10 strength XP.");
    }
    public void RunOnTreadmill()
    {
        PlayerStatsManager.Fitness.Instance.AddXP(10);
        PlayerStatsManager.Instance.AddEnergy(-10);
        GameLibOfMethods.AddChatMessege("You've runned on treadmill and got 10 fitness XP.");
    }

    public void TakeShower()
    {
        PlayerStatsManager.Instance.AddHygiene(100);
        GameLibOfMethods.player.transform.position = GameLibOfMethods.TempPos;
    }
    public void TakeADump()
    {
        PlayerStatsManager.Instance.AddBladder(100);
    }

    public void Study()
    {
        

                        PlayerStatsManager.Intelligence.Instance.AddXP(10);
                        PlayerStatsManager.Instance.SubstractEnergy(10);

        GameLibOfMethods.AddChatMessege("You studied and got 10 Inteligence XP.");




    }

   

    public IEnumerator FloatingMessage(string text)
    {
        while (true)
        {
            
            GameObject FloatingText = Instantiate(floatingText, GameLibOfMethods.player.transform.position, GameLibOfMethods.player.transform.rotation);
            FloatingText.GetComponent<TextMeshPro>().text = text;
            FloatingText.transform.position = new Vector3(FloatingText.transform.position.x, FloatingText.transform.position.y + Time.deltaTime);
            Destroy(FloatingText, 2);
            yield return new WaitForFixedUpdate();
        }
       
    }

}
  
