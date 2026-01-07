using System.Collections.Generic;
using UnityEngine;

public static class KeyManager
{
    private static List<char> greyList = new();
    private static List<char> yellowList = new();
    private static List<char> greenList = new();

    public static void AddGrey(char c)
    {
        greyList.Add(c);
    }

    public static void AddYellow(char c)
    {
        yellowList.Add(c);
    }
    public static void AddGreen(char c)
    {
        if (yellowList.Contains(c)) yellowList.Remove(c);
        greenList.Add(c);
    }

    public static void ResetKey()
    {
        greyList.Clear();
        yellowList.Clear();
        greenList.Clear();
    }


    public static ColorType CheckKeyColor(char c)
    {

        if (greenList.Contains(c)) return ColorType.Green;
        else if (yellowList.Contains(c)) return ColorType.Yellow;
        else if (greyList.Contains(c)) return ColorType.Grey;
        else return ColorType.White;
    }

}
