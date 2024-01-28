using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {
    [SerializeField] private GameObject inventoryCanvas = default;
    [SerializeField] private GameObject inventoryContainerPrefab = default;

    [SerializeField] private Transform containerParent = default;
    [SerializeField] private List<GameObject> invItemsPrefabs = default;
    private GameObject teamsPrefab;
    private GameObject equipmentPrefab;
    private GameObject equipmentSlotPrefab;

    private void Start() {
        teamsPrefab = invItemsPrefabs.Find(item => item.name == "TeamsPrefab");
        equipmentPrefab = invItemsPrefabs.Find(item => item.name == "EquipmentContainer");
        equipmentSlotPrefab = invItemsPrefabs.Find(item => item.name == "EquipmentSlot");
        InstantiateContainer("Units", "Assets/Sprites/Cards", Type.GetType("UnitsInventory"), true);
        InstantiateContainer("Items", "Assets/Sprites/Items/ItemsInventory", Type.GetType("ItemsInventory"), true);
        InstantiateContainer("Equipments", equipmentSlotPrefab, Type.GetType("EquipmentsInventory"), false, equipmentPrefab);
        InstantiateContainer("Teams", teamsPrefab, Type.GetType("TeamsInventory"), false);

        GetComponentInParent<InventoryUI>().InitialTitles();
    }

    private void InstantiateContainer(string containerName, GameObject prefab, Type componentName, bool leftSide, GameObject containerPrefab) {
        GameObject invCont = Instantiate(containerPrefab);
        invCont.name = containerName + "Container";
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
            rectTR.sizeDelta = new Vector2(invWidth * 45f / 100, invHeight * 90f / 100);
            rectTR.transform.localPosition = new Vector3(invWidth * 26f / 100, -invHeight * 3f / 100, 0);
        }

        inventoryCanvas.GetComponent<InventoryUI>().AddInvContainer(invCont);

        invCont.AddComponent(componentName);
        AddInvComponent<BaseInventory>(invCont, prefab);
    }

    private void InstantiateContainer(string containerName, GameObject prefab, Type componentName, bool leftSide) {
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
            rectTR.sizeDelta = new Vector2(invWidth * 45f / 100, invHeight * 90f / 100);
            rectTR.transform.localPosition = new Vector3(invWidth * 26f / 100, -invHeight * 3f / 100, 0);
        }

        inventoryCanvas.GetComponent<InventoryUI>().AddInvContainer(invCont);

        invCont.name = containerName + "Container";
        GameObject invContent = GameObject.Find("Content");
        invContent.name = containerName + "Content";

        StartCoroutine(SwitchGridLayout(invContent));

        invContent.AddComponent(componentName);
        AddInvComponent<BaseInventory>(invContent, prefab);

        GameObject viewPort = GameObject.Find("Viewport");
        viewPort.name = containerName + viewPort.name;
    }

    private IEnumerator SwitchGridLayout(GameObject content) {
        GridLayoutGroup deleteGrid = content.GetComponent<GridLayoutGroup>();

        Destroy(deleteGrid);
        yield return null;
        content.AddComponent<VerticalLayoutGroup>();
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
            rectTR.sizeDelta = new Vector2(invWidth * 45f / 100, invHeight * 90f / 100);
            rectTR.transform.localPosition = new Vector3(invWidth * 26f / 100, -invHeight * 3f / 100, 0);
        }

        inventoryCanvas.GetComponent<InventoryUI>().AddInvContainer(invCont);

        invCont.name = containerName + "Container";
        GameObject invContent = GameObject.Find("Content");
        invContent.name = containerName + "Content";
        invContent.AddComponent(componentName);
        AddInvComponent<BaseInventory>(invContent, folderPath);

        GameObject viewPort = GameObject.Find("Viewport");
        viewPort.name = containerName + viewPort.name;
    }

    private void AddInvComponent<T>(GameObject invContent, string folderPath) where T : BaseInventory {
        invContent.GetComponent<T>().emptyItemPrefab = invItemsPrefabs.Find(item => item.name == "InventorySlot");
        invContent.GetComponent<T>().inventoryCanvas = inventoryCanvas;
        invContent.GetComponent<T>().container = (RectTransform)containerParent.transform;
        invContent.GetComponent<T>().folderPath = folderPath;

        List<GameObject> inventoryPrefabs = new List<GameObject>();
        inventoryPrefabs = invItemsPrefabs.FindAll(other => other.name != "InventorySlot");
        invContent.GetComponent<T>().otherPrefabs = inventoryPrefabs;
    }

    private void AddInvComponent<T>(GameObject invContent, GameObject prefab) where T : BaseInventory {
        invContent.GetComponent<T>().emptyItemPrefab = prefab;
        invContent.GetComponent<T>().inventoryCanvas = inventoryCanvas;
        List<GameObject> inventoryPrefabs = new List<GameObject>();
        inventoryPrefabs = invItemsPrefabs.FindAll(other => other.name != "InventorySlot");
        invContent.GetComponent<T>().otherPrefabs = inventoryPrefabs;
    }
}