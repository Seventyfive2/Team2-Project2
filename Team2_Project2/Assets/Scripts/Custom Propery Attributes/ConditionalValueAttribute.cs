using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class ConditionalValueAttribute : PropertyAttribute
{
    public readonly string Condition;
    public string Condition2 = "";

    public ConditionalValueAttribute(string conditionalSourceField)
    {
        Condition = conditionalSourceField;
    }
}
