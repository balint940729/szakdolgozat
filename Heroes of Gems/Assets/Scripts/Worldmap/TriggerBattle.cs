using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerBattle : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            SceneManager.LoadScene(1);
        }
    }
}