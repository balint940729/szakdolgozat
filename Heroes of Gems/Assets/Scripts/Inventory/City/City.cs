using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New City", menuName = "City")]
public class City : ScriptableObject {
    public string cityName;
    public List<Building> buildings;
}