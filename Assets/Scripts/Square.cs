using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public int index;
    public Vector2 coordinates;
    public Color color;

    public void SetCoordinates(float x, float y)
    {
        coordinates.x = x;
        coordinates.y = y;
        transform.name = x + "," + y;
    }

    public void SetColor(string colorHex)
    {
        if (ColorUtility.TryParseHtmlString(colorHex, out var parsedColor))
        {
            color = parsedColor;
            GetComponent<SpriteRenderer>().color = color;
        }
    }
}
[Serializable]
public class SquareInfo
{
    public string randomColor;
    public float percentage;

    public SquareInfo(string randColor, float percent)
    {
        this.randomColor = randColor;
        this.percentage = percent;
    }
}