using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour {
    public ArrayLayout boardLayout;

    [Header("UI Elements")]
    public Sprite[] pieces;

    public RectTransform gameBoard;

    [Header("Prefabs")]
    public GameObject nodePiece;

    private int width = 8;
    private int height = 8;
    private int[] fills;
    private Node[,] board;

    private List<NodePiece> update;
    private List<FlippedPieces> flipped;
    private List<NodePiece> dead;

    public event System.Action attackTriggered;

    public event System.Action<int, string> gainManaTriggered;

    public event System.Action turnChangeTriggered;

    private bool isTurnEnd = false;

    private System.Random random;

    private void Start() {
        StartGame();
    }

    public Node[,] GetBoard() {
        return board;
    }

    private void StartGame() {
        fills = new int[width];
        string seed = GetRandomSeed();
        random = new System.Random(seed.GetHashCode());
        update = new List<NodePiece>();
        dead = new List<NodePiece>();
        flipped = new List<FlippedPieces>();
        BattleStateHandler.setState(BattleState.Start);

        InitializeBoard();
        VerifyBoard();
        InstantiateBoard();
    }

    private void InitializeBoard() {
        board = new Node[width, height];
        for (int y = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                board[x, y] = new Node((boardLayout.rows[y].row[x]) ? -1 : FillPiece(), new Point(x, y));
            }
        }
    }

    private void InstantiateBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Node node = GetNodeAtPoint(new Point(x, y));
                int val = node.value;
                if (val <= 0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece piece = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(64 + (130 * x), -64 - (136 * y));
                piece.Initialize(val, new Point(x, y), pieces[val - 1]);
                node.SetPiece(piece);
            }
        }

        BattleStateHandler.setState(BattleState.WaitingForPlayer);
    }

    private void VerifyBoard() {
        List<int> remove = new List<int>();
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Point point = new Point(x, y);
                int val = GetValueAtPoint(point);
                if (val <= 0) continue;

                remove = new List<int>();
                while (IsConnected(point, true).Count > 0) {
                    val = GetValueAtPoint(point);
                    if (!remove.Contains(val)) {
                        remove.Add(val);
                    }
                    SetValueAtPoint(point, NewValue(ref remove));
                }
            }
        }
    }

    public void ApplyGravityToBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = (height - 1); y >= 0; y--) {
                Point p = new Point(x, y);
                Node node = GetNodeAtPoint(p);
                int val = GetValueAtPoint(p);

                if (val != 0) continue;
                for (int ny = (y - 1); ny >= -1; ny--) {
                    Point next = new Point(x, ny);
                    int nextVal = GetValueAtPoint(next);
                    if (nextVal == 0) continue;
                    if (nextVal != -1) // fill the current hole
                    {
                        Node got = GetNodeAtPoint(next);
                        NodePiece piece = got.GetPiece();

                        node.SetPiece(piece);
                        update.Add(piece);

                        got.SetPiece(null);
                    }
                    else {
                        int newVal = FillPiece();
                        NodePiece piece;
                        Point fallPoint = new Point(x, -1 - fills[x]);
                        if (dead.Count > 0) {
                            NodePiece revived = dead[0];
                            revived.gameObject.SetActive(true);
                            piece = revived;

                            dead.RemoveAt(0);
                        }
                        else {
                            GameObject obj = Instantiate(nodePiece, gameBoard);
                            NodePiece n = obj.GetComponent<NodePiece>();
                            piece = n;
                        }
                        piece.Initialize(newVal, p, pieces[newVal - 1]);
                        piece.rect.anchoredPosition = GetPositionFromPoint(fallPoint);

                        Node hole = GetNodeAtPoint(p);
                        hole.SetPiece(piece);
                        ResetPiece(piece);
                        fills[x]++;
                    }

                    break;
                }
            }
        }
    }

    public int GetBoardWidth() {
        return width;
    }

    public int GetBoardHeight() {
        return height;
    }

    public void ResetPiece(NodePiece piece) {
        piece.ResetPosition();
        update.Add(piece);
    }

    public void FlipPieces(Point one, Point two, bool main) {
        if (GetValueAtPoint(one) < 0) return;

        Node nodeOne = GetNodeAtPoint(one);
        NodePiece pieceOne = nodeOne.GetPiece();

        if (GetValueAtPoint(two) > 0) {
            Node nodeTwo = GetNodeAtPoint(two);
            NodePiece pieceTwo = nodeTwo.GetPiece();
            nodeOne.SetPiece(pieceTwo);
            nodeTwo.SetPiece(pieceOne);

            if (main)
                flipped.Add(new FlippedPieces(pieceOne, pieceTwo));

            update.Add(pieceOne);
            update.Add(pieceTwo);
        }
        else ResetPiece(pieceOne);
    }

    private Node GetNodeAtPoint(Point point) {
        return board[point.x, point.y];
    }

    private int NewValue(ref List<int> remove) // Remove the value and change to different one
    {
        List<int> available = new List<int>();
        for (int i = 0; i < pieces.Length; i++)
            available.Add(i + 1);
        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0) return 0;

        return available[random.Next(0, available.Count)];
    }

    private int SetValueAtPoint(Point p, int v) {
        return board[p.x, p.y].value = v;
    }

    private List<Point> IsConnected(Point point, bool main) {
        List<Point> connected = new List<Point>();
        int val = GetValueAtPoint(point);
        Point[] directions = {
            Point.Up,
            Point.Right,
            Point.Down,
            Point.Left
        };

        foreach (Point dir in directions) //Checking if there is 2 same in the directions -> X X Y OR Y X X
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for (int i = 1; i < 3; i++) {
                Point check = Point.Add(point, Point.Mul(dir, i));
                if (GetValueAtPoint(check) == val) {
                    line.Add(check);
                    same++;
                }
            }

            if (same > 1) {
                AddPoints(ref connected, line);
            }
        }

        for (int i = 0; i < 2; i++) //Checking if we are in the middle  X Y X
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = {
                    Point.Add(point, directions[i]),
                    Point.Add(point, directions[i+2])
                };
            foreach (Point next in check) // Check both sides of the piece
            {
                if (GetValueAtPoint(next) == val) {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1) {
                AddPoints(ref connected, line);
            }

            if (BattleStateHandler.GetState() != BattleState.Start) {
                if (same == 2) { // When we are middle, check if can be an extra turn
                    Point[] check2 = {
                    Point.Add(point, Point.Mul(directions[i], 2)),
                    Point.Add(point, Point.Mul(directions[i+2], 2))
                };
                    foreach (Point next in check2) // Check both sides of the piece
                    {
                        if (GetValueAtPoint(next) == val) {
                            same++;
                        }
                    }
                }

                if (same > 2) {
                    //Debug.Log("extra turn");
                    ExtraTurnHandler.SetExtraTurn();
                }
            }
        }

        if (BattleStateHandler.GetState() != BattleState.Start && !ExtraTurnHandler.IsExtraTurn()) {
            for (int i = 0; i < 3; i += 2) //Checking if we are in the edge
            {
                int same = 0;
                int same2 = 0;
                Point[] checkEdge = {
                    Point.Add(point, directions[i]),
                    Point.Add(point, Point.Mul(directions[i], 2)),
                    Point.Add(point, directions[1]),
                    Point.Add(point, Point.Mul(directions[1], 2))
                };

                Point[] checkEdge2 = {
                    Point.Add(point, directions[i]),
                    Point.Add(point, Point.Mul(directions[i], 2)),
                    Point.Add(point, directions[3]),
                    Point.Add(point, Point.Mul(directions[3], 2))
                };

                foreach (Point next in checkEdge) {
                    if (GetValueAtPoint(next) == val)
                        same++;
                }

                foreach (Point next in checkEdge2) {
                    if (GetValueAtPoint(next) == val)
                        same2++;
                }

                if (same == 4 || same2 == 4) {
                    ExtraTurnHandler.SetExtraTurn();
                }
            }

            if (!ExtraTurnHandler.IsExtraTurn()) {
                for (int i = 0; i < 2; i++) //Checking if we are in T shape
                {
                    int same = 0;
                    int same2 = 0;

                    //Only 4 directions are in the array
                    int tempDir = 3;
                    if (i + 3 > 3) {
                        tempDir = 0;
                    }

                    Point[] checkTShape = {
                        Point.Add(point, directions[i]),
                        Point.Add(point, Point.Mul(directions[i], 2)),
                        Point.Add(point, directions[i+1]),
                        Point.Add(point, directions[tempDir]),
                    };

                    Point[] checkTShape2 = {
                        Point.Add(point, directions[i+2]),
                        Point.Add(point, Point.Mul(directions[i+2], 2)),
                        Point.Add(point, directions[i+1]),
                        Point.Add(point, directions[tempDir]),
                    };

                    foreach (Point next in checkTShape) {
                        if (GetValueAtPoint(next) == val)
                            same++;
                    }

                    foreach (Point next in checkTShape2) {
                        if (GetValueAtPoint(next) == val)
                            same2++;
                    }

                    if (same == 4 || same2 == 4) {
                        ExtraTurnHandler.SetExtraTurn();
                    }
                }
            }
        }

        if (main) // Check if there is more match can be
    {
            for (int i = 0; i < connected.Count; i++) {
                AddPoints(ref connected, IsConnected(connected[i], false));
            }
        }

        return connected;
    }

    private void AddPoints(ref List<Point> points, List<Point> add) {
        foreach (Point p in add) {
            bool doAdd = true;
            for (int i = 0; i < points.Count; i++) {
                if (points[i].Equals(p)) {
                    doAdd = false;
                    break;
                }
            }
            if (doAdd) points.Add(p);
        }
    }

    public int GetValueAtPoint(Point point) {
        if (point.x < 0 || point.x >= width || point.y < 0 || point.y >= height) return -1;
        return board[point.x, point.y].value;
    }

    private int FillPiece() {
        int val = (random.Next(0, 350) / (350 / pieces.Length)) + 1;
        return val;
    }

    private void Update() {
        List<NodePiece> finishedUpdating = new List<NodePiece>();
        List<Point> connected = new List<Point>();

        for (int i = 0; i < update.Count; i++) {
            NodePiece piece = update[i];
            if (!piece.UpdatePiece())
                finishedUpdating.Add(piece);
        }

        for (int i = 0; i < finishedUpdating.Count; i++) {
            NodePiece piece = finishedUpdating[i];
            FlippedPieces flip = GetFlipped(piece);
            NodePiece flippedPiece = null;

            int x = (int)piece.index.x;
            fills[x] = Mathf.Clamp(fills[x] - 1, 0, width);

            connected = IsConnected(piece.index, true);
            bool wasFlipped = (flip != null);

            if (wasFlipped) // ha felcseréltük akkor hívjuk meg az update-t
            {
                flippedPiece = flip.GetOtherPiece(piece);
                AddPoints(ref connected, IsConnected(flippedPiece.index, true));
            }

            if (connected.Count == 0) {
                if (wasFlipped)
                    FlipPieces(piece.index, flippedPiece.index, false); // If we flipped the piece, but no match - reverse

                if (!wasFlipped && update.Count == 1)
                    isTurnEnd = true;
            }
            else    // If there is a match
            {
                if (wasFlipped) { // If we flipped the piece and match
                    if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer) {
                        BattleStateHandler.setState(BattleState.PlayerTurn);
                    }
                    else if (BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
                        BattleStateHandler.setState(BattleState.EnemyTurn);
                    }
                }

                MatchAction(connected);

                foreach (Point pnt in connected) // Remove the connected ones
                {
                    Node node = GetNodeAtPoint(pnt);
                    NodePiece nodePiece = node.GetPiece();

                    if (piece != null) {
                        nodePiece.gameObject.SetActive(false);
                        dead.Add(nodePiece);
                    }
                    node.SetPiece(null);
                }
                ApplyGravityToBoard();
            }

            flipped.Remove(flip);
            update.Remove(piece);
        }
    }

    private void LateUpdate() {
        if (isTurnEnd) {
            if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
                BattleStateHandler.setState(ExtraTurnHandler.IsExtraTurn() ? BattleState.WaitingForPlayer : BattleState.WaitingForEnemy);
                //Debug.Log(BattleStateHandler.GetState());
            }
            else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
                BattleStateHandler.setState(ExtraTurnHandler.IsExtraTurn() ? BattleState.WaitingForEnemy : BattleState.WaitingForPlayer);
                //Debug.Log(BattleStateHandler.GetState());
            }
            turnChangeTriggered?.Invoke();
            ExtraTurnHandler.ResetExtraTurn();
            isTurnEnd = false;
        }
    }

    private void MatchAction(List<Point> points) {
        List<MatchedGem> matchGems = new List<MatchedGem>();
        MatchedGem gem;

        foreach (Point point in points) {
            Node node = GetNodeAtPoint(point);
            NodePiece nodePiece = node.GetPiece();
            int pieceValue = nodePiece.GetValue();

            if (matchGems.Find(MatchGems => MatchGems.colorCode == pieceValue) == null) {
                gem = new MatchedGem(1, pieceValue);
                matchGems.Add(gem);
            }
            else {
                gem = matchGems.Find(MatchGems => MatchGems.colorCode == pieceValue);
                gem.IncreaseCount();
            }
        }

        foreach (MatchedGem matchedGem in matchGems) {
            if (matchedGem.colorCode == 1) {
                if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer) {
                    BattleStateHandler.setState(BattleState.PlayerTurn);
                }
                else if (BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
                    BattleStateHandler.setState(BattleState.EnemyTurn);
                }
                attackTriggered?.Invoke();
            }

            if (matchedGem.colorCode > 1) {
                string color = null;
                int manaGain = 0;

                switch (matchedGem.colorCode) {
                    case 2:
                        color = "Purple";
                        break;

                    case 3:
                        color = "Green";
                        break;

                    case 4:
                        color = "Red";
                        break;

                    case 5:
                        color = "Blue";
                        break;

                    case 6:
                        color = "Yellow";
                        break;

                    case 7:
                        color = "Brown";
                        break;

                    default:
                        color = null;
                        break;
                }

                if (matchedGem.count >= 5) {
                    manaGain = 5;
                }
                else if (matchedGem.count == 4) {
                    manaGain = 4;
                }
                else {
                    manaGain = 3;
                }

                gainManaTriggered?.Invoke(manaGain, color);
            }
        }
    }

    private FlippedPieces GetFlipped(NodePiece p) {
        FlippedPieces flip = null;
        for (int i = 0; i < flipped.Count; i++) {
            if (flipped[i].GetOtherPiece(p) != null) {
                flip = flipped[i];
                break;
            }
        }
        return flip;
    }

    private string GetRandomSeed() {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";

        for (int i = 0; i < 20; i++) {
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        }

        return seed;
    }

    public Vector2 GetPositionFromPoint(Point point) {
        return new Vector2(64 + (130 * point.x), -64 - (136 * point.y));
    }

    private class MatchedGem {
        public int count { get; set; }
        public int colorCode { get; set; }

        public MatchedGem(int count, int colorCode) {
            this.count = count;
            this.colorCode = colorCode;
        }

        public void IncreaseCount() {
            count++;
        }
    }
}

[System.Serializable]
public class Node {

    //0 = üres, 1 = skull, 2 = amethyst, 3 = emerald, 4 = ruby, 5 = sapphire, 6 = topaz, 7 = turmaline, -1 = hole
    public int value; //Az adott mezőn található Gem értéke

    public Point index;
    private NodePiece piece;

    public Node(int v, Point i) {
        value = v;
        index = i;
    }

    public void SetPiece(NodePiece nodePiece) {
        piece = nodePiece;
        value = (piece == null) ? 0 : piece.value;
        if (piece == null) return;
        piece.SetIndex(index);
    }

    public NodePiece GetPiece() {
        return piece;
    }
}

[System.Serializable]
public class FlippedPieces {
    public NodePiece one;
    public NodePiece two;

    public FlippedPieces(NodePiece o, NodePiece t) {
        one = o;
        two = t;
    }

    public NodePiece GetOtherPiece(NodePiece piece) {
        if (piece == one)
            return two;
        else if (piece == two)
            return one;
        else
            return null;
    }
}