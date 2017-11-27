using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StatControl))]
[CanEditMultipleObjects]
public class StatControlInspector : Editor
{
    StatControl sc;
    GUIStyle titleFormat;
    GUIStyle subtitleFormat;
    bool showHelp;

    void OnEnable()
    {
        sc = (StatControl)target;
        titleFormat = new GUIStyle();
        titleFormat.fontStyle = FontStyle.Bold;
        titleFormat.wordWrap = true;
        subtitleFormat = new GUIStyle();
        subtitleFormat.wordWrap = true;
    }
    public override void OnInspectorGUI()
    {
        ShowValues();
    }

    void ShowValues()
    {
        EditorGUILayout.LabelField("Stat controller", titleFormat);
        if(sc.CurrentStats != null)
        {
            sc.CurrentStats.SetLevel(EditorGUILayout.IntSlider("Level", sc.CurrentStats[StatType.LEVEL], 0, 100));
            if(GUI.changed)
            {
                //We should load the default stats for the selected level.
            }

            if (GUILayout.Button("Show help")) showHelp = !showHelp;

            if (showHelp) EditorGUILayout.HelpBox("Strength affects the damage of blunt attacks", MessageType.Info);
            sc.CurrentStats.SetStrength(EditorGUILayout.IntSlider("Strength", sc.CurrentStats[StatType.STRENGTH], 0, 100));
            if (showHelp) EditorGUILayout.HelpBox("Marksmanship affects the damage and accuracy of ranged attacks", MessageType.Info);
            sc.CurrentStats.SetMarksmanship(EditorGUILayout.IntSlider("Marksmanship", sc.CurrentStats[StatType.MARKSMANSHIP], 0, 100));
            if (showHelp) EditorGUILayout.HelpBox("Dexterity affects the damage of slice attacks", MessageType.Info);
            sc.CurrentStats.SetDexterity(EditorGUILayout.IntSlider("Dexterity", sc.CurrentStats[StatType.DEXTERITY], 0, 100));
            if (showHelp) EditorGUILayout.HelpBox("Luck affects the chance of doing critical damage or quickly refilling energy", MessageType.Info);
            sc.CurrentStats.SetLuck(EditorGUILayout.IntSlider("Luck", sc.CurrentStats[StatType.LUCK], 0, 100));
            if (showHelp) EditorGUILayout.HelpBox("Speed affects the time it takes to recover energy", MessageType.Info);
            sc.CurrentStats.SetSpeed(EditorGUILayout.IntSlider("Speed", sc.CurrentStats[StatType.SPEED], 0, 100));

            //There should be a disabled "SET CHANGES AS DEFAULT" option that gets enabled whenever you change a value.
            if (GUILayout.Button("Apply changes as default"))
            {

            }
        }
        else EditorGUILayout.HelpBox("Stats will be shown in Play mode. This will allow you to dynamically change the values in-game, and store them as level defaults as you see fit", MessageType.Info);
    }
}
