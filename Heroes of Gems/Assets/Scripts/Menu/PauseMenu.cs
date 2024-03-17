using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenuUI;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (PauseStateHandler.IsGamePaused()) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        PauseStateHandler.SetGamePause(false);
    }

    private void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        PauseStateHandler.SetGamePause(true);
    }

    public void LoadMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
        pauseMenuUI.SetActive(false);
        PauseStateHandler.SetGamePause(false);
    }

    public void LoadGame() {
        Time.timeScale = 1f;
        DataPersistenceManager.instance.LoadGame();
        pauseMenuUI.SetActive(false);
        PauseStateHandler.SetGamePause(false);
    }

    public void SaveGame() {
        DataPersistenceManager.instance.SaveGame();
        Resume();
    }

    public void ExitMenu() {
        Application.Quit();
    }

    public void SaveMenu() {
        DataPersistenceManager.instance.SaveGame();
    }
}