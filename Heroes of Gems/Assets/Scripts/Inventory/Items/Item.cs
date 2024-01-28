using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ItemType { Weapon, Helmet, BodyArmor, Cape, Gloves, Pants, Boots, OffHand, Trinkets, Other };

[CreateAssetMenu]
public class Item : ScriptableObject {
    public string itemName;
    public Sprite itemIcon;
    public List<ItemType> itemTypes;
    public List<Race> affectedRaces;
    public List<RaceStats> affectedStats;
    public int statModifier;

    [HideInInspector]
    public string itemDescr;

    public virtual void InitializeItemDescription() {
        string races = "";
        if (affectedRaces.Count == 0) {
            races += "Everyone";
        }
        else if (affectedRaces.Count == 1) {
            races += "All " + affectedRaces[0].raceName;
        }
        else if (affectedRaces.Count > 1) {
            races += "All " + string.Join(", ", affectedRaces.Take(affectedRaces.Count - 1)) + " and " + affectedRaces.Last();
        }

        string stats = "";
        if (affectedStats.Count == 1) {
            stats += affectedStats[0].ToString();
        }
        else if (affectedStats.Count > 1) {
            stats += string.Join(", ", affectedStats.Take(affectedStats.Count - 1)) + " and " + affectedStats.Last();
        }

        itemDescr = $"{races} in your team gain +{statModifier} {stats}";
    }
}