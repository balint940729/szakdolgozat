using UnityEngine;

public class EnemyController : MonoBehaviour {
    [SerializeField] private bool isAlive = true;

    private void LateUpdate() {
        if (!isAlive) {
            EnemiesController.SetEnemy(gameObject, false);
            gameObject.SetActive(false);
        }
    }

    public bool IsEnemyAlive() {
        return isAlive;
    }

    public void SetEnemyAlive(bool isAlive) {
        this.isAlive = isAlive;
    }
}