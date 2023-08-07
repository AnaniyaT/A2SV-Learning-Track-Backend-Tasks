using System.Text.Json.Serialization;

namespace StudentManagement;

public enum DepartmentType
{
    Engineering,
    Medicine,
    Architecture,
    Law
}

public class Student
{
    public String Name { get; set; }
    public DepartmentType Department { get; set; }
    public readonly int RollNumber;
    [JsonPropertyName("RollNumber")]
    public int rollNumber  { get { return RollNumber; } }
    public int Age { get; set; }
    public double Grade { get; set; }

    public Student(string name, DepartmentType department, int rollNumber, int age, double grade)
    {
        Name = name;
        Department = department;
        RollNumber = rollNumber;
        Age = age;
        Grade = grade;
    }

    public void Update(string? name=null, DepartmentType? dep=null, int? age=null, double? grade=null)
    {
        Name = name ?? Name;
        Department = dep ?? Department;
        Age = age ?? Age;
        Grade = grade ?? Grade;
    }

    public override string ToString()
    {
        return $"""
            Name       : {Name}
            Department : {Department}
            RollNumber : {RollNumber}
            Age        : {Age}
            Grade      : {Grade}
        """;
    }
}