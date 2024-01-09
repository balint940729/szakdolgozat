using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Inventory : MonoBehaviour {
    [SerializeField] private GameObject inventoryCanvas = default;
    [SerializeField] private GameObject inventoryContainerPrefab = default;

    [SerializeField] private Transform containerParent = default;
    [SerializeField] private List<GameObject> invItemsPrefabs = default;

    private void Start() {
        InstantiateContainer("Units", "Assets/Sprites/Cards", Type.GetType("UnitsInventory"), true);
        InstantiateContainer("Items", "Assets/Sprites/Items/ItemsInventory", Type.GetType("ItemsInventory"), true);
        InstantiateContainer("Equipments", "Assets/Sprites/Items/ItemsInventory", Type.GetType("ItemsInventory"), false);
        InstantiateContainer("Teams", "Assets/Sprites/Items/ItemsInventory", Type.GetType("ItemsInventory"), false);
    }

    private void InstantiateContainer(string containerName, string folderPath, Type componentName, bool leftSide) {
        GameObject invCont = Instantiate(inventoryContainerPrefab);
        invCont.transform.SetParent(containerParent, false);

        RectTransform parentTR = (RectTransform)containerParent.transform;
        RectTransform rectTR = (RectTransform)invCont.transform;
        float invHeight = parentTR.rect.height;
        float invWidth = parentTR.rect.width;

        if (leftSide) {
            rectTR.sizeDelta = new Vector2(invWidth * 50f / 100, invHeight * 90f / 100);
            rectTR.transform.localPosition = new Vector3(-invWidth * 24f / 100, -invHeight * 3f / 100, 0);
        }
        else {
            rectTR.sizeDelta = new Vector2(invWidth * 50f / 100, invHeight * 90f / 100);
            rectTR.transform.localPosition = new Vector3(invWidth * 25f / 100, -invHeight * 3f / 100, 0);
        }

        inventoryCanvas.GetComponent<InventoryUI>().AddInvContainer(invCont);

        invCont.name = containerName + "Container";
        GameObject invContent = GameObject.Find("Content");
        invContent.name = containerName + "Content";
        invContent.AddComponent(componentName);
        AddInvComponent<BaseInventory>(invContent, folderPath);
    }

    private void AddInvComponent<T>(GameObject invCont, string folderPath) where T : BaseInventory {
        invCont.GetComponent<T>().emptyItemPrefab = invItemsPrefabs.Find(item => item.name == "InventorySlot");
        invCont.GetComponent<T>().folderPath = folderPath;
    }
}