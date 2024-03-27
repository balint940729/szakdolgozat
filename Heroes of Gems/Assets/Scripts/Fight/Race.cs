using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Race", menuName = "Race")]
[System.Serializable]
public class Race : ScriptableObject {
    public string raceName;
    public List<Race> strongAgainst;
    public List<Race> weakAgainst;
    public RaceBonus raceBonus;
    public Sprite raceImage;
}