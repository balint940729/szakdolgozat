#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpellBaseSO))]
public class SpellBaseEditor : Editor {
    private SerializedProperty spellLogicProperty;

    private void OnEnable() {
        // Fetch the SerializedProperty for the SpellLogic field
        spellLogicProperty = serializedObject.FindProperty("spellLogic");
    }

    public override void OnInspectorGUI() {
        // Draw the default inspector, which includes the field for SpellLogic
        DrawDefaultInspector();

        // Update the SerializedObject
        serializedObject.Update();

        // Check if the SpellLogic field has changed
        CheckSpellLogicChange();

        // Apply modified properties to save changes
        serializedObject.ApplyModifiedProperties();
    }

    private void CheckSpellLogicChange() {
        // Check if the field is a MonoScript
        if (spellLogicProperty != null && spellLogicProperty.propertyType == SerializedPropertyType.ObjectReference) {
            MonoScript currentSpellLogicScript = spellLogicProperty.objectReferenceValue as MonoScript;

            if (currentSpellLogicScript != null) {
                // Check if the MonoScript is a child of SpellBaseClass
                if (!IsSubclassOfSpellBase(currentSpellLogicScript)) {
                    Debug.Log($"{currentSpellLogicScript.name} is not derived from SpellBaseClass.");

                    spellLogicProperty.objectReferenceValue = null;
                }
            }
        }
    }

    private bool IsSubclassOfSpellBase(MonoScript monoScript) {
        return monoScript != null && monoScript.GetClass().IsSubclassOf(typeof(SpellBaseClass));
    }
}

#endif