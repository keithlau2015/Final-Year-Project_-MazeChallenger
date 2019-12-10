
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void SavingData(OpeningVideo ov)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/untitled.player";
        FileStream stream = new FileStream(path, FileMode.Create);

        OpenGameCounting data = new OpenGameCounting(ov);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static OpenGameCounting LoadData()
    {
        string path = Application.persistentDataPath + "/untitled.player";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            OpenGameCounting data = formatter.Deserialize(stream) as OpenGameCounting;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
