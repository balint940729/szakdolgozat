using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitController : MonoBehaviour, IPointerClickHandler {
    private UnitDisplay unitCard;
    private SpellDisplay spellCard;
    private string folderPath = "Assets/Sprites/Cards";

    public event System.Action<string> onSpellDisplay;

    private string spellFolderPath = "Assets/Sprites/Spells";
    private string[] assetGuids;
    private string[] spellAssetGuids;

    private int health;
    private int armor;
    private int attack;
    private int spellDamage;
    private int mana;
    private int maxMana;
    private List<Colors> colors;
    private SpellBase spell;

    private void Awake() {
        unitCard = GetComponent<UnitDisplay>();
        spellCard = GetComponent<SpellDisplay>();
        assetGuids = AssetDatabase.FindAssets("t:Unit", new string[] { folderPath });
    }

    public void setUp(int cardID, GameObject spellGO) {
        string assetPath = AssetDatabase.GUIDToAssetPath(assetGuids[cardID]);
        unitCard.card = AssetDatabase.LoadAssetAtPath<Unit>(assetPath);

        string typeSpell = "t:" + unitCard.card.name + "Spell";
        spellAssetGuids = AssetDatabase.FindAssets(typeSpell, new string[] { spellFolderPath });

        string spellAssetPath = AssetDatabase.GUIDToAssetPath(spellAssetGuids[0]);
        spellCard = spellGO.GetComponent<SpellDisplay>();
        spellCard.spell = AssetDatabase.LoadAssetAtPath<SpellBase>(spellAssetPath);

        health = unitCard.card.baseHealth;
        armor = unitCard.card.baseArmor;
        attack = unitCard.card.baseAttack;
        spellDamage = unitCard.card.baseSpellDamage;
        mana = unitCard.card.currentMana;
        maxMana = unitCard.card.maxMana;

        colors = unitCard.card.colors;
        spell = unitCard.card.spell;
        spell.ChangeSpellDescription(spellDamage);
    }

    public void OnPointerClick(PointerEventData eventData) {
        onSpellDisplay?.Invoke(spellCard.name);
    }

    public bool castSpell(List<UnitController> allyTargets, List<UnitController> targets) {
        if (mana == maxMana) {
            if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer) {
                BattleStateHandler.setState(BattleState.PlayerTurn);
            }
            else if (BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
                BattleStateHandler.setState(BattleState.EnemyTurn);
            }
            spell.setCaster(this);
            spell.setEnemyTargets(targets);
            spell.setPlayerTargets(allyTargets);
            spell.InitializeSpell();

            mana = 0;
            unitCard.setMana(mana);
            SpellController.CloseSpell();
            return true;
        }

        return false;
    }

    public void setUpColors(GameObject colorBG, int index) {
        unitCard.setColors(colorBG, index);
    }

    public List<Colors> getColors() {
        return colors;
    }

    public int getColorsCount() {
        return colors.Count;
    }

    public int GetHealth() {
        return health;
    }

    public int GetArmor() {
        return armor;
    }

    public int GetAttack() {
        return attack;
    }

    public int GetSpellDamage() {
        return spellDamage;
    }

    public void normalDamage(int damage, UnitController target) {
        target.armor -= damage;

        if (target.armor < 0) {
            target.health += target.armor;
            target.armor = 0;
        }

        if (target.health < 0) {
            target.health = 0;
        }

        target.unitCard.setHealth(target.health, target.armor);
    }

    public void TrueDamage(int damage, UnitController target) {
        target.health -= damage;

        if (target.health < 0) {
            target.health = 0;
        }

        target.unitCard.setHealth(target.health, target.armor);
    }

    public void spellAttack(int spellDamage, UnitController target) {
        target.armor -= spellDamage;

        if (target.armor < 0) {
            target.health += target.armor;
            target.armor = 0;
        }

        if (target.health < 0) {
            target.health = 0;
        }

        target.unitCard.setHealth(target.health, target.armor);
    }

    public int gainMana(int manaAmount) {
        int remainedMana = 0;
        mana += manaAmount;

        if (mana > maxMana) {
            remainedMana = mana - maxMana;
            mana = maxMana;
        }

        unitCard.setMana(mana);

        return remainedMana;
    }

    public bool isOnFullMana() {
        if (mana == maxMana) {
            return true;
        }

        return false;
    }
}