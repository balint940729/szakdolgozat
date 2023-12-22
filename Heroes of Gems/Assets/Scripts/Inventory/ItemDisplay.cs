using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ItemDisplay : MonoBehaviour {
    public Image item;
    public GameObject grayScale;
    public TMP_Text itemCounter;

    public void ChangeItemCounter(int count) {
        itemCounter.text = count.ToString();
    }

    public void ChangeItemImage(Sprite img) {
        item.sprite = img;
    }

    public void ChangeGrayScale(bool active) {
        grayScale.SetActive(active);
    }
}