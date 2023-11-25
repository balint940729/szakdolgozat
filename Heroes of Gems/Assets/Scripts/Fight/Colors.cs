using UnityEngine;

[CreateAssetMenu(fileName = "New Color", menuName = "Color")]
[System.Serializable]
public class Colors : ScriptableObject {
    public string colorName;

    public Color color;

    public int colorCode;
}