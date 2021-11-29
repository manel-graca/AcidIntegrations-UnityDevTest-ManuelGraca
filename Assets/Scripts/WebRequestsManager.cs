using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestsManager : MonoBehaviour
{
    public static WebRequestsManager instance;

    private SquareInfo[] squareInfoCollection;
    private const string requestURL = "https://hotelapi.eastus.cloudapp.azure.com/";

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(GetCubesInfoRoutine());
    }

    IEnumerator GetCubesInfoRoutine()
    {
        UnityWebRequest request = UnityWebRequest.Get(requestURL);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            var downloadedData = request.downloadHandler.data;
            var json = Encoding.ASCII.GetString(downloadedData);
            JsonUtility.ToJson(json);
            squareInfoCollection = JsonHelper.GetJsonArray<SquareInfo>(json);
        }
        else
        {
            Debug.Log(request.error);
        }
    }

    public List<SquareInfo> RetrieveSquaresData(int squaresAmount)
    {
        var grid = GridController.instance;
        var width = grid.GetGridSize()[0];
        var height = grid.GetGridSize()[1];
        var totalSize = width * height;

        
        var finalCollection = new List<SquareInfo>();

        // I FLOOR THE NUMBER SO BELOW I CAN ACCURATELY POPULATE THE LIST WITH THE MISSING MEMBER(S)
        float numOfEachDifObjects = (float) squaresAmount / squareInfoCollection.Length;
        numOfEachDifObjects = Mathf.FloorToInt(numOfEachDifObjects);

        Debug.Log(numOfEachDifObjects);

        foreach (var s in squareInfoCollection)
        {
            for (int i = 0; i < numOfEachDifObjects; i++)
            {
                finalCollection.Add(s);
            }
        }

        // IN CASE THE INPUT IS LESS THAN SQUARES COUNT
        if (finalCollection.Count < totalSize)
        {
            while (finalCollection.Count < totalSize)
            {
                var obj = finalCollection[finalCollection.Count - 1];
                finalCollection.Add(obj);
                Debug.Log(finalCollection.Count);
            }
        }

        return finalCollection;
    }
}