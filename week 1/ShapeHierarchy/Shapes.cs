public abstract class Shape
{
    public String name;
    public abstract double CalculateArea();
}

public class Circle : Shape
{
    private double radius;
    double PI = 3.1415;

    public Circle(double r)
    {
        name = "Circle";
        radius = r;
    }

    public override double CalculateArea()
    {
        return PI * radius * radius;
    }
}

public class Rectangle : Shape
{
    private double width;
    private double height;

    public Rectangle(double w, double h)
    {
        name = "Rectangle";
        width = w;
        height = h;
    }

    public override double CalculateArea()
    {
        return width * height;
    }
}

public class Triangle : Shape
{
    private double baseLength;
    private double height;

    public Triangle(double b, double h)
    {
        name = "Triangle";
        baseLength = b;
        height = h;
    }

    public override double CalculateArea()
    {
        return 0.5 * baseLength * height;
    }
}

