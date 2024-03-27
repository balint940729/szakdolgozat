using UnityEngine;

public class PauseStateHandler : MonoBehaviour {
    private static bool gameIsPaused = false;

    public static bool IsGamePaused() {
        return gameIsPaused;
    }

    public static void SetGamePause(bool pause) {
        gameIsPaused = pause;
    }
}