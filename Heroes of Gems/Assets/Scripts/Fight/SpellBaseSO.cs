using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Spell", menuName = "Spell")]
public class SpellBaseSO : ScriptableObject {
    public string spellName;
    public string spellDescription;
    public Sprite spellImage;

    [SerializeField]
    public MonoScript spellLogic;
}