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
    private int originalOrder;

    public void Initialize(int v, Point point, Sprite piece) {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        originalOrder = rect.GetSiblingIndex();

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
        pos = new Vector2(64 + (130 * index.x), -64 - (136 * index.y));
    }

    private void UpdateName() {
        transform.name = "Node [" + index.x + ", " + index.y + "]";
    }

    public bool UpdatePiece() {
        if (Vector3.Distance(rect.anchoredPosition, pos) > 1) {
            MovePositionTo(pos);
            updating = true;
            return true;
        }
        else // return false if we didn't moved
        {
            rect.anchoredPosition = pos;
            updating = false;
            return false;
        }
    }

    public void MovePositionTo(Vector2 move) {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 25f);
    }

    public void MovePosition(Vector2 move) {
        rect.anchoredPosition += move * Time.deltaTime * 25f;
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer) {
            if (updating) return;
            rect.SetAsLastSibling();
            MovePieces.instance.MovePiece(this);
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer) {
            rect.SetSiblingIndex(originalOrder);
            MovePieces.instance.DropPiece();
        }
    }

    public int GetValue() {
        return value;
    }
}