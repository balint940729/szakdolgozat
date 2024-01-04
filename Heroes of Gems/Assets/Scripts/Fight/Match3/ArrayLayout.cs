﻿using UnityEngine;

[System.Serializable]
public class ArrayLayout {

    [System.Serializable]
    public struct RowData {
        public bool[] row;
    }

    public Grid grid;
    public RowData[] rows = new RowData[8]; //Grid of 8x8
}