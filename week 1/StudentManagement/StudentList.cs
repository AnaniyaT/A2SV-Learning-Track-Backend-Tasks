using System.Text.Json;
using System.Text.Json.Serialization;
namespace StudentManagement;

public class StudentList<T>
{
    private List<T> Students = new List<T>();
    [JsonPropertyName("Students")]
    public List<T> StudentsPublic { get { return Students; } }
    
    public List<T> GetStudents()
    {
        return Students;
    }

    public void Add(T student)
    {
        Students.Add(student);
    }

    public void Remove(T student)
    {
        Students.Remove(student);
    }

    public void PutAt(int index, T student)
    {
        if (index < Students.Count)
            Students[index] = student;
    }

    public void RemoveAt(int index)
    {
        if (index < Students.Count)
            Students.RemoveAt(index);
    }

    public void PrintAll()
    {
        foreach (var student in Students)
        {
            Console.WriteLine(student);
            Console.WriteLine("------------------------------------------");
        }
        ;
    }

    public void WriteToJson(string path)
    {
        try
        {
            var json = JsonSerializer.Serialize(Students);
            File.WriteAllText(path, json);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public void LoadFromJson(string path)
    {
        try
        {
            string? json = File.ReadAllText(path);
            List<T>? loadedStudents = JsonSerializer.Deserialize<List<T>>(json);
            Students = loadedStudents ?? new List<T>();
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Json file not found");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}