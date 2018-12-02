using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tools : MonoBehaviour
{
    [MenuItem("Tools/Clear Data &q")]
    static void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }

    static int index = 0;

    [MenuItem("Tools/Screenshot &s")]
    static void Screenshot()
    {
        index++;
        ScreenCapture.CaptureScreenshot("Assets/Visuals/OutOfGame/Thumbnails/Screenshot" + index + ".png");
    }
}