﻿using UnityEngine;

[System.Serializable]
public class TeamObjectData {
    [SerializeField] public string teamName;
    [SerializeField] public Unit[] members;
    [SerializeField] public bool isSelected;

    public TeamObjectData() {
        members = new Unit[4];
    }

    public TeamObjectData(string teamName, Unit[] units, bool isSelected) {
        this.teamName = teamName;
        members = units;
        this.isSelected = isSelected;
    }
}