using UnityEngine;
using System.Collections;

public class TestStart : MonoBehaviour
{
    private MissionHandler mh;

    void Start()
    {
        mh = GameObject.Find("MissionHandler").GetComponent<MissionHandler>();
        mh.RandomizeMissions();
        mh.ShowMissions();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            mh.HideMissions();

            Application.LoadLevel(1);
        }
    }
}