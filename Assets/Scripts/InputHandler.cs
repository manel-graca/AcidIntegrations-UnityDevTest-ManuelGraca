using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InputHandler : MonoBehaviour
{
    public static InputHandler instance;
    
    private string inputHistory;
    private string processedInput;
    
    private UIManager uiManager;
    private FilesManager filesManager;
    
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CacheReferences();
    }

    private void CacheReferences()
    {
        uiManager = FindObjectOfType<UIManager>();
        filesManager = FilesManager.instance;
    }
    public void ProcessUserInput(string rawInput)
    {
        processedInput = ProcessTextToCapital(rawInput);
        HandleProcessedInput();

        if (!String.IsNullOrEmpty(inputHistory))
        {
            inputHistory += Environment.NewLine + processedInput;
        }
        else
        {
            inputHistory = processedInput;
        }
    }
    private void HandleProcessedInput()
    {
        // CASE (X,Y)
        if (processedInput.Length == 3)
        {
            var nums = processedInput.ToCharArray();
            int x; 
            int y;
            string sX = nums[0].ToString();
            string sY = nums[2].ToString();
            if (nums[1].ToString() == ",")
            {
                if (Int32.TryParse(sX, out x))
                {
                    if (Int32.TryParse(sY, out y))
                    {
                        uiManager.DisplayCubeColor(x,y);
                    }
                }
            }
            return;
        }
        // CASE (#XXXX)
        if (ColorUtility.TryParseHtmlString(processedInput, out var tempColor)) 
        {
            uiManager.DisplayPercentOfCubesColor(tempColor);
            return;
        }

        // CASE N X,Y
        if (processedInput.Length == 5 && processedInput[0].ToString() == "N" && processedInput[3].ToString() == ",") 
        {
            string sX = processedInput[2].ToString();
            string sY = processedInput[4].ToString();

            int x;
            int y;
            if (Int32.TryParse(sX, out x))
            {
                if(Int32.TryParse(sY, out y))
                {
                    uiManager.DisplayNeighbourCubesColors(x,y);
                }        
            }
            return;
        }

        // CASE MX X,Y
        if (processedInput.Length == 6 && processedInput[0].ToString() == "M" &&
            processedInput[1].ToString() == "X" &&
            processedInput[4].ToString() == ",")
        {
            string sX = processedInput[3].ToString();
            string sY = processedInput[5].ToString();

            int x;
            int y;
            if (Int32.TryParse(sX, out x))
            {
                if(Int32.TryParse(sY, out y))
                {
                    uiManager.DisplayMirroredCubeColorX(x,y);
                }        
            }
            return;
        }
        
        // CASE MY X,Y
        if (processedInput.Length == 6 && processedInput[0].ToString() == "M" &&
            processedInput[1].ToString() == "Y" &&
            processedInput[4].ToString() == ",")
        {
            string sX = processedInput[3].ToString();
            string sY = processedInput[5].ToString();

            int x;
            int y;
            if (Int32.TryParse(sX, out x))
            {
                if(Int32.TryParse(sY, out y))
                {
                    uiManager.DisplayMirroredCubeColorY(x,y);
                }        
            }
            return;
        }

        if (processedInput == "LOGS")
        {
            FilesManager.instance.CreateLogsFile(inputHistory);
            uiManager.UpdateDisplayText(filesManager.LoadLogsFile());
            return;
        }
        if (processedInput == "DELETE")
        {
            FilesManager.instance.DeleteFromLogs();
            return;
        }
        
        uiManager.ThrowInfoMessage("Command not valid");

    }

    private string ProcessTextToCapital(string input)
    {
        var chars = input.ToCharArray();

        var processedString = string.Empty;
        int i = 0;
        foreach(var a in chars)
        {
            var upperChar = char.ToUpper(a);
            processedString = processedString.Insert(i, upperChar.ToString());
            i++;
        }
        return processedString;
    }

    

}
