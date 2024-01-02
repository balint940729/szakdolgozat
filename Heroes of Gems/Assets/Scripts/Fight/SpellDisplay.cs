using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellDisplay : MonoBehaviour {
    public SpellBaseSO spell;

    public TMP_Text spellName;
    public TMP_Text spellDescription;

    public Image image;

    private string originalDesc;

    private void Start() {
        spellName.text = spell.spellName.ToString();

        image.sprite = spell.spellImage;
    }

    public void SetSpellDescription(int spellDamage) {
        if (originalDesc == null) {
            originalDesc = spell.spellDescription;
        }

        spellDescription.text = originalDesc.Replace("&X", spellDamage.ToString());
        //spell.ChangeSpellDescription(spellDescription.text);
        //this.spellDescription.text = spellDescription.ToString();
    }
}