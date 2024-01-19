using UnityEngine;
using UnityEngine.EventSystems;

public class DragUnit : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {
    private static Canvas canvas;
    public GameObject originalGO;
    private RectTransform rectTransform;
    public static GameObject copyObject;
    public static UnitItem copyUnit;
    private RectTransform parentTR;

    private CanvasGroup canvasGroup;

    public void OnBeginDrag(PointerEventData eventData) {
        copyObject = Instantiate(originalGO);
        copyUnit = originalGO.GetComponentInParent<UnitItem>();
        copyObject.transform.SetParent(parentTR, false);
        copyObject.transform.position = new Vector3(originalGO.transform.position.x, originalGO.transform.position.y);

        if (copyObject.GetComponent<TeamSlotDisplay>() != null) {
            copyObject.GetComponent<TeamSlotDisplay>().SetMemberDisplay(copyUnit.unit);
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

        copyUnit = null;
        Destroy(copyObject);
    }

    public void SetCanvas(Canvas canvasIn) {
        canvas = canvasIn;
    }

    public void SetParent(RectTransform parent) {
        parentTR = parent;
    }
}