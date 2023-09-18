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

    private bool extraTurn = false;
    private bool isMatch = false;
    private bool isUpdate = false;

    private System.Random random;

    private void Start() {
        StartGame();
    }

    private void StartGame() {
        fills = new int[width];
        string seed = getRandomSeed();
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
                board[x, y] = new Node((boardLayout.rows[y].row[x]) ? -1 : fillPiece(), new Point(x, y));
            }
        }
    }

    private void InstantiateBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                Node node = getNodeAtPoint(new Point(x, y));
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
                int val = getValueAtPoint(point);
                if (val <= 0) continue;

                remove = new List<int>();
                while (isConnected(point, true).Count > 0) {
                    val = getValueAtPoint(point);
                    if (!remove.Contains(val)) {
                        remove.Add(val);
                    }
                    setValueAtPoint(point, newValue(ref remove));
                }
            }
        }
    }

    public void ApplyGravityToBoard() {
        for (int x = 0; x < width; x++) {
            for (int y = (height - 1); y >= 0; y--) {
                Point p = new Point(x, y);
                Node node = getNodeAtPoint(p);
                int val = getValueAtPoint(p);

                if (val != 0) continue;
                for (int ny = (y - 1); ny >= -1; ny--) {
                    Point next = new Point(x, ny);
                    int nextVal = getValueAtPoint(next);
                    if (nextVal == 0) continue;
                    if (nextVal != -1) // fill the current hole
                    {
                        Node got = getNodeAtPoint(next);
                        NodePiece piece = got.getPiece();

                        node.SetPiece(piece);
                        update.Add(piece);

                        got.SetPiece(null);
                    }
                    else {
                        int newVal = fillPiece();
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
                        piece.rect.anchoredPosition = getPositionFromPoint(fallPoint);

                        Node hole = getNodeAtPoint(p);
                        hole.SetPiece(piece);
                        ResetPiece(piece);
                        fills[x]++;
                    }

                    break;
                }
            }
        }
    }

    public void ResetPiece(NodePiece piece) {
        piece.ResetPosition();
        update.Add(piece);
    }

    public void FlipPieces(Point one, Point two, bool main) {
        if (getValueAtPoint(one) < 0) return;

        Node nodeOne = getNodeAtPoint(one);
        NodePiece pieceOne = nodeOne.getPiece();

        if (getValueAtPoint(two) > 0) {
            Node nodeTwo = getNodeAtPoint(two);
            NodePiece pieceTwo = nodeTwo.getPiece();
            nodeOne.SetPiece(pieceTwo);
            nodeTwo.SetPiece(pieceOne);

            if (main)
                flipped.Add(new FlippedPieces(pieceOne, pieceTwo));

            update.Add(pieceOne);
            update.Add(pieceTwo);
        }
        else ResetPiece(pieceOne);
    }

    private Node getNodeAtPoint(Point point) {
        return board[point.x, point.y];
    }

    private int newValue(ref List<int> remove) // Kivesszük az értéket és helyette teszünk bele egy másikat, úgy hogy az ne legyen ütközés az ellőzővel
    {
        List<int> available = new List<int>();
        for (int i = 0; i < pieces.Length; i++)
            available.Add(i + 1);
        foreach (int i in remove)
            available.Remove(i);

        if (available.Count <= 0) return 0;

        return available[random.Next(0, available.Count)];
    }

    private int setValueAtPoint(Point p, int v) {
        return board[p.x, p.y].value = v;
    }

    private List<Point> isConnected(Point point, bool main) {
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(point);
        Point[] directions = {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };

        foreach (Point dir in directions) //Checking if there is 2 more same in the directions
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for (int i = 1; i < 4; i++) {
                Point check = Point.add(point, Point.mul(dir, i));
                if (getValueAtPoint(check) == val) {
                    line.Add(check);
                    same++;
                }
            }
            if (same > 1) {
                AddPoints(ref connected, line);
            }

            if (BattleStateHandler.GetState() != BattleState.Start) {
                if (same > 2) {
                    //Debug.Log("extra turn");
                    extraTurn = true;
                }
            }
        }

        for (int i = 0; i < 2; i++) //Checking if we are in the middle
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { //Megnézi az egyik illetve a másik irányban is, hogy van-e match
                    Point.add(point, directions[i]),
                    Point.add(point, directions[i+2])
                };
            foreach (Point next in check) // Check both sides of the piece
            {
                if (getValueAtPoint(next) == val) {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1) {
                AddPoints(ref connected, line);
            }
        }

        if (main) //Megnézi hogy van e több match is az adott pontban
        {
            for (int i = 0; i < connected.Count; i++) {
                AddPoints(ref connected, isConnected(connected[i], false));
            }
        }

        /*if (connected.Count > 0)
            connected.Add(point);*/

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

    private int getValueAtPoint(Point point) {
        if (point.x < 0 || point.x >= width || point.y < 0 || point.y >= height) return -1;
        return board[point.x, point.y].value;
    }

    private int fillPiece() {
        int val = 1;
        val = (random.Next(0, 350) / (350 / pieces.Length)) + 1;
        return val;
    }

    private void Update() {
        List<NodePiece> finishedUpdating = new List<NodePiece>();
        List<Point> connected = new List<Point>();
        if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer || BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
            for (int i = 0; i < update.Count; i++) {
                NodePiece piece = update[i];
                if (!piece.updatePiece())
                    finishedUpdating.Add(piece);
            }

            for (int i = 0; i < finishedUpdating.Count; i++) {
                NodePiece piece = finishedUpdating[i];
                FlippedPieces flip = getFlipped(piece);
                NodePiece flippedPiece = null;

                int x = (int)piece.index.x;
                fills[x] = Mathf.Clamp(fills[x] - 1, 0, width);

                connected = isConnected(piece.index, true);
                bool wasFlipped = (flip != null);

                if (wasFlipped) // ha felcseréltük akkor hívjuk meg az update-t
                {
                    flippedPiece = flip.getOtherPiece(piece);
                    AddPoints(ref connected, isConnected(flippedPiece.index, true));
                }

                if (connected.Count == 0) // Ha nincs matchünk
                {
                    if (wasFlipped) // Ha felcseréltük
                        FlipPieces(piece.index, flippedPiece.index, false); //Visszacserélés
                }
                else    // Ha match van
                {
                    isMatch = true;
                    if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer) {
                        BattleStateHandler.setState(BattleState.PlayerTurn);
                    }
                    else if (BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
                        BattleStateHandler.setState(BattleState.EnemyTurn);
                    }
                    //Debug.Log(BattleStateHandler.GetState() + " was");

                    matchAction(connected[0]);

                    foreach (Point pnt in connected) //Összekapcsoltakat kivesszük
                    {
                        Node node = getNodeAtPoint(pnt);
                        NodePiece nodePiece = node.getPiece();

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
    }

    private void LateUpdate() {
        if (isMatch) {
            if (BattleStateHandler.GetState() == BattleState.PlayerTurn) {
                BattleStateHandler.setState((extraTurn) ? BattleState.WaitingForPlayer : BattleState.WaitingForEnemy);
                Debug.Log(BattleStateHandler.GetState() + " changed to");
            }
            else if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
                BattleStateHandler.setState((extraTurn) ? BattleState.WaitingForEnemy : BattleState.WaitingForPlayer);
                Debug.Log(BattleStateHandler.GetState() + " changed to");
            }
            extraTurn = false;
            isMatch = false;
        }
    }

    //private bool isStillUpdating() {
    //    List<int> remove = new List<int>();
    //    for (int x = 0; x < width; x++) {
    //        for (int y = 0; y < height; y++) {
    //            Point point = new Point(x, y);
    //            int val = getValueAtPoint(point);
    //            if (val <= 0) continue;

    //            while (isConnected(point, true).Count > 0) {
    //                val = getValueAtPoint(point);
    //                if (!remove.Contains(val)) {
    //                    remove.Add(val);
    //                }
    //                //setValueAtPoint(point, newValue(ref remove));
    //            }
    //        }
    //    }
    //    if (remove.Count > 0) {
    //        return true;
    //    }
    //    else
    //        return false;
    //}

    private void matchAction(Point point) {
        Node node = getNodeAtPoint(point);
        NodePiece nodePiece = node.getPiece();
        int pieceValue = nodePiece.GetValue();

        if (pieceValue == 1) {
            if (BattleStateHandler.GetState() == BattleState.WaitingForPlayer) {
                BattleStateHandler.setState(BattleState.PlayerTurn);
            }
            else if (BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
                BattleStateHandler.setState(BattleState.EnemyTurn);
            }
            attackTriggered?.Invoke();
        }
    }

    private FlippedPieces getFlipped(NodePiece p) {
        FlippedPieces flip = null;
        for (int i = 0; i < flipped.Count; i++) {
            if (flipped[i].getOtherPiece(p) != null) {
                flip = flipped[i];
                break;
            }
        }
        return flip;
    }

    private string getRandomSeed() {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";

        for (int i = 0; i < 20; i++) {
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        }

        return seed;
    }

    public Vector2 getPositionFromPoint(Point point) {
        return new Vector2(64 + (130 * point.x), -64 - (136 * point.y));
    }
}

[System.Serializable]
public class Node {

    //0 = üres, 1 = amethyst, 2 = emerald, 3 = sapphire, 4 = ruby, 5 = topaz, 6-turmaline, -1 = hole
    //0 = üres, 1 = skull, 2 = amethyst, 3 = emerald, 4 = sapphire, 5 = ruby, 6 = topaz, 7 = turmaline, -1 = hole
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

    public NodePiece getPiece() {
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

    public NodePiece getOtherPiece(NodePiece piece) {
        if (piece == one)
            return two;
        else if (piece == two)
            return one;
        else
            return null;
    }
}