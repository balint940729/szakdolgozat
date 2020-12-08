using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject inventoryMenuUI;

    public GameObject theFollower;
    public GameObject thePlayer;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && (theFollower.transform.position.x - thePlayer.transform.position.x) <= 1
            && (theFollower.transform.position.x - thePlayer.transform.position.x) >= -1
            && (theFollower.transform.position.y - thePlayer.transform.position.y) <= 1
            && (theFollower.transform.position.y - thePlayer.transform.position.y) >= -1)
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if ((theFollower.transform.position.x - thePlayer.transform.position.x) >= 1
            || (theFollower.transform.position.x - thePlayer.transform.position.x) <= -1
            || (theFollower.transform.position.y - thePlayer.transform.position.y) >= 1
            || (theFollower.transform.position.y - thePlayer.transform.position.y) <= -1)
        {
            inventoryMenuUI.SetActive(false);
        }
    }

    public void Resume()
    {
        inventoryMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }
    void Pause()
    {
        inventoryMenuUI.SetActive(true);
        //Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
