using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

//Work in progress, does nothing at the moment

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    // Update is called once per frame
    void Update()
    {
        //Test key to save
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Save();
        }
    }

    private void Save()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "test.dat", FileMode.OpenOrCreate);

            SaveData data = new SaveData();

            SavePlayer(data);
            bf.Serialize(file, data);

            file.Close();
        }
        catch(System.Exception)
        {
            //Handles Errors
        }
    }

    private void SavePlayer(SaveData data)
    {
        data.MyPlayerData = new PlayerData();
    }

    private void Load()
    {
        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + "test.dat", FileMode.Open);

            SaveData data =  (SaveData)bf.Deserialize(file);

            file.Close();
            LoadPlayer(data);
        }
        catch (System.Exception)
        {
            //Handles Errors
        }
    }

    private void LoadPlayer(SaveData data)
    {

    }
}
