using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour
{
    public ArrayLayout boardLayout;

    [Header("UI Elements")]
    public Sprite[] pieces;
    public RectTransform gameBoard;

    [Header("Prefabs")]
    public GameObject nodePiece;
    int width = 8;
    int height = 8;
    Node[,] board;

    System.Random random;
    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        board = new Node[width, height];
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());

        InitializeBoard();
        VerifyBoard();
        InstantiateBoard();
    }

    void InitializeBoard()
    {
        board = new Node[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                board[x, y] = new Node((boardLayout.rows[y].row[x]) ? -1 : fillPiece(), new Point(x, y));
            }
        }
    }
    void InstantiateBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int val = board[x, y].value;
                if (val <= 0) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(63 + (128 * x), -63 - (128 * y));
            }
        }
    }



    void VerifyBoard()
    {
        List<int> remove;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Point point = new Point(x, y);
                int val = getValueAtPoint(point);
                if (val <= 0) continue;

                remove = new List<int>();
                while (IsConnected(point, true).Count > 0)
                {
                    val = getValueAtPoint(point);
                    if (!remove.Contains(val))
                    {
                        remove.Add(val);
                    }
                    setValueAtPoint(point, newValue(ref remove));
                }
            }
        }
    }

    int newValue(ref List<int> remove) // Kivesszük az értéket és helyette teszünk bele egy másikat, úgy hogy az ne legyen ütközés az ellőzővel
    {
        List<int> available = new List<int>();
        for (int i = 0; i < pieces.Length; i++)
        {
            available.Add(i + 1);
        }
        foreach (int i in remove)
        {
            available.Remove(i);
        }

        if (available.Count <= 0) return 0;

        return available[random.Next(0, available.Count)];
    }

    int setValueAtPoint(Point p, int v)
    {
        return board[p.x, p.y].value = v;
    }

    List<Point> IsConnected(Point point, bool main)
    {
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(point);
        Point[] directions = {
            Point.up,
            Point.right,
            Point.down,
            Point.left
        };

        foreach (Point dir in directions) //Checking id there is 2 more same in the directions
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for (int i = 1; i < 3; i++)
            {
                Point check = Point.add(point, Point.mul(dir, i));
                if (getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }
            if (same > 1)
                AddPoints(ref connected, line);
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
                if (getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }

            if (same > 1)
            {
                AddPoints(ref connected, line);
            }
        }
        if (main) //Megnézi hogy van e több match is az adott pontban
        {
            for (int i = 0; i < connected.Count; i++)
            {
                AddPoints(ref connected, IsConnected(connected[i], false));
            }
        }

        /*if (connected.Count > 0)
            connected.Add(point);*/

        return connected;
    }

    void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach (Point p in add)
        {
            bool doAdd = true;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }
            if (doAdd) points.Add(p);
        }
    }

    int getValueAtPoint(Point point)
    {
        if (point.x < 0 || point.x >= width || point.y < 0 || point.y >= height) return -1;
        return board[point.x, point.y].value;
    }

    int fillPiece()
    {
        int val = 1;
        val = (random.Next(0, 100) / (100 / pieces.Length)) + 1;
        return val;
    }

    void Update()
    {

    }

    string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";

        for (int i = 0; i < 20; i++)
        {
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        }

        return seed;
    }
}

[System.Serializable]
public class Node
{
    public int value; //Az adott mezőn található Gem értéke
    public Point index;

    public Node(int v, Point i)
    {
        value = v;
        index = i;
    }

}