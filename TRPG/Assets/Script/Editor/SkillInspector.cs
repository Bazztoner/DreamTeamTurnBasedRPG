using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Skill))]
public class SkillInspector : Editor
{
    Skill sk;

    void OnEnable()
    {
        sk = (Skill)target;
    }

    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Open in Skill Manager"))
        {
            SkillManagerWindow.CreateWindow(sk);
        }
    }
}
