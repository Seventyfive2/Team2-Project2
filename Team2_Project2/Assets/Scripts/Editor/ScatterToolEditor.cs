using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(ScatterTool))]
public class ScatterToolEditor : Editor
{
    public static ScatterTool scatterTool;
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        scatterTool = (ScatterTool)target;

        FieldInfo[] property = typeof(ScatterTool).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < property.Length; i++)
        {
            if (property[i].IsPublic && property[i].GetCustomAttribute<HideInInspector>() == null || property[i].GetCustomAttribute<SerializeField>() != null)
            {
                if (property[i].GetCustomAttribute<ConditionalValueAttribute>() != null)
                {
                    ConditionalValueAttribute at = property[i].GetCustomAttribute<ConditionalValueAttribute>();
                    if (serializedObject.FindProperty(at.Condition).boolValue)
                    {
                        if (at.Condition2 != "")
                        {
                            if (serializedObject.FindProperty(at.Condition2).boolValue)
                            {
                                if (at.Condition3 != "")
                                {
                                    if (serializedObject.FindProperty(at.Condition3).boolValue)
                                    {
                                        EditorGUILayout.PropertyField(serializedObject.FindProperty(property[i].Name));
                                    }
                                }
                                else
                                {
                                    EditorGUILayout.PropertyField(serializedObject.FindProperty(property[i].Name));
                                }
                            }
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty(property[i].Name));
                        }
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(property[i].Name));
                }
            }
        }

        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Generate"))
        {
            scatterTool.Generate();
        }
    }
}

[CustomEditor(typeof(ScatteredObject)), CanEditMultipleObjects]
public class ScatteredObjectEditor : Editor
{
    SerializedObject scatterTool = new SerializedObject(ScatterToolEditor.scatterTool);
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        FieldInfo[] property = typeof(ScatteredObject).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        for (int i = 0; i < property.Length; i++)
        {
            if (property[i].IsPublic && property[i].GetCustomAttribute<HideInInspector>() == null || property[i].GetCustomAttribute<SerializeField>() != null)
            {
                if (property[i].GetCustomAttribute<ConditionalValueAttribute>() != null)
                {
                    ConditionalValueAttribute at = property[i].GetCustomAttribute<ConditionalValueAttribute>();
                    if (scatterTool.FindProperty(at.Condition).boolValue)
                    {
                        Debug.Log("Displaying " + at.Condition);
                        if (at.Condition2 != "")
                        {
                            if (scatterTool.FindProperty(at.Condition2).boolValue)
                            {
                                if (at.Condition3 != "")
                                {
                                    if (scatterTool.FindProperty(at.Condition3).boolValue)
                                    {
                                        EditorGUILayout.PropertyField(serializedObject.FindProperty(property[i].Name));
                                    }
                                }
                                else
                                {
                                    EditorGUILayout.PropertyField(serializedObject.FindProperty(property[i].Name));
                                }
                            }
                        }
                        else
                        {
                            EditorGUILayout.PropertyField(serializedObject.FindProperty(property[i].Name));
                        }
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(property[i].Name));
                    Debug.Log("Displaying " + property[i]);
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}

