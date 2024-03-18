using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerBattle : MonoBehaviour {
    public static GameObject enemyGO;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player" && TurnBase.GetInstance() == null) {
            if (HasTeam()) {
                EnemyTeamHandler.SetTeam(GetComponent<Team>().team);

                enemyGO = gameObject;

                PauseStateHandler.SetGamePause(true);

                ChangeMusic();

                SceneManager.LoadScene(1, LoadSceneMode.Additive);
            }
            else {
                Debug.Log("Battle can't start with empty Team");
            }
        }
    }

    private bool HasTeam() {
        foreach (Unit unit in PlayerTeamHandler.GetTeam()) {
            if (unit != null) {
                return true;
            }
        }

        return false;
    }

    private void ChangeMusic() {
        AudioManager.instance.ChangeMusic("Theme", "Fight");
    }
}