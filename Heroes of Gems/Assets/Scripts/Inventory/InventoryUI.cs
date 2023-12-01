using UnityEngine;

public class InventoryUI : MonoBehaviour {
    public static bool gameIsPaused = false;
    public GameObject inventoryMenuUI;

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.I)) {
            if (gameIsPaused) {
                Resume();
            }
            else {
                Pause();
            }
        }
    }

    public void Resume() {
        inventoryMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    private void Pause() {
        inventoryMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }
}