using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Inventory inventory;
    [SerializeField] KeyCode itemPickupKeyCode = KeyCode.E;
    [SerializeField] GameObject PlayerGameObject;
    [SerializeField] bool InRange;

    private void Update()
    {
        if(InRange == true && Input.GetKeyDown(itemPickupKeyCode))
        {
            inventory.AddItem(item);
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == PlayerGameObject)
        {
            InRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == PlayerGameObject)
        {
            InRange = false;
        }
    }
}
