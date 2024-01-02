using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "SpellSO", menuName = "Spells/SpellSO")]
public class SpellBaseSO : ScriptableObject {
    public string spellName;
    public string spellDescription;
    public Sprite spellImage;

    [SerializeField]
    public MonoScript spellLogic;
}