using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class SkillManagerWindow : EditorWindow
{
    public Skill sk;
    GUIStyle titleFormat;
    GUIStyle subtitleFormat;

    List<int> scalesWithPicks;

    public static void CreateWindow(Skill sk)
    {
        SkillManagerWindow smw = (SkillManagerWindow)GetWindow(typeof(SkillManagerWindow));
        smw.Show();
        smw.sk = sk;
    }

    void OnGUI()
    {
        if (titleFormat == null)
        {
            titleFormat = new GUIStyle();
            titleFormat.fontStyle = FontStyle.Bold;
            titleFormat.wordWrap = true;
        }
        if (subtitleFormat == null)
        {
            subtitleFormat = new GUIStyle();
            subtitleFormat.wordWrap = true;
        }

        DrawWindow();
    }

    void DrawWindow()
    {
        if (sk.Actions == null) sk.Actions = new SkillAction[3];

        EditorGUILayout.LabelField("Welcome to the Skill Manager", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUI.BeginDisabledGroup(sk == null);
        if (GUILayout.Button("Add offensive action"))
        {
            if (sk.Actions[0] == null) sk.Actions[0] = new OffensiveAction();
            else Debug.LogWarning("You already have an offensive action!");
        }
        if (GUILayout.Button("Add support action"))
        {
            if (sk.Actions[1] == null) sk.Actions[1] = new SupportAction();
            else Debug.LogWarning("You already have a support action!");
        }
        if (GUILayout.Button("Add buff/debuff"))
        {
            if (sk.Actions[2] == null) sk.Actions[2] = new BuffAction();
            else Debug.LogWarning("You already have a buff!");
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();
        if (sk != null)
        {
            EditorGUILayout.LabelField(sk.name, titleFormat);
            if (sk.Actions[0] != null)
            {
                EditorGUILayout.LabelField("Off!");
                DrawOffensiveActions();
            }

            if (sk.Actions[1] != null)
            {
                EditorGUILayout.LabelField("Supp!");
            }

            if (sk.Actions[2] != null)
            {
                EditorGUILayout.LabelField("Buff!");
            }
        }
    }

    void DrawOffensiveActions()
    {
        var off = sk.Actions[0] as OffensiveAction;
        EditorGUILayout.LabelField("Offensive action", titleFormat);
        EditorGUILayout.LabelField("Damage Percentages", subtitleFormat);
        if (GUILayout.Button("Add damage factor"))
        {
            if (off.ScalesWith == null)
            {
                off.ScalesWith = new List<StatType>();
                off.DmgPercentage = new List<int>();
            }
            off.ScalesWith.Add(StatType.LEVEL);
            off.DmgPercentage.Add(0);
        }
        if (off.ScalesWith != null)
        {
            scalesWithPicks = new List<int>();
            for (int i = 0; i < off.ScalesWith.Count; i++)
            {
                scalesWithPicks.Add((int)off.ScalesWith[i]);
            }

            EditorGUILayout.LabelField("Scales with:");
            for (int i = 0; i < off.ScalesWith.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                scalesWithPicks[i] = EditorGUILayout.Popup(scalesWithPicks[i], Enum.GetNames(typeof(StatType)));
                off.ScalesWith[i] = (StatType)scalesWithPicks[i];
                if (GUI.changed) Debug.Log(off.ScalesWith[i]);

                off.DmgPercentage[i] = EditorGUILayout.IntSlider("Percentage: ", off.DmgPercentage[i], 0, 150);
                if (GUILayout.Button("Remove"))
                {
                    scalesWithPicks.RemoveAt(i);
                    off.ScalesWith.RemoveAt(i);
                    off.DmgPercentage.RemoveAt(i);
                }
                EditorGUILayout.EndHorizontal();
            }
        }

    }
}
