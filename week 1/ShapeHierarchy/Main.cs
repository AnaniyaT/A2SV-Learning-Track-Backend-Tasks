using System;

void PrintShapeArea(Shape shape)
{
    Console.WriteLine($"The name of the Shape is {shape.name}.");
    Console.WriteLine($"It's area is {shape.CalculateArea()}.\n");
}

Shape[] shapes =  {
    new Rectangle(20, 30),
    new Triangle(10, 47),
    new Circle(20)
};

foreach (Shape shape in shapes)
{
    PrintShapeArea(shape);
}
