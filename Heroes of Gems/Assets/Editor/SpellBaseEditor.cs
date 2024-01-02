//#if UNITY_EDITOR

//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(SpellBaseSO))]
//public class SpellBaseEditor : Editor {
//    SerializedProperty spellLogicProperty;

//    private void OnEnable() {
//        spellLogicProperty = serializedObject.FindProperty("spellLogic");
//    }
//    public override void OnInspectorGUI() {
//        serializedObject.Update();

//        SpellBaseSO container = (SpellBaseSO)target;

//        DrawDefaultInspector();

//        //EditorGUILayout.PropertyField(serializedObject.FindProperty("spellLogic"), true);

//        // Custom editor GUI for assigning the script reference
//        EditorGUI.BeginChangeCheck();
//        //EditorGUILayout.PropertyField(spellLogicProperty, new GUIContent("Spell Logic"), false);
//        if (EditorGUI.EndChangeCheck()) {
//            serializedObject.ApplyModifiedProperties();
//        }

//        //if (GUILayout.Button("Add Spell")) {
//        //    AssignSpell(container);
//        //}

//        serializedObject.ApplyModifiedProperties();
//    }

//    //private void AssignSpell(SpellBaseSO container) {
//    //    string path = EditorUtility.OpenFilePanel("Select Spell Script", "Assets/Scripts/Fight/Spells", "cs");
//    //    if (!string.IsNullOrEmpty(path)) {
//    //        string assetPath = path.Replace(Application.dataPath, "Assets");
//    //        MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);

//    //        if (script != null && script.GetClass() != null) {
//    //            if (script.GetClass().IsSubclassOf(typeof(SpellBaseClass)) || script.GetClass() == typeof(SpellBaseClass)) {
//    //                // Create an instance of the script class
//    //                container.spellLogic = script.GetClass().Assembly.CreateInstance(script.GetClass().FullName) as SpellBaseClass;



//    //                // Save the changes to the asset
//    //                EditorUtility.SetDirty(container);
//    //                AssetDatabase.SaveAssets();
//    //                AssetDatabase.Refresh();
//    //            }
//    //            else {
//    //                Debug.LogWarning("Selected script is not a valid SpellBaseClass or its subclass.");
//    //            }
//    //        }
//    //        else {
//    //            Debug.LogWarning("Failed to load script.");
//    //        }
//    //    }

//    //}
//}

//#endif