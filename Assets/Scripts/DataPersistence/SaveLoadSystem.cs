using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{ 
    public static void NewGame()
    {
        SaveGame(new GameData());
    }

    public static GameData LoadGame()
    {
        //Load saved data
        string path = Application.persistentDataPath + "/player.saves";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData saveLoaded = formatter.Deserialize(stream) as GameData;
            stream.Close();

            return saveLoaded;

        }else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void SaveGame(GameData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.saves";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }
}
