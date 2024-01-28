using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    public GameObject originalGO;
    public static GameObject copyObject;
    public static ItemSlot copyItem;

    private static Canvas canvas;
    private RectTransform parentTR;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public void OnBeginDrag(PointerEventData eventData) {
        copyObject = Instantiate(originalGO);
        copyItem = originalGO.GetComponentInParent<ItemSlot>();
        copyObject.transform.SetParent(parentTR, false);
        copyObject.transform.position = new Vector3(originalGO.transform.position.x, originalGO.transform.position.y);

        if (copyObject.GetComponent<EquipmentDisplay>() != null) {
            copyObject.GetComponent<EquipmentDisplay>().SetEquipmentDisplay(copyItem.item);
        }

        rectTransform = copyObject.GetComponent<RectTransform>();
        rectTransform.SetAsLastSibling();

        canvasGroup = copyObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData) {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        copyItem = null;
        Destroy(copyObject);
    }

    public void SetCanvas(Canvas canvasIn) {
        canvas = canvasIn;
    }

    public void SetParent(RectTransform parent) {
        parentTR = parent;
    }
}