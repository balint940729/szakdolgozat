using System.Collections.Generic;
using UnityEngine;

public class Rewards : MonoBehaviour {
    [SerializeField] private int goldReward = default;
    [SerializeField] private List<Dropable> guaranteedRewards = new List<Dropable>();
    [SerializeField] private List<Dropable> randomRewards = new List<Dropable>();

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            List<Dropable> rewards = GenerateRewards();

            foreach (Dropable drop in rewards) {
                if (drop.GetType() == typeof(Unit)) {
                    UnitsInventory.AddUnit(drop as Unit);
                }
            }
        }
    }

    public List<Dropable> GenerateRewards() {
        List<Dropable> rewards = new List<Dropable>();

        foreach (Dropable gReward in guaranteedRewards) {
            rewards.Add(gReward);
        }

        int randomNumber = Random.Range(1, 101);

        foreach (Dropable randomReward in randomRewards) {
            if (randomNumber <= randomReward.dropChance) {
                rewards.Add(randomReward);
            }
        }

        return rewards;
    }
}