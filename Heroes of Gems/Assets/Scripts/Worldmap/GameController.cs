using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController controller;

    public bool playerDead;
    public bool inBattle;

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "Game" && playerDead == true) {
            inBattle = false;
        }
    }

    private void Awake() {
        if (controller = null) {
            //DontDestroyOnLoad(gameObject);
            controller = this;
        }
    }
}