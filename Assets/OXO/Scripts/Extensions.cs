using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static void ColoredLog(string text,Color color)
    {
        Debug.Log($"<color=orange><b>(!) {text} </b> </color>");
    }
}
