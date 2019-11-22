using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AfterLevel : MonoBehaviour {
    private MissionHandler mh;

    void Start () {
        mh = GameObject.Find("MissionHandler").GetComponent<MissionHandler>();

        mh.CheckMissions();
        mh.ShowMissions();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            mh.CullAccomplished();
            mh.HideMissions();

            SceneManager.LoadScene(0);
        }
    }
}