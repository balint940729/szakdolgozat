using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour {
    public GameObject achNote;
    public bool achActive = false;

    public GameObject ach01Image;
    public GameObject achTitle;
    public GameObject achDesc;

    public static int ach01Count;
    public int ach01Trigger = 1;
    public int ach01Code = 0;

    private void Awake() {
        PlayerPrefs.SetInt("Ach01", 0);
    }

    // Update is called once per frame
    private void Update() {
        ach01Code = PlayerPrefs.GetInt("Ach01");
        if (ach01Count == ach01Trigger && ach01Code != 12345) {
            StartCoroutine(Trigger01Ach());
        }
    }

    private IEnumerator Trigger01Ach() {
        achActive = true;
        ach01Code = 12345;
        PlayerPrefs.SetInt("Ach01", ach01Code);
        ach01Image.SetActive(true);
        achTitle.GetComponent<Text>().text = "First key";
        achDesc.GetComponent<Text>().text = "You found the first key";
        achNote.SetActive(true);
        yield return new WaitForSeconds(7);

        // Resetting
        achNote.SetActive(false);
        ach01Image.SetActive(false);
        achTitle.GetComponent<Text>().text = "";
        achDesc.GetComponent<Text>().text = "";
        achActive = false;
    }
}