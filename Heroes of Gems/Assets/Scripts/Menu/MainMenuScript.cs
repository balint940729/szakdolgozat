﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public void PlayGame() {
        SceneManager.LoadScene(0);
    }

    public void ExitGame() {
        Application.Quit();
    }
}