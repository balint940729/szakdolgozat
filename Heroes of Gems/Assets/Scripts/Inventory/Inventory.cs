using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Inventory : MonoBehaviour {
    [SerializeField] private GameObject inventoryContainerPrefab;

    [SerializeField] private Transform containerParent;
    [SerializeField] private List<GameObject> invItemsPrefabs;

    private void Start() {
        for (int i = 0; i < 2; i++) {
            GameObject invCont = Instantiate(inventoryContainerPrefab);
            invCont.transform.SetParent(containerParent, false);

            RectTransform parentTR = (RectTransform)containerParent.transform;
            RectTransform rectTR = (RectTransform)invCont.transform;
            float invHeight = parentTR.rect.height;
            float invWidth = parentTR.rect.width;
            rectTR.sizeDelta = new Vector2(invWidth * 50f / 100, invHeight * 90f / 100);
            rectTR.transform.localPosition = new Vector3(-invWidth * 24f / 100, -invHeight * 3f / 100, 0);

            GameObject.Find("InventoryCanvas").GetComponent<InventoryUI>().AddInvContainer(invCont);
            switch (i) {
                case 0:
                    invCont.name = "UnitsContainer";
                    GameObject unitsContent = GameObject.Find("Content");
                    unitsContent.name = "UnitsContent";
                    unitsContent.AddComponent<UnitsInventory>();
                    unitsContent.GetComponent<UnitsInventory>().emptyItemPrefab = invItemsPrefabs.Find(item => item.name == "InventorySlot");
                    unitsContent.GetComponent<UnitsInventory>().folderPath = "Assets/Sprites/Cards";

                    //invCont.SetActive(false);
                    break;

                case 1:
                    invCont.name = "ItemsContainer";
                    GameObject itemsContent = GameObject.Find("Content");
                    itemsContent.name = "ItemsContent";
                    itemsContent.AddComponent<ItemsInventory>();
                    itemsContent.GetComponent<ItemsInventory>().emptyItemPrefab = invItemsPrefabs.Find(item => item.name == "InventorySlot");
                    itemsContent.GetComponent<ItemsInventory>().folderPath = "Assets/Sprites/Items/ItemsInventory";

                    break;

                default:
                    break;
            }
        }
    }
}