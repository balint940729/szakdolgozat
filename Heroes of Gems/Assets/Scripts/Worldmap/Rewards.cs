using System.Collections.Generic;
using UnityEngine;

public class Rewards : MonoBehaviour {
    [SerializeField] private int goldReward = default;
    [SerializeField] private List<Lootable> guaranteedRewards = new List<Lootable>();
    [SerializeField] private List<Lootable> randomRewards = new List<Lootable>();

    public void GainLoot() {
        List<Lootable> rewards = GenerateRewards();

        foreach (Lootable drop in rewards) {
            if (drop.GetType() == typeof(Unit)) {
                UnitsInventory.AddUnit(drop as Unit);
            }

            if (drop.GetType() == typeof(Item)) {
                ItemsInventory.AddItem(drop as Item);
            }
        }

        GoldController.AddGold(goldReward);
    }

    public List<Lootable> GenerateRewards() {
        List<Lootable> rewards = new List<Lootable>();

        foreach (Lootable gReward in guaranteedRewards) {
            rewards.Add(gReward);
        }

        int randomNumber = Random.Range(1, 101);

        foreach (Lootable randomReward in randomRewards) {
            if (randomNumber <= randomReward.dropChance) {
                rewards.Add(randomReward);
            }
        }

        return rewards;
    }
}