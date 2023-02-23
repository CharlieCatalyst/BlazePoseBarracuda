using System;

[System.Serializable]
public class Point : IEquatable<Point>
{

    public double x, y;

    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public Point()
        : this(0, 0)
    {

    }

    public Point(double[] vals)
        : this()
    {
        set(vals);
    }

    public void set(double[] vals)
    {
        if (vals != null)
        {
            x = vals.Length > 0 ? vals[0] : 0;
            y = vals.Length > 1 ? vals[1] : 0;
        }
        else
        {
            x = 0;
            y = 0;
        }
    }

    public Point clone()
    {
        return new Point(x, y);
    }

    public double dot(Point p)
    {
        return x * p.x + y * p.y;
    }


    public override string ToString()
    {
        return "{" + x + ", " + y + "}";
    }

    //

    #region Operators

    // (here p stand for a point ( Point ), alpha for a real-valued scalar ( double ).)

    #region Unary

    // -p
    public static Point operator -(Point a)
    {
        return new Point(-a.x, -a.y);
    }

    #endregion

    #region Binary

    // p + p
    public static Point operator +(Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }

    // p - p
    public static Point operator -(Point a, Point b)
    {
        return new Point(a.x - b.x, a.y - b.y);
    }

    // p * alpha, alpha * p
    public static Point operator *(Point a, double b)
    {
        return new Point(a.x * b, a.y * b);
    }

    public static Point operator *(double a, Point b)
    {
        return new Point(b.x * a, b.y * a);
    }

    // p / alpha
    public static Point operator /(Point a, double b)
    {
        return new Point(a.x / b, a.y / b);
    }

    #endregion

    #region Comparison

    public bool Equals(Point a)
    {
        // If both are same instance, return true.
        if (System.Object.ReferenceEquals(this, a))
        {
            return true;
        }

        // If object is null, return false.
        if ((object)a == null)
        {
            return false;
        }

        // Return true if the fields match:
        return this.x == a.x && this.y == a.y;
    }

    // p == p
    public static bool operator ==(Point a, Point b)
    {
        // If both are null, or both are same instance, return true.
        if (System.Object.ReferenceEquals(a, b))
        {
            return true;
        }

        // If one is null, but not both, return false.
        if (((object)a == null) || ((object)b == null))
        {
            return false;
        }

        // Return true if the fields match:
        return a.x == b.x && a.y == b.y;
    }

    // p != p
    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }

    #endregion

    #endregion

    //
}
