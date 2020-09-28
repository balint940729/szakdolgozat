using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match3 : MonoBehaviour
{

    public ArrayLayout boardLayout;
    int width = 9;
    int height = 14;
    Node[,] board;

    System.Random random;
    void Start()
    {

    }

    void StartGame()
    {
        board = new Node[width, height];
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());

        InitializeBoard();
    }

    void InitializeBoard()
    {
        board = new Node[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x > width; x++)
            {
                board[x, y] = new Node(-1, new Point(x, y));
            }
        }
    }
    void Update()
    {

    }

    string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";

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