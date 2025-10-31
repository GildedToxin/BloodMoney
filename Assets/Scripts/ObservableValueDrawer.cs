using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ObservableValue<>))]
public class ObservableValueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty valueProp = property.FindPropertyRelative("_value");

        if (valueProp != null)
        {
            EditorGUI.PropertyField(position, valueProp, label, true);
        }
        else
        {
            EditorGUI.LabelField(position, label.text, "Unsupported type");
        }
    }
}
