using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(GenerateCity))]
public class CityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Debug.Log("entrando no Editor");

        base.OnInspectorGUI();
        GenerateCity city = (GenerateCity)target;
        NewMethod(city);
    }

    private static void NewMethod(GenerateCity city)
    {
        city.GenerateMap();
    }
}
