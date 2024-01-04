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

    private int[,] boardValues;
    private int[,] tempBoardValues;

    // Update is called once per frame
    private void Update() {
        if (BattleStateHandler.GetState() == BattleState.WaitingForEnemy) {
            BattleStateHandler.SetState(BattleState.EnemyTurn);
            CheckSpellCast();
            CalculateMove();

            if (!bestMove.Equals(default(MoveValue))) {
                GameObject fromGO = GameObject.Find("Node [" + bestMove.from.x + ", " + bestMove.from.y + "]");
                GameObject toGO = GameObject.Find("Node [" + bestMove.to.x + ", " + bestMove.to.y + "]");
                NodePiece fromNode = fromGO.GetComponent<NodePiece>();
                NodePiece toNode = toGO.GetComponent<NodePiece>();
                MovePieces.instance.EnemyMove(fromNode, toNode);
                //MovePieces.instance.MovePiece(fromGO.GetComponent<NodePiece>());
                bestMove = default;
            }

            //BattleStateHandler.setState(BattleState.WaitingForPlayer);
        }
    }

    private void CalculateMove() {
        if (BattleStateHandler.GetState() == BattleState.EnemyTurn) {
            Node[,] board = FindObjectOfType<Match3>().GetBoard();
            boardValues = new int[board.GetLength(0), board.GetLength(1)];
            tempBoardValues = new int[board.GetLength(0), board.GetLength(1)];
            List<MoveValue> possibleMoves = new List<MoveValue>();

            for (int i = 0; i < board.GetLength(0); i++) {
                for (int j = 0; j < board.GetLength(1); j++) {
                    //Node node = (Node)board.GetValue(i, j);
                    Point point = new Point(i, j);
                    boardValues[i, j] = FindObjectOfType<Match3>().GetValueAtPoint(point);
                }
            }
            possibleMoves = GetPossibleMoves();

            bestMove = CalculateBestMove(possibleMoves);

            if (bestMove.Equals(default(MoveValue))) {
                // No possible match
                Debug.Log("No match");
            }

            //Debug.Log("Best move: from [" + bestMove.from.x.ToString() + "," + bestMove.from.y.ToString() + "] to [" + bestMove.to.x.ToString() + "," + bestMove.to.y.ToString() + "]");
        }
    }

    public MoveValue GetBestMove() {
        return bestMove;
    }

    private MoveValue CalculateBestMove(List<MoveValue> moveValues) {
        MoveValue bestMove;

        List<GameObject> enemyTeam = TurnBase.GetInstance().GetEnemyTeam();
        bestMove = moveValues.Find(em => em.isExtraTurn && em.color == 1); // Extra turn + skull

        if (bestMove.Equals(default(MoveValue))) {
            List<int> enemyUsedColors = new List<int>();
            List<MoveValue> extraTurnMoves = moveValues.FindAll(pv => pv.isExtraTurn);

            foreach (GameObject enemyGo in enemyTeam) {
                UnitController enemy = enemyGo.GetComponent<UnitController>();

                if (!enemy.IsOnFullMana()) {
                    enemyUsedColors.AddRange(enemy.GetColors().Select(color => color.colorCode));
                }
            }

            if (extraTurnMoves.Count > 0) {
                // Extra turn + gain mana
                bestMove = extraTurnMoves.FirstOrDefault(move => enemyUsedColors.Contains(move.color));

                if (bestMove.Equals(default(MoveValue))) {
                    // Extra turn
                    return extraTurnMoves.First();
                }

                return bestMove;
            }

            //Skull
            bestMove = moveValues.FirstOrDefault(em => em.color == 1);

            if (bestMove.Equals(default(MoveValue))) {
                //Gain mana
                bestMove = moveValues.FirstOrDefault(move => enemyUsedColors.Contains(move.color));
            }

            if (bestMove.Equals(default(MoveValue))) {
                // Other combination
                return moveValues.First();
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
        yield return new WaitForSeconds(5);
    }

    private List<MoveValue> GetPossibleMoves() {
        List<MoveValue> moveValues = new List<MoveValue>();

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
                    //tempBoardValues = boardValues;

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
                            if (GetValueAtPoint(check) == val) {
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
                            if (GetValueAtPoint(next) == val) {
                                same++;
                            }

                            if (same == 2) {
                                Point[] check2 = {
                                    Point.Add(newPoint, Point.Mul(directions[i], 2)),
                                    Point.Add(newPoint, Point.Mul(directions[i+2], 2))
                                };
                                foreach (Point next2 in check2) { // Check both sides of the piece
                                    if (GetValueAtPoint(next2) == val) {
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
                            if (GetValueAtPoint(next) == val)
                                same++;
                        }

                        foreach (Point next in checkEdge2) {
                            if (GetValueAtPoint(next) == val)
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
                            if (GetValueAtPoint(next) == val)
                                same++;
                        }

                        foreach (Point next in checkTShape2) {
                            if (GetValueAtPoint(next) == val)
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

    private int GetValueAtPoint(Point point) {
        int width = FindObjectOfType<Match3>().GetBoardWidth();
        int height = FindObjectOfType<Match3>().GetBoardHeight();

        if (point.x < 0 || point.x >= width || point.y < 0 || point.y >= height) return -1;
        return tempBoardValues[point.x, point.y];
    }
}