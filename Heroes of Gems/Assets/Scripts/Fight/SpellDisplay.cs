using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellDisplay : MonoBehaviour {
    public SpellBase spell;

    public TMP_Text spellName;
    public TMP_Text spellDescription;

    public Image image;

    //Start is called before the first frame update
    private void Start() {
        spellName.text = spell.spellName.ToString();
        spellDescription.text = spell.spellDescription.ToString();

        image.sprite = spell.spellImage;
    }

    public void SetSpellDescription() {
        spellDescription.text = spell.spellDescription.ToString();
    }
}