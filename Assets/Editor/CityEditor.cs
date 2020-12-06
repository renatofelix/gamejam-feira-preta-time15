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
        base.OnInspectorGUI();
        GenerateCity city = target as GenerateCity;

    }

}
