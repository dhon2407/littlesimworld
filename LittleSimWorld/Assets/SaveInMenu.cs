using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Linq;
public class SaveInMenu : MonoBehaviour
{
    public TextMeshProUGUI saveNameFrom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadThisSave()
    {
        GameManager.Instance.CurrentSaveName = saveNameFrom.text;
        MainMenu.Instance.LoadMainSceneGame(1);
    }
    public void DeleteThisSave()
    {
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath);
        var files = di.GetFiles().Where(obj => obj.Name.EndsWith(saveNameFrom.text + ".save"));
        foreach(FileInfo file in files)
        {
            file.Delete();
        }
        MainMenu.Instance.LoadSaves();
    }
}
