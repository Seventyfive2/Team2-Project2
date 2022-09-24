using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.ScrollRect;

[CustomEditor(typeof(WeaponSO))]
public class WeaponSOEditor : Editor
{
    SerializedProperty pAttackStyle;
    SerializedProperty sAttackStyle;

    private void OnEnable()
    {
        pAttackStyle = serializedObject.FindProperty("primaryAttackStyle");
        sAttackStyle = serializedObject.FindProperty("secondaryAttackStyle");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        FieldInfo[] property = typeof(WeaponSO).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        for (int i = 0; i < property.Length; i++)
        {
            SerializedProperty serializedProperty = serializedObject.FindProperty(property[i].Name);

            if (property[i].IsPublic && property[i].GetCustomAttribute<HideInInspector>() == null || property[i].GetCustomAttribute<SerializeField>() != null)
            {
                if (property[i].GetCustomAttribute<ConditionalValueAttribute>() != null)
                {
                    ConditionalValueAttribute at = property[i].GetCustomAttribute<ConditionalValueAttribute>();
                    if (at.Condition == "primaryAttackStyle")
                    {
                        WeaponSO.AttackStyle ms = (WeaponSO.AttackStyle)pAttackStyle.enumValueIndex;
                        if (at.Condition2 == ms.ToString())
                        {
                            EditorGUILayout.PropertyField(serializedProperty);
                        }
                    }
                    if (at.Condition == "secondaryAttackStyle")
                    {
                        WeaponSO.AttackStyle ms = (WeaponSO.AttackStyle)sAttackStyle.enumValueIndex;
                        if (at.Condition2 == ms.ToString())
                        {
                            EditorGUILayout.PropertyField(serializedProperty);
                        }
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(serializedProperty);
                }
            }

        }

        serializedObject.ApplyModifiedProperties();
    }
}
