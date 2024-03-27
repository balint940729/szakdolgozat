using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {
    public static bool isNewGame = false;

    public void NewGame() {
        isNewGame = true;
        SceneManager.LoadScene(0);
    }

    public void LoadGame() {
        isNewGame = false;
        SceneManager.LoadScene(0);
    }

    public void ExitGame() {
        Application.Quit();
    }
}