using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerBattle : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.name == "Player") {
            EnemyTeamHandler.SetTeam(GetComponent<Teams>().team);
            PlayerTeamHandler.SetTeam(other.GetComponent<Teams>().team);

            SceneManager.LoadScene(1);
        }
    }
}