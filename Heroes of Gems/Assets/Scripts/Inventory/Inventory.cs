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

            ////RectTransform rectTR = (RectTransform)containerParent.transform;
            //RectTransform rectTR = (RectTransform)invCont.transform;
            //float invHeight = rectTR.rect.height;
            //float invWidth = rectTR.rect.width;
            ////invCont.transform.position = new Vector3(Screen.width * (25f * titleCount) / 100, Screen.height - titleHeight);
            ////invCont.transform.position = new Vector3(invWidth * 75f / 100, invHeight * 75f / 100);
            //rectTR.rect.Set(rectTR.rect.x, rectTR.rect.y, rectTR.rect.width - 600f, rectTR.rect.height * 75f / 100);

            GameObject.Find("InventoryCanvas").GetComponent<InventoryUI>().AddInvContainer(invCont);
            switch (i) {
                case 0:
                    invCont.name = "UnitsContainer";
                    invCont.AddComponent<UnitsInventory>();
                    invCont.GetComponent<UnitsInventory>().emptyItemPrefab = invItemsPrefabs.Find(item => item.name == "InventorySlot");
                    invCont.GetComponent<UnitsInventory>().folderPath = "Assets/Sprites/Items/UnitsInventory";

                    //invCont.SetActive(false);
                    break;

                case 1:
                    invCont.name = "ItemsContainer";
                    invCont.AddComponent<ItemsInventory>();
                    invCont.GetComponent<ItemsInventory>().emptyItemPrefab = invItemsPrefabs.Find(item => item.name == "InventorySlot");
                    invCont.GetComponent<ItemsInventory>().folderPath = "Assets/Sprites/Items/ItemsInventory";

                    break;

                default:
                    break;
            }
        }
    }
}