using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class FilesManager : MonoBehaviour
{
    public static FilesManager instance;
    
    private const string savesFolder = "/Saves";

    private UIManager ui;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ui = UIManager.instance;
    }

    public void CreateLogsFile(string inputHistory)
    {
        File.WriteAllText(Application.dataPath + savesFolder + "/logs.txt", inputHistory);
        ui.ThrowInfoMessage("Logs file created");
    }

    public string LoadLogsFile()
    {
        var content = File.ReadAllText(Application.dataPath + savesFolder + "/logs.txt");
        return content;
    }

    public void DeleteFromLogs()
    {
        File.WriteAllText(Application.dataPath + savesFolder + "/logs.txt",string.Empty);
        ui.ThrowInfoMessage("Logs file content was deleted");
    }
    
}
// https://answers.unity.com/questions/1290561/how-do-i-go-about-deserializing-a-json-array.html
// The class below was copied from the link above. Had to do it because Unity JsonUtility apparently
// doesn't support json arrays.
// I've decided to make this workaround instead of using a third-party Json Library, because the test rules
// explicitly say that is NOT allowed the use of plug-ins.
public class JsonHelper
{
    public static T[] GetJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }
 
    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}