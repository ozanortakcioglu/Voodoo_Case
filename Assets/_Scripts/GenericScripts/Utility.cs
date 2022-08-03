using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public static class Utility
{
    public static string FormatBigNumbers(int num)
    {
        if (num > 999999999 || num < -999999999)
        {
            return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999999 || num < -999999)
        {
            return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
        }
        else
        if (num > 999 || num < -999)
        {
            return num.ToString("0,.#K", CultureInfo.InvariantCulture);
        }
        else
        {
            return num.ToString(CultureInfo.InvariantCulture);
        }
    }

    public static int GetUniqueRandomIndex(int currentIndex, int min, int max)
    {
        int rand = Random.Range(min, max);
        while (rand == currentIndex)
        {
            rand = Random.Range(min, max);
        }
        return rand;
    }

    public static int GetNearestTen(int number)
    {
        return ((int)Mathf.Round(number / 10.0f)) * 10;
    }

    public static string DivideEnumToString(string toDivide)
    {
        string toReturn = "";
        string[] temp = Regex.Split(toDivide, @"(?<!^)(?=[A-Z])");

        foreach (var item in temp)
        {
            toReturn += item + " ";
        }
        return toReturn;
    }

    public static bool IsPointerOverUI_AnyCanvas()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}