using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyAlive {

    [SerializeField]
    public GameObjectData enemy;

    [SerializeField]
    public bool isAlive;

    public EnemyAlive(GameObjectData enemy, bool isAlive) {
        this.enemy = enemy;
        this.isAlive = isAlive;
    }
}

public class EnemiesController : MonoBehaviour, IDataPersistence {
    private static List<EnemyAlive> enemies = new List<EnemyAlive>();
    private List<GameObject> enemiesGO = new List<GameObject>();

    private void Start() {
        enemiesGO.AddRange(GameObject.FindGameObjectsWithTag("Enemies"));

        foreach (GameObject enemy in enemiesGO) {
            GameObjectData enemyGOD = new GameObjectData(enemy);
            EnemyAlive enemyAlive = new EnemyAlive(enemyGOD, enemy.GetComponent<EnemyController>().IsEnemyAlive());
            enemies.Add(enemyAlive);
        }
    }

    public static void SetEnemy(GameObject enemy, bool isAlive) {
        GameObjectData enemyGOD = new GameObjectData(enemy);
        int index = enemies.FindIndex(e => e.enemy.name == enemyGOD.name);
        if (index != -1) {
            enemies[index] = new EnemyAlive(enemyGOD, isAlive);
        }
    }

    public void LoadData(GameData gameData) {
        foreach (EnemyAlive enemyGOD in gameData.enemies) {
            if (!enemyGOD.isAlive) {
                GameObject.Find(enemyGOD.enemy.name.ToString()).GetComponent<EnemyController>().SetEnemyAlive(false);
            }
        }
    }

    public void SaveData(ref GameData gameData) {
        gameData.enemies = enemies;
    }
}