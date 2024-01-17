using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerBattle : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            EnemyTeamHandler.SetTeam(GetComponent<Team>().team);
            PlayerTeamHandler.SetTeam(other.GetComponent<Team>().team);

            SceneManager.LoadScene(1);
        }
    }
}