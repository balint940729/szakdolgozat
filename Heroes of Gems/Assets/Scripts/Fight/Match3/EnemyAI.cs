using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    public struct MoveValue {
        public Point from;
        public Point to;
        public bool isExtraTurn;
        public int color;

        public MoveValue(Point from, Point to, bool isExtraTurn, int color) {
            this.from = from;
            this.to = to;
            this.isExtraTurn = isExtraTurn;
            this.color = color;
        }
    }

    private MoveValue bestMove;
    private List<GameObject> playerTeam = new List<GameObject>();
    private List<GameObject> enemyTeam = new List<GameObject>();

    // Update is called once per frame
    private void Update() {
        if (BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
            BattleStateHandler.SetState(BattleState.EnemyTurn);

            SetTeams();
            CheckSpellCast();
            CalculateMove();

            RemoveTeams();

            if (!bestMove.Equals(default(MoveValue))) {
                GameObject fromGO = GameObject.Find("Node [" + bestMove.from.x + ", " + bestMove.from.y + "]");
                GameObject toGO = GameObject.Find("Node [" + bestMove.to.x + ", " + bestMove.to.y + "]");
                NodePiece fromNode = fromGO.GetComponent<NodePiece>();
                NodePiece toNode = toGO.GetComponent<NodePiece>();
                MovePieces.instance.EnemyMove(fromNode, toNode);
                bestMove = default;
            }
        }
    }

    private void CalculateMove() {
        if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            Node[,] board = FindObjectOfType<Match3>().GetBoard();
            int[,] boardValues = new int[board.GetLength(0), board.GetLength(1)];
            List<MoveValue> possibleMoves = new List<MoveValue>();

            for (int i = 0; i < board.GetLength(0); i++) {
                for (int j = 0; j < board.GetLength(1); j++) {
                    Point point = new Point(i, j);
                    boardValues[i, j] = FindObjectOfType<Match3>().GetValueAtPoint(point);
                }
            }
            possibleMoves = GetPossibleMoves(boardValues);

            bestMove = CalculateBestMove(possibleMoves);

            if (bestMove.Equals(default(MoveValue))) {
                // No possible match
                Debug.Log("No match");
            }
        }
    }

    public MoveValue GetBestMove() {
        return bestMove;
    }

    private MoveValue CalculateBestMove(List<MoveValue> moveValues) {
        MoveValue bestMove;

        bestMove = moveValues.Find(em => em.isExtraTurn && em.color == 1); // Extra turn + skull
        if (bestMove.Equals(default(MoveValue))) {
            List<int> teamUsedColors = new List<int>();
            List<MoveValue> extraTurnMoves = moveValues.FindAll(pv => pv.isExtraTurn);

            foreach (GameObject unitGO in enemyTeam) {
                UnitController unit = unitGO.GetComponent<UnitController>();

                if (!unit.IsOnFullMana()) {
                    teamUsedColors.AddRange(unit.GetColors().Select(color => color.colorCode));
                }
            }

            if (extraTurnMoves.Count > 0) {
                // Extra turn + gain mana
                bestMove = extraTurnMoves.FirstOrDefault(move => teamUsedColors.Contains(move.color));

                if (bestMove.Equals(default(MoveValue))) {
                    // Extra turn
                    return extraTurnMoves.FirstOrDefault();
                }

                return bestMove;
            }

            //Skull
            bestMove = moveValues.FirstOrDefault(em => em.color == 1);

            if (bestMove.Equals(default(MoveValue))) {
                //Gain mana
                bestMove = moveValues.FirstOrDefault(move => teamUsedColors.Contains(move.color));
            }

            if (bestMove.Equals(default(MoveValue))) {
                // Other combination
                return moveValues.FirstOrDefault();
            }

            return bestMove;
        }

        return bestMove;
    }

    private void CheckSpellCast() {
        foreach (GameObject casterGO in TurnBase.GetInstance().GetEnemyTeam()) {
            UnitController caster = casterGO.GetComponent<UnitController>();
            if (caster.IsOnFullMana()) {
                CastSpellDelay(30);
                TurnBase.GetInstance().CastSpell(caster);
                break;
            }
        }
    }

    private IEnumerator<WaitForSeconds> CastSpellDelay(float second) {
        yield return new WaitForSeconds(second);
    }

    private void RemoveTeams() {
        foreach (GameObject unitGo in playerTeam) {
            Destroy(unitGo);
        }

        foreach (GameObject unitGo in enemyTeam) {
            Destroy(unitGo);
        }
    }

    private void SetTeams() {
        GameObject copiedGO;
        UnitController copiedUnit;
        foreach (GameObject unitGO in TurnBase.GetInstance().GetPlayerTeam()) {
            copiedGO = Instantiate(unitGO);
            copiedUnit = copiedGO.GetComponent<UnitController>();

            copiedUnit.CopyUnitController(unitGO.GetComponent<UnitController>());

            playerTeam.Add(copiedGO);
        }
        foreach (GameObject unitGO in TurnBase.GetInstance().GetEnemyTeam()) {
            copiedGO = Instantiate(unitGO);
            copiedUnit = copiedGO.GetComponent<UnitController>();

            copiedUnit.CopyUnitController(unitGO.GetComponent<UnitController>());

            playerTeam.Add(copiedGO);
        }
    }

    private List<MoveValue> GetPossibleMoves(int[,] boardValues) {
        List<MoveValue> moveValues = new List<MoveValue>();

        int[,] tempBoardValues = boardValues.Clone() as int[,];

        for (int x = 0; x < boardValues.GetLength(0); x++) {
            for (int y = 0; y < boardValues.GetLength(1); y++) {
                Point point = new Point(x, y);

                Point[] directions = {
                    Point.Up,
                    Point.Right,
                    Point.Down,
                    Point.Left
                };

                int width = FindObjectOfType<Match3>().GetBoardWidth();
                int height = FindObjectOfType<Match3>().GetBoardHeight();

                foreach (Point dir in directions) { //Checking if there is 2 same in the directions -> X X Y OR Y X X
                    Point newPoint = Point.Add(point, Point.Mul(dir, -1));
                    //Check if this valid move
                    if (newPoint.x < 0 || newPoint.x >= width || newPoint.y < 0 || newPoint.y >= height) continue;

                    tempBoardValues = boardValues.Clone() as int[,];

                    //Switch values
                    int tempValue = boardValues[point.x, point.y];
                    int tempValue2 = boardValues[newPoint.x, newPoint.y];

                    tempBoardValues[point.x, point.y] = tempValue2;
                    tempBoardValues[newPoint.x, newPoint.y] = tempValue;

                    int val = tempValue;

                    foreach (Point dir2 in directions) {
                        int same = 0;
                        for (int i = 1; i < 3; i++) {
                            Point check = Point.Add(newPoint, Point.Mul(dir2, i));
                            if (GetValueAtPoint(tempBoardValues, check) == val) {
                                same++;
                            }
                        }

                        if (same > 1) {
                            moveValues.Add(new MoveValue(point, newPoint, false, tempValue));
                        }
                    }

                    //Checking if we are in the middle  X Y X
                    for (int i = 0; i < 2; i++) {
                        int same = 0;
                        Point[] check = {
                            Point.Add(newPoint, directions[i]),
                            Point.Add(newPoint, directions[i+2])
                        };

                        // Check both sides of the piece
                        foreach (Point next in check) {
                            if (GetValueAtPoint(tempBoardValues, next) == val) {
                                same++;
                            }

                            if (same == 2) {
                                Point[] check2 = {
                                    Point.Add(newPoint, Point.Mul(directions[i], 2)),
                                    Point.Add(newPoint, Point.Mul(directions[i+2], 2))
                                };
                                foreach (Point next2 in check2) { // Check both sides of the piece
                                    if (GetValueAtPoint(tempBoardValues, next2) == val) {
                                        same++;
                                    }
                                }
                            }

                            if (same > 2) {
                                moveValues.Add(new MoveValue(point, newPoint, true, tempValue));
                            }
                            else if (same == 2) {
                                moveValues.Add(new MoveValue(point, newPoint, false, tempValue));
                            }
                        }
                    }

                    for (int i = 0; i < 3; i += 2) { //Checking if we are in the edge
                        int same = 0;
                        int same2 = 0;
                        Point[] checkEdge = {
                            Point.Add(newPoint, directions[i]),
                            Point.Add(newPoint, Point.Mul(directions[i], 2)),
                            Point.Add(newPoint, directions[1]),
                            Point.Add(newPoint, Point.Mul(directions[1], 2))
                        };

                        Point[] checkEdge2 = {
                            Point.Add(newPoint, directions[i]),
                            Point.Add(newPoint, Point.Mul(directions[i], 2)),
                            Point.Add(newPoint, directions[3]),
                            Point.Add(newPoint, Point.Mul(directions[3], 2))
                        };

                        foreach (Point next in checkEdge) {
                            if (GetValueAtPoint(tempBoardValues, next) == val)
                                same++;
                        }

                        foreach (Point next in checkEdge2) {
                            if (GetValueAtPoint(tempBoardValues, next) == val)
                                same2++;
                        }

                        if (same == 4 || same2 == 4) {
                            moveValues.Add(new MoveValue(point, newPoint, true, tempValue));
                        }
                    }

                    for (int i = 0; i < 2; i++) { //Checking if we are in T shape
                        int same = 0;
                        int same2 = 0;

                        //Only 4 directions are in the array
                        int tempDir = 3;
                        if (i + 3 > 3) {
                            tempDir = 0;
                        }

                        Point[] checkTShape = {
                            Point.Add(newPoint, directions[i]),
                            Point.Add(newPoint, Point.Mul(directions[i], 2)),
                            Point.Add(newPoint, directions[i+1]),
                            Point.Add(newPoint, directions[tempDir]),
                        };

                        Point[] checkTShape2 = {
                            Point.Add(newPoint, directions[i+2]),
                            Point.Add(newPoint, Point.Mul(directions[i+2], 2)),
                            Point.Add(newPoint, directions[i+1]),
                            Point.Add(newPoint, directions[tempDir]),
                        };

                        foreach (Point next in checkTShape) {
                            if (GetValueAtPoint(tempBoardValues, next) == val)
                                same++;
                        }

                        foreach (Point next in checkTShape2) {
                            if (GetValueAtPoint(tempBoardValues, next) == val)
                                same2++;
                        }

                        if (same == 4 || same2 == 4) {
                            moveValues.Add(new MoveValue(point, newPoint, true, tempValue));
                        }
                    }
                }
            }
        }

        return moveValues;
    }

    private int GetValueAtPoint(int[,] boardValues, Point point) {
        int width = FindObjectOfType<Match3>().GetBoardWidth();
        int height = FindObjectOfType<Match3>().GetBoardHeight();

        if (point.x < 0 || point.x >= width || point.y < 0 || point.y >= height) return -1;
        return boardValues[point.x, point.y];
    }

    //private MoveValue NewCalculateBestMove(int[,] boardValues, List<MoveValue> moveValues, bool isEnemyTurn, int depth, int alpha, int beta) {
    //    MoveValue bestMove = default(MoveValue);
    //    List<UnitController> allyTeam = new List<UnitController>();
    //    List<UnitController> oppentTeam = new List<UnitController>();
    //    List<int> teamUsedColors = new List<int>();
    //    List<MoveValue> possibleMoves = new List<MoveValue>();
    //    int tempPoint = 0;

    //    if (depth == 0) {
    //        return bestMove;
    //    }

    //    if (isEnemyTurn) {
    //        // Maximize for enemies' turn
    //        int maxScore = int.MinValue;

    //        foreach (MoveValue moveValue in moveValues) {
    //            int[,] newBoard = ApplyMove(boardValues, moveValue);

    //            possibleMoves = GetPossibleMoves(newBoard);

    //            MoveValue opponentMove = NewCalculateBestMove(newBoard, possibleMoves, false, depth - 1, alpha, beta);

    //            int score = 10;

    //            alpha = Mathf.Max(alpha, score);

    //            if (score > maxScore) {
    //                maxScore = score;
    //                bestMove = moveValue;
    //            }

    //            // Check if we can prune
    //            if (beta <= alpha)
    //                break;
    //        }

    //        //allyTeam = enemyTeam;
    //        //oppentTeam = playerTeam;
    //    }
    //    else {
    //        // Minimize for player's turn
    //        int minScore = int.MaxValue;

    //        foreach (MoveValue moveValue in moveValues) {
    //            int[,] newBoard = ApplyMove(boardValues, moveValue);
    //            possibleMoves = GetPossibleMoves(newBoard);

    //            MoveValue playerMove = NewCalculateBestMove(newBoard, possibleMoves, true, depth - 1, alpha, beta);

    //            int score = 10;

    //            beta = Mathf.Min(beta, score);

    //            if (score < minScore) {
    //                minScore = score;
    //                bestMove = moveValue;
    //            }

    //            // Check if we can prune
    //            if (beta <= alpha)
    //                break;
    //        }

    //        //allyTeam = playerTeam;
    //        //oppentTeam = enemyTeam;
    //    }

    //    //foreach (UnitController ally in allyTeam) {
    //    //    if (ally.IsOnFullMana()) {
    //    //        tempPoint += 10;
    //    //    }
    //    //    else {
    //    //        teamUsedColors.AddRange(ally.GetColors().Select(color => color.colorCode));
    //    //    }
    //    //}

    //    //if (depth == 0 || bestMove.Equals(default(MoveValue))) {
    //    //    return bestMove;
    //    //}

    //    //// Extra turn
    //    //if (moveValues.Exists(em => em.isExtraTurn)) {
    //    //    if (moveValues.Exists(em => em.isExtraTurn && em.color == 1)) {
    //    //        tempPoint += 50;
    //    //        bestMove = moveValues.Find(em => em.isExtraTurn && em.color == 1); // Extra turn + skull
    //    //    }
    //    //    else {
    //    //        tempPoint += 30;
    //    //    }
    //    //}

    //    return bestMove;
    //}

    //private int[,] ApplyMove(int[,] boardValues, MoveValue move) {
    //    List<Point> connected = new List<Point>();
    //    int[,] tempBoardValues = boardValues.Clone() as int[,];

    //    //Switch values
    //    int tempValue = tempBoardValues[move.from.x, move.from.y];
    //    int tempValue2 = tempBoardValues[move.to.x, move.to.y];

    //    tempBoardValues[move.from.x, move.from.y] = tempValue2;
    //    tempBoardValues[move.to.x, move.to.y] = tempValue;

    //    int val = tempValue;

    //    connected = FindMatches(tempBoardValues);

    //    return tempBoardValues;
    //}

    //private List<Point> FindMatches(int[,] tempBoardValues) {
    //    List<Point> connected = new List<Point>();

    //    for (int x = 0; x < tempBoardValues.GetLength(0); x++) {
    //        for (int y = 0; y < tempBoardValues.GetLength(1); y++) {
    //            Point point = new Point(x, y);
    //            int val = tempBoardValues[x, y];

    //            Point[] directions = {
    //                Point.Up,
    //                Point.Right,
    //                Point.Down,
    //                Point.Left
    //            };
    //            foreach (Point dir in directions) { //Checking if there is 2 same in the directions -> X X Y OR Y X X
    //                List<Point> line = new List<Point>();

    //                int same = 0;
    //                for (int i = 1; i < 3; i++) {
    //                    Point check = Point.Add(point, Point.Mul(dir, i));
    //                    if (GetValueAtPoint(tempBoardValues, check) == val) {
    //                        line.Add(check);
    //                        same++;
    //                    }
    //                }

    //                if (same > 1) {
    //                    AddPoints(ref connected, line);
    //                }
    //            }

    //            for (int i = 0; i < 2; i++) //Checking if we are in the middle  X Y X
    //            {
    //                List<Point> line = new List<Point>();

    //                int same = 0;
    //                Point[] check = {
    //                    Point.Add(point, directions[i]),
    //                    Point.Add(point, directions[i+2])
    //                };

    //                foreach (Point next in check) // Check both sides of the piece
    //                {
    //                    if (GetValueAtPoint(tempBoardValues, next) == val) {
    //                        line.Add(next);
    //                        same++;
    //                    }
    //                }

    //                if (same > 1) {
    //                    AddPoints(ref connected, line);
    //                }

    //                if (same == 2) { // When we are middle, check if can be an extra turn
    //                    Point[] check2 = {
    //                        Point.Add(point, Point.Mul(directions[i], 2)),
    //                        Point.Add(point, Point.Mul(directions[i+2], 2))
    //                    };
    //                    foreach (Point next in check2) { // Check both sides of the piece
    //                        if (GetValueAtPoint(tempBoardValues, next) == val) {
    //                            same++;
    //                        }
    //                    }
    //                }

    //                if (same > 2) {
    //                    //Debug.Log("extra turn");
    //                    //ExtraTurnHandler.SetExtraTurn();
    //                }
    //            }

    //            for (int i = 0; i < 3; i += 2) //Checking if we are in the edge
    //            {
    //                int same = 0;
    //                int same2 = 0;
    //                Point[] checkEdge = {
    //                        Point.Add(point, directions[i]),
    //                        Point.Add(point, Point.Mul(directions[i], 2)),
    //                        Point.Add(point, directions[1]),
    //                        Point.Add(point, Point.Mul(directions[1], 2))
    //                };

    //                Point[] checkEdge2 = {
    //                        Point.Add(point, directions[i]),
    //                        Point.Add(point, Point.Mul(directions[i], 2)),
    //                        Point.Add(point, directions[3]),
    //                        Point.Add(point, Point.Mul(directions[3], 2))
    //                };

    //                foreach (Point next in checkEdge) {
    //                    if (GetValueAtPoint(tempBoardValues, next) == val)
    //                        same++;
    //                }

    //                foreach (Point next in checkEdge2) {
    //                    if (GetValueAtPoint(tempBoardValues, next) == val)
    //                        same2++;
    //                }

    //                if (same == 4 || same2 == 4) {
    //                    //ExtraTurnHandler.SetExtraTurn();
    //                }
    //            }

    //            for (int i = 0; i < 2; i++) { //Checking if we are in T shape
    //                int same = 0;
    //                int same2 = 0;

    //                //Only 4 directions are in the array
    //                int tempDir = 3;
    //                if (i + 3 > 3) {
    //                    tempDir = 0;
    //                }

    //                Point[] checkTShape = {
    //                        Point.Add(point, directions[i]),
    //                        Point.Add(point, Point.Mul(directions[i], 2)),
    //                        Point.Add(point, directions[i+1]),
    //                        Point.Add(point, directions[tempDir]),
    //                    };

    //                Point[] checkTShape2 = {
    //                        Point.Add(point, directions[i+2]),
    //                        Point.Add(point, Point.Mul(directions[i+2], 2)),
    //                        Point.Add(point, directions[i+1]),
    //                        Point.Add(point, directions[tempDir]),
    //                    };

    //                foreach (Point next in checkTShape) {
    //                    if (GetValueAtPoint(tempBoardValues, next) == val)
    //                        same++;
    //                }

    //                foreach (Point next in checkTShape2) {
    //                    if (GetValueAtPoint(tempBoardValues, next) == val)
    //                        same2++;
    //                }

    //                if (same == 4 || same2 == 4) {
    //                    //ExtraTurnHandler.SetExtraTurn();
    //                }
    //            }
    //        }

    //        //for (int i = 0; i < connected.Count; i++) {
    //        //    MoveValue tempMoveValue = new MoveValue(connected[i]);
    //        //    AddPoints(ref connected, ApplyMove(tempBoardValues, connected[i], false));
    //        //}
    //    }
    //    return connected;
    //}

    //public void AddPoints(ref List<Point> points, List<Point> add) {
    //    foreach (Point p in add) {
    //        bool doAdd = true;
    //        for (int i = 0; i < points.Count; i++) {
    //            if (points[i].Equals(p)) {
    //                doAdd = false;
    //                break;
    //            }
    //        }
    //        if (doAdd) points.Add(p);
    //    }
    //}
}