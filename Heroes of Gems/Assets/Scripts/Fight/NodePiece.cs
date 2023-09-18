using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodePiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public int value;
    public Point index;
    private bool updating;

    [HideInInspector]
    public Vector2 pos;

    [HideInInspector]
    public RectTransform rect;

    private Image image;

    public void Initialize(int v, Point point, Sprite piece) {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();

        value = v;
        SetIndex(point);
        image.sprite = piece;
    }

    public void SetIndex(Point p) {
        index = p;
        ResetPosition();
        UpdateName();
    }

    public void ResetPosition() {
        pos = new Vector2(64 + (128 * index.x), -64 - (128 * index.y));
    }

    private void UpdateName() {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public bool updatePiece() {
        if (Vector3.Distance(rect.anchoredPosition, pos) > 1) {
            MovePositionTo(pos);
            updating = true;
            return true;
        }
        else // return false ha nem mozdítjuk
        {
            rect.anchoredPosition = pos;
            updating = false;
            return false;
        }
    }

    public void MovePositionTo(Vector2 move) {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 32f);
    }

    public void MovePosition(Vector2 move) {
        rect.anchoredPosition += move * Time.deltaTime * 32f;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (updating) return;
        MovePieces.instance.MovePiece(this);
    }

    public void OnPointerUp(PointerEventData eventData) {
        MovePieces.instance.DropPiece();
    }
}