using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private TextMeshProUGUI infoMessageText;

    public Button submitButton;
    public Button startButton;
    public Button restartButton;
    
    private Square[] squares;
    private string rawUserInput;
    private int inputX;
    private int inputY;

    GridController grid;
    
    private void Start()
    {
        grid = GridController.instance;
        
        restartButton.onClick.AddListener(RestartScene);
        submitButton.onClick.AddListener(GetStringInput);
        displayText.text = String.Empty;
        infoMessageText.text = String.Empty;
    }
    public void UpdateDisplayText(string text)
    {
        displayText.text = text;
    }

    public void ThrowInfoMessage(string message)
    {
        infoMessageText.text = message;
    }

    void GetSquaresInScene()
    {
        squares = FindObjectsOfType<Square>();
    }

    void GetStringInput()
    {
        infoMessageText.text = String.Empty;
        GetSquaresInScene();
        rawUserInput = inputField.text;
        InputHandler.instance.ProcessUserInput(rawUserInput);
    }

    private void RestartScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
    
    private Square GetTargetSquare(int x, int y)
    {
        Square targetSquare = null;
        foreach (var sq in squares)
        {
            if ((int)sq.coordinates.x == x && (int)sq.coordinates.y == y)
            {
                targetSquare = sq;
            }
        }
        return targetSquare;
    }

    public void DisplayCubeColor(int x, int y)
    {
        var hex = String.Empty;
        foreach (var sq in squares)
        {
            if ((int)sq.coordinates.x == x && (int)sq.coordinates.y == y)
            {
                hex = ColorUtility.ToHtmlStringRGB(sq.color);
            }
        }
        UpdateDisplayText($"The cube in ({ x + "," + y}) is <color=#{hex}>#{hex}</color>");
    }

    public void DisplayPercentOfCubesColor(Color color)
    {
        float cubesSharingColor = 0;
        foreach (var cube in squares)
        {
            if (cube.color == color)
            {
                cubesSharingColor += 1;
            }
        }
        float percentage = ((cubesSharingColor / squares.Length) * 100f) / 100f;
        displayText.text = $"There are {percentage * 100f}% of cubes with that color";
        Debug.Log(percentage);
    }
    
    public void DisplayNeighbourCubesColors(int x, int y)
    {
        var neighbours = new List<Square>();
        string hex = String.Empty;
        Square targetSquare = GetTargetSquare(x,y);

        foreach (var sq in squares)
        {
            if (Vector2.Distance(targetSquare.transform.localPosition, sq.transform.localPosition) < 1.5f)
            {
                if (sq != targetSquare)
                {
                    neighbours.Add(sq);
                }
            }
        }
        displayText.text = $"The cube in ({x + "," + y}) has {neighbours.Count} neighbours.\n";
        for (int i = 0; i < neighbours.Count; i++)
        {
            hex = ColorUtility.ToHtmlStringRGB(neighbours[i].color);
            displayText.text += $"\nNeighbour {i+1} in ({neighbours[i].coordinates.x + "," + neighbours[i].coordinates.y}) is <color=#{hex}>#{hex}</color>";
        }
        Debug.Log(neighbours.Count);
    }

    public void DisplayMirroredCubeColorX(int x, int y)
    {
        int width = grid.GetGridSize()[0] - 1;
        Square targetSquare = GetTargetSquare(x,y);
        
        var targetX = (int)targetSquare.coordinates.x;
        var targetY = (int)targetSquare.coordinates.y;

        if (targetX == 5)
        {
            foreach (var sq in squares)
            {
                if ((int)sq.coordinates.x == targetX - 1 && (int)sq.coordinates.y == targetY)
                {
                    string hex = String.Empty;
                    hex = ColorUtility.ToHtmlStringRGB(sq.color);
                    displayText.text = $"({sq.coordinates.x},{sq.coordinates.y}) - <color=#{hex}>#{hex}</color>";
                }
            }
        }

        if (targetX <= 4 && targetX >= 0)
        {
            foreach (var sq in squares)
            {
                if ((int)sq.coordinates.x == width - targetX && (int)sq.coordinates.y == targetY)
                {
                    string hex = String.Empty;
                    hex = ColorUtility.ToHtmlStringRGB(sq.color);
                    displayText.text = $"({sq.coordinates.x},{sq.coordinates.y}) - <color=#{hex}>#{hex}</color>";
                }
            }
        }
        if (targetX >= 6 && targetX <= width)
        {
            foreach (var sq in squares)
            {
                if ((int)sq.coordinates.x == width - targetX && (int)sq.coordinates.y == targetY)
                {
                    string hex = String.Empty;
                    hex = ColorUtility.ToHtmlStringRGB(sq.color);
                    displayText.text = $"({sq.coordinates.x},{sq.coordinates.y}) - <color=#{hex}>#{hex}</color>";
                }
            }
        }
    }
    public void DisplayMirroredCubeColorY(int x, int y)
    {
        int height = grid.GetGridSize()[1] - 1;
        Square targetSquare = GetTargetSquare(x,y);
        
        var targetX = (int)targetSquare.coordinates.x;
        var targetY = (int)targetSquare.coordinates.y;
        
        if (targetY == 5)
        {
            foreach (var sq in squares)
            {
                if ((int)sq.coordinates.y == targetY - 1 && (int)sq.coordinates.x == targetX)
                {
                    string hex = String.Empty;
                    hex = ColorUtility.ToHtmlStringRGB(sq.color);
                    displayText.text = $"({sq.coordinates.x},{sq.coordinates.y}) - <color=#{hex}>#{hex}</color>";
                }
            }
        }

        if (targetY <= 4 && targetY >= 0)
        {
            foreach (var sq in squares)
            {
                if ((int)sq.coordinates.y == height - targetY && (int)sq.coordinates.x == targetX)
                {
                    string hex = String.Empty;
                    hex = ColorUtility.ToHtmlStringRGB(sq.color);
                    displayText.text = $"({sq.coordinates.x},{sq.coordinates.y}) - <color=#{hex}>#{hex}</color>";
                }
            }
        }
        if (targetY >= 6 && targetY <= height)
        {
            foreach (var sq in squares)
            {
                if ((int)sq.coordinates.y == height - targetY && (int)sq.coordinates.x == targetX)
                {
                    string hex = String.Empty;
                    hex = ColorUtility.ToHtmlStringRGB(sq.color);
                    displayText.text = $"({sq.coordinates.x},{sq.coordinates.y}) - <color=#{hex}>#{hex}</color>";
                }
            }
        }
    }

    

    
}
