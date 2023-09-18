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

    public void mul(int szorzo) {
        x *= szorzo;
        y *= szorzo;
    }

    public void add(Point point) {
        x += point.x;
        y += point.y;
    }

    public static Point fromVector(Vector2 v) {
        return new Point((int)v.x, (int)v.y);
    }

    public static Point fromVector(Vector3 v) {
        return new Point((int)v.x, (int)v.y);
    }

    public static Point mul(Point point, int szorzo) {
        return new Point(point.x * szorzo, point.y * szorzo);
    }

    public static Point add(Point point, Point point2) {
        return new Point(point.x + point2.x, point.y + point2.y);
    }

    public static Point clone(Point point) {
        return new Point(point.x, point.y);
    }

    public static Point zero {
        get { return new Point(0, 0); }
    }

    public static Point one {
        get { return new Point(1, 1); }
    }

    public static Point up {
        get { return new Point(0, 1); }
    }

    public static Point down {
        get { return new Point(0, -1); }
    }

    public static Point left {
        get { return new Point(-1, 0); }
    }

    public static Point right {
        get { return new Point(1, 0); }
    }
}