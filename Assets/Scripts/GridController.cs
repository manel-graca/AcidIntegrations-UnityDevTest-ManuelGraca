using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GridController : MonoBehaviour
{
    public static GridController instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private Transform gridParent;
    [SerializeField] private GameObject squarePrefab;

    private Button startButton;

    private int width = 10;
    private int height = 10;

    private List<Square> squares = new List<Square>();

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        startButton = UIManager.instance.startButton;
        if (startButton != null) startButton.onClick.AddListener(GenerateGrid);
    }

    public List<int> GetGridSize()
    {
        var size = new List<int>{width,height};
        return size;
    }

    private void GenerateGrid()
    {
        var index = 0;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                index++;
                var obj = Instantiate(squarePrefab, new Vector3(x, y, 0f), Quaternion.identity);
                var squareClass = obj.GetComponent<Square>();
                squares.Add(squareClass);
                squareClass.transform.SetParent(gridParent);
                squareClass.SetCoordinates(x, y);
                squareClass.index = index;
            }
        }

        RotateGrid();
        ApplyPercentToCubes();
        UIManager.instance.startButton.gameObject.SetActive(false);
    }

    private void RotateGrid()
    {
        var rotation = new Vector3(0f, 0f, 45f);
        var gridParentTransform = gridParent.transform;
        gridParentTransform.localRotation = Quaternion.Euler(rotation);
        gridParentTransform.localPosition =
            new Vector3(gridParentTransform.position.x + 5, gridParentTransform.position.y, 0f);
        cam.transform.position = new Vector3(0f, 6.5f, -10f);
    }

    private void ApplyPercentToCubes()
    {
        var squaresInformation = WebRequestsManager.instance.RetrieveSquaresData(squares.Count);

        Debug.Log(squaresInformation.Count + " count squares info");

        for (int i = 0; i < squares.Count; i++)
        {
            for (int j = 0; j < squaresInformation.Count; j++)
            {
                squares[i].SetColor(squaresInformation[squares[i].index - 1].randomColor);
            }
        }
    }

}