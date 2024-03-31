using UnityEngine;

public class GameController : MonoBehaviour {
    private static GameController controller;
    private static bool newGame = false;

    //public bool playerDead;
    //public bool inBattle;

    public static void SetNewGame(bool isNewGame) {
        newGame = isNewGame;
    }

    public static bool IsNewGame() {
        return newGame;
    }

    //private void OnEnable() {
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnDisable() {
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
    //    if (scene.name == "Game" && playerDead == true) {
    //        inBattle = false;
    //    }
    //}

    private void Awake() {
        if (controller = null) {
            controller = this;
        }
    }
}