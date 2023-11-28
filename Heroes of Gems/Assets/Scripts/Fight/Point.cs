using UnityEngine;

[System.Serializable]
public class Point {
    public int x;
    public int y;

    public Point(int nx, int ny) {
        x = nx;
        y = ny;
    }

    public Vector2 ToVector() {
        return new Vector2(x, y);
    }

    public bool Equals(Point point) {
        return (x == point.x && y == point.y);
    }

    public void Mul(int szorzo) {
        x *= szorzo;
        y *= szorzo;
    }

    public void Add(Point point) {
        x += point.x;
        y += point.y;
    }

    public static Point FromVector(Vector2 v) {
        return new Point((int)v.x, (int)v.y);
    }

    public static Point FromVector(Vector3 v) {
        return new Point((int)v.x, (int)v.y);
    }

    public static Point Mul(Point point, int szorzo) {
        return new Point(point.x * szorzo, point.y * szorzo);
    }

    public static Point Add(Point point, Point point2) {
        return new Point(point.x + point2.x, point.y + point2.y);
    }

    public static Point Clone(Point point) {
        return new Point(point.x, point.y);
    }

    public static Point Zero {
        get { return new Point(0, 0); }
    }

    public static Point One {
        get { return new Point(1, 1); }
    }

    public static Point Up {
        get { return new Point(0, 1); }
    }

    public static Point Down {
        get { return new Point(0, -1); }
    }

    public static Point Left {
        get { return new Point(-1, 0); }
    }

    public static Point Right {
        get { return new Point(1, 0); }
    }
}