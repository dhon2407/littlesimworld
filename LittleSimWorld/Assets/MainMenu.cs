using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;
using System.Linq;
using System;
public class MainMenu : MonoBehaviour
{

    public int MainGameSceneNumber;
    public GameObject loadingPanal;
    public Slider loadingSlider;
    public TextMeshProUGUI loadingText;
    public static MainMenu Instance;
    public GameObject saveGO;
    public Transform WhereToPasteSaveGOs;
    public Texture2D cursor;

    public LevelLoader ll;

    private void Awake()
    {
        Instance = this;
        cursorSet(cursor);
    }
    public void LoadMainSceneGame(int sceneIndex)
    {
        loadingPanal.SetActive(true);
        StartCoroutine(LoadAsyncronously(sceneIndex));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadSaves()
    {
        foreach(Transform child in WhereToPasteSaveGOs)
        {
            Destroy(child.gameObject);
        }
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);
        var files = di.GetFiles().Where(obj => obj.Name.EndsWith(".save"));
        
        foreach(FileInfo file in files)
        {
            

            Debug.Log(file.Name);
            GameObject save = Instantiate(saveGO as GameObject, WhereToPasteSaveGOs);
            save.name = file.Name.Replace(".save","");
            save.GetComponentInChildren<TextMeshProUGUI>().text = save.name;

            BinaryFormatter bf = new BinaryFormatter();

            FileStream fs = File.Open(file.FullName, FileMode.Open);
            fs.Position = 0;
            Save savefile = (Save)bf.Deserialize(fs);

            save.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Total time spent: "+ TimeSpan.FromSeconds(savefile.RealPlayTime).Hours.ToString() + " hours " + TimeSpan.FromSeconds(savefile.RealPlayTime).Minutes.ToString() + "  minutes.";
            string totalLvl = (
                savefile.charismaSkill.Level - 1
                + savefile.fitnesSkill.Level - 1
                + savefile.intelligenceSkill.Level - 1
                + savefile.strengthSkill.Level - 1
                + savefile.cookingSkill.Level - 1
                + savefile.repairSkill.Level - 1
                ).ToString();
            if(totalLvl == "0")
            save.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Total character level: " + "-";
            else
            {
                save.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Total character level: " + totalLvl;
            }


        }
    }
    IEnumerator LoadAsyncronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {

            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            loadingSlider.value = progress;
            loadingText.text = Mathf.Round((progress * 100f)) + "%";

            if (progress > 0.49)
            {
                loadingText.color = Color.white;
            }

            yield return null;
        }
    }
    void cursorSet(Texture2D tex)
    {
        CursorMode mode = CursorMode.Auto;
        var xspot = 0;
        var yspot = 0;
        Vector2 hotSpot = new Vector2(xspot, yspot);
        Cursor.SetCursor(tex, hotSpot, mode);
    }
}
