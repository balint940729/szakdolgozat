using UnityEngine;

public class ItemPickup : MonoBehaviour {
    [SerializeField] private Item item;
    [SerializeField] private Inventory inventory;
    [SerializeField] private KeyCode itemPickupKeyCode = KeyCode.E;
    [SerializeField] private GameObject PlayerGameObject;
    [SerializeField] private bool InRange;

    private void Update() {
        if (InRange == true && Input.GetKeyDown(itemPickupKeyCode)) {
            Achievements.ach01Count += 1;
            inventory.AddItem(item);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject == PlayerGameObject) {
            InRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject == PlayerGameObject) {
            InRange = false;
        }
    }
}