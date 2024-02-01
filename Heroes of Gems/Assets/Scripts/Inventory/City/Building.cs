using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Building", menuName = "Building")]
public class Building : ScriptableObject {
    public string buildingName;
    public Sprite[] images = new Sprite[0];
    public List<ModifStats> bonusStats;
    public int bonusModifier;

    [HideInInspector]
    public int buildingLevel = 0;

    public int buildingMaxLevel;
    public int upgradeCost;
}