using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ArrayLayout))]
public class CustPropertyDrawer : PropertyDrawer {
    private const float HEIGHT = 22f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.PrefixLabel(position, label);
        Rect newposition = position;
        newposition.y += HEIGHT;
        SerializedProperty data = property.FindPropertyRelative("rows");
        if (data.arraySize != 8)
            data.arraySize = 8;
        //data.rows[0][]
        for (int j = 0; j < 8; j++) {
            SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
            newposition.height = HEIGHT;
            if (row.arraySize != 8)
                row.arraySize = 8;
            newposition.width = position.width / 8;
            for (int i = 0; i < 8; i++) {
                EditorGUI.PropertyField(newposition, row.GetArrayElementAtIndex(i), GUIContent.none);
                newposition.x += newposition.width;
            }

            newposition.x = position.x;
            newposition.y += HEIGHT;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return HEIGHT * 9;
    }
}