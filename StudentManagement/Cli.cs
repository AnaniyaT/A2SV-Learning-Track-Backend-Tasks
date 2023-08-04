namespace StudentManagement;

public class Cli
{
    private StudentList<Student> _students = new StudentList<Student>();
    private int _nextRollNo = 1;

    List<Student> Search(string query)
    {
        var filtered =
            from student in _students.GetStudents()
            where student.rollNumber.ToString() == query ||
                  student.Name.ToLower().Contains(query.ToLower())
            select student;
        return filtered.ToList();
    }
    
    public int FindIndex(int rollNo)
    {
        List<Student> students = _students.GetStudents();
        for (int index = 0; index < students.Count; index++)
        {
            if (students[index].rollNumber == rollNo)
                return index;
        }

        return -1;
    }
    
     private void AddStudentFromInput()
    {
        String name = InputHandler.GetValidatedInput(
            "Enter the name of the Student",
            name => name.Length > 2 ? null : "Name should be longer than 2 characters"
        );

        DepartmentType department = DepartmentType.Architecture;
        
        String dep = InputHandler.GetValidatedInput(
            "Enter the department. Engineering, Architecture, Medicine or Law",
            input => Enum.TryParse(input, out department) ? null : "Department only be Engineering, Architecture, Medicine or Law"
        );

        int age = 0;
        String ageInput = InputHandler.GetValidatedInput(
            "Enter the student's age",
            input => int.TryParse(input, out age) ? null : "Enter a valid age homie"
        );
        
        double grade = 0;
        String gradeInput = InputHandler.GetValidatedInput(
            "Enter the student's grade",
            input => double.TryParse(input, out grade) && grade <= 4 ? null : "Enter a valid grade homie (<= 4.0)"
        );

        Student student = new(name, department, _nextRollNo, age, grade);
        _nextRollNo++;
        _students.Add(student);
        _students.WriteToJson("students.json");
        Console.WriteLine("Student Added Successfully\n");
    }

     public void Update(int RollNo)
     {
         Student[] student = _students.GetStudents().Where(stu => stu.rollNumber == RollNo).ToArray();
         if (student.Length < 1)
         {
             Console.WriteLine($"Student with Roll No {RollNo} not found");
             return;
         }
         
         Console.WriteLine($"Update {student[0].Name}. Roll Number {student[0].rollNumber}");
         Console.WriteLine("Enter attributes when prompted. Empty entry will leave attribute untouched");
         String name = InputHandler.GetValidatedInput(
             "\nEnter name",
             name => name.Length > 2 || name.Length == 0 ? null : "Name should be longer than 2 characters"
         );

         if (name.Length > 0)
         {
             student[0].Update(name: name);
             Console.WriteLine("Name Updated.");
         }

         DepartmentType department = DepartmentType.Architecture;
        
         String dep = InputHandler.GetValidatedInput(
             "Enter the department. Engineering, Architecture, Medicine or Law",
             input => Enum.TryParse(input, out department) || input.Length == 0 ? null : "Department only be Engineering, Architecture, Medicine or Law"
         );

         if (dep.Length > 0)
         {
             student[0].Update(dep: department);
             Console.WriteLine("Department Updated");
         }
         
         int age = 0;
         String ageInput = InputHandler.GetValidatedInput(
             "Enter age",
             input => int.TryParse(input, out age) || input.Length == 0 ? null : "Enter a valid age homie"
         );
        
         if (ageInput.Length > 0)
         {
             student[0].Update(age: age);
             Console.WriteLine("Age Updated");
         }

         double grade = 0;
         String gradeInput = InputHandler.GetValidatedInput(
             "Enter grade",
             input => (double.TryParse(input, out grade) && grade <= 4) || input.Length == 0 ? null : "Enter a valid grade homie (<=4.0)"
         );
         
         if (gradeInput.Length > 0)
         {
             student[0].Update(grade: grade);
             Console.WriteLine("Grade Updated");
         }
         
         _students.WriteToJson("students.json");
     }

    private void SetNextRollNo()
    {
        foreach (var student in _students.GetStudents())
        {
            if (student.rollNumber >= _nextRollNo)
                _nextRollNo = student.rollNumber + 1;
        }
    }
    
    void PrintCommandList()
    {
        Console.WriteLine(""" 
        List of Commands:
            ls     :  List All Students.
            add    :  Add a student.
            update :  Update student info. Type update followed by Roll number. eg. 'update 43'.
            search :  Search students based on name or Roll No. eg. 'search 35', 'search John'.
            remove :  Remove a student. Type remove followed by Roll number. eg. 'remove 12'.
            help   :  Help.
            quit   : quit.
        """);
    }
    
    public void Start()
    {
        Console.WriteLine("Alright, Don't expect me to welcome you. Just use the app\n");
        Console.WriteLine("Getting students from local...");
        _students.LoadFromJson("students.json");
        SetNextRollNo();
        PrintCommandList();

        bool running = true;
        while (running)
        {
            string command = InputHandler.GetValidatedInput(
                "Enter a Command (ls, add, remove, search, update, help, quit)",
                input => input.Length > 0 ? null : "Empty command"
            );

            string[] split = command.Split().ToArray();
            switch (split[0].ToLower())
            {
                case("ls"):
                    _students.PrintAll();
                    break;
                case("add"):
                    AddStudentFromInput();
                    break;
                case("update"):
                    if (split.Length < 2)
                    {
                        Console.WriteLine("Student Roll Number needed");
                        break;
                    }
                    int rollNo = 0;
                    if (int.TryParse(split[1], out rollNo))
                    {
                        Update(rollNo);
                    }
                    break;
                
                case("search"):
                    if (split.Length < 2)
                    {
                        Console.WriteLine("Search argument needed");
                        break;
                    }
                    Console.WriteLine("Search Results\n");
                    foreach (var student in Search(split[1]))
                    {
                        Console.WriteLine(student);
                        Console.WriteLine("--------------------------------------------");
                    }
                    break;
                
                case("remove"):
                    if (split.Length < 2)
                    {
                        Console.WriteLine("Student Roll Number needed");
                        break;
                    }

                    int roll = -1;
                    if (int.TryParse(split[1], out roll))
                    {
                        int indx = FindIndex(roll);
                        if (roll > -1)
                        {
                            _students.RemoveAt(indx);
                            _students.WriteToJson("students.json");
                            Console.WriteLine("Student Removed Successfully");
                            break;
                        }
                    }
                    
                    Console.WriteLine("Provide a valid Roll Number");
                    break;
                
                case("help"):
                    PrintCommandList();
                    break;
                
                case("quit"):
                    running = false;
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }
    }
}