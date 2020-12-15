using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitDisplay : MonoBehaviour
{
    public Unit card;

    public TMP_Text health;
    public TMP_Text attack;
    public TMP_Text armor;

    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        health.text = card.unitHealth.ToString();
        attack.text = card.unitAttack.ToString();
        armor.text = card.unitArmor.ToString();

        image.sprite = card.unitImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
