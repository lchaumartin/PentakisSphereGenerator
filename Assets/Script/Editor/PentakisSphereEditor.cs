using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PentakisSphere))]
public class PentakisSphereEditor : Editor
{
    SerializedProperty radius;
    SerializedProperty resolution;

    GUIStyle titleStyle;

    private void OnEnable()
    {
        radius = serializedObject.FindProperty("radius");
        resolution = serializedObject.FindProperty("resolution");
        titleStyle = new GUIStyle();
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.normal.textColor = Color.white;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.fontSize = 16;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Pentakis Sphere Generator", titleStyle);
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(radius);
        EditorGUILayout.PropertyField(resolution);
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("Update Mesh", GUILayout.Height(24)))
        {
            ((PentakisSphere)(serializedObject.targetObject)).Generate();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.LabelField("Pentakis Sphere Generator", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.LabelField("© 2022 Léo Chaumartin", EditorStyles.centeredGreyMiniLabel);

        serializedObject.ApplyModifiedProperties();
    }
}
