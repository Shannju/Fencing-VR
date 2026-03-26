using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TutorialManager))]
public class TutorialManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TutorialManager manager = (TutorialManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Empty Event"))
        {
            Undo.RecordObject(manager, "Add Empty Event");
            manager.AddEmptyEvent();
            EditorUtility.SetDirty(manager);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.HelpBox("Use Add Empty Event to add a new event item in the tutorialEvents list.", MessageType.Info);
    }
}
