using UnityEngine;

public class MovePieces : MonoBehaviour {
    private Match3 game;
    private NodePiece moving;
    private Point newIndex;
    private Vector2 mouseStart;
    public static MovePieces instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        game = GetComponent<Match3>();
    }

    private void Update() {
        if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer) {
            PlayerMove();
        }
    }

    private void PlayerMove() {
        if (moving != null) {
            Vector2 dir = ((Vector2)Input.mousePosition - mouseStart);
            Vector2 nDir = dir.normalized;
            Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

            newIndex = Point.Clone(moving.index);
            Point add = Point.Zero;
            if (dir.magnitude > 32) //When clicked and moved the mouse at least 32 pixel
            {
                if (aDir.x > aDir.y)
                    add = (new Point((nDir.x > 0) ? 1 : -1, 0));
                else if (aDir.y > aDir.x)
                    add = (new Point(0, (nDir.y > 0) ? -1 : 1));
            }
            newIndex.Add(add);

            Vector2 pos = game.GetPositionFromPoint(moving.index);
            if (!newIndex.Equals(moving.index)) //Move the gem to that direction
                pos += Point.Mul(new Point(add.x, -add.y), 64).ToVector();
            moving.MovePositionTo(pos);
        }
    }

    public void EnemyMove(NodePiece from, NodePiece to) {
        moving = from;
        mouseStart = from.transform.position;

        Vector2 dir = ((Vector2)to.transform.position - mouseStart);
        Vector2 nDir = dir.normalized;
        Vector2 aDir = new Vector2(Mathf.Abs(dir.x), Mathf.Abs(dir.y));

        newIndex = Point.Clone(moving.index);
        Point add = Point.Zero;
        if (dir.magnitude > 32) //When clicked and moved the mouse at least 32 pixel
        {
            if (aDir.x > aDir.y)
                add = (new Point((nDir.x > 0) ? 1 : -1, 0));
            else if (aDir.y > aDir.x)
                add = (new Point(0, (nDir.y > 0) ? -1 : 1));
        }
        newIndex.Add(add);

        Vector2 pos = game.GetPositionFromPoint(moving.index);
        if (!newIndex.Equals(moving.index))  //Move the gem to that direction
            pos += Point.Mul(new Point(add.x, -add.y), 64).ToVector();
        moving.MovePositionTo(pos);

        DropPiece();
    }

    public void MovePiece(NodePiece piece) {
        if (moving != null) return;
        moving = piece;

        if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            mouseStart = piece.transform.position;
        }
        else
            mouseStart = Input.mousePosition;
    }

    public void DropPiece() {
        if (moving == null) return;

        if (!newIndex.Equals(moving.index)) {
            game.FlipPieces(moving.index, newIndex, true);
        }
        else
            game.ResetPiece(moving);

        moving = null;
    }
}