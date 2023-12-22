using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Race", menuName = "Race")]
[System.Serializable]
public class Race : ScriptableObject {
    public string raceName;
    public int raceBonusModifier;
    public List<Race> strongAgainst;
    public List<Race> weakAgainst;
    public List<string> raceBonusStat;
    public Sprite raceImage;
}