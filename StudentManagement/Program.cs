// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using System.Text.Json.Serialization;
using StudentManagement;

Console.WriteLine("Hello, World!");

// Student student = new Student(
//         "Ananas",
//         DepartmentType.Architecture,
//         4,
//         21,
//         3.5
//     );
//
// StudentList<Student> list = new StudentList<Student>();
// list.LoadFromJson("something.json");
// string json = JsonSerializer.Serialize(list);
// Console.WriteLine(json);

// Student? stu = JsonSerializer.Deserialize<Student>(json);
    // Console.WriteLine(stu);
    Cli cli = new Cli();
    cli.Start();