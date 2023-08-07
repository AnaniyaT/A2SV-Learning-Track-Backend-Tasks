using System;

GradeCalculator gradeCalculator = new();
gradeCalculator.DoTheDeed();

class GradeCalculator {
    String? studentName;
    int numberOfSubjects = 0;
    Dictionary<string, float> grades = new();

    private void AcceptName() {
        Console.WriteLine("Enter the name of the student");
        studentName = Console.ReadLine();
        Console.WriteLine($"\nHello {studentName}!");
    }

    private void AcceptNumberOfSubjects() {
        Console.WriteLine("\nEnter the number of subjects");
        numberOfSubjects = Convert.ToInt32(Console.ReadLine());
    }
    private void AddGrade(String subject, float score) {
        grades[subject] = score;
    }
    private void AddGradesFromInputs() {

        Console.WriteLine("\nEnter Grades. Subject followed by Score. Eg. Math 86\n");

        while (grades.Count < numberOfSubjects) {
            Console.WriteLine($"Enter grade nubmer {grades.Count + 1}");
            String? input = Console.ReadLine();

            if (input != null) {
                String[] split = input.Split();

                if (split.Length == 2 && float.TryParse(split[1], out _)) {
                    String subject = split[0];
                    float score = float.Parse(split[1]);

                    if (score >= 0 && score <= 100)
                        AddGrade(subject, score);
                    else
                        Console.WriteLine("Score must be between 0 and 100. Please Try again.\n");
                }
                // if the input isn't in the correct format  
                else {
                    Console.WriteLine("Please Use the correct format and Try again.\n");
                }
            } 
            // if the input is null
            else {
                Console.WriteLine("Please Use the correct format and Try again.\n");
            }
        }
    }

    public float GetAverageGrade() {
        float totalScore = 0;

        foreach(float score in grades.Values) {
            totalScore += score;
        }

        return totalScore / grades.Count;
    }
    public void DoTheDeed() {
        AcceptName();
        AcceptNumberOfSubjects();
        AddGradesFromInputs();

        Console.WriteLine();
        Console.WriteLine($"Your Average Score is {GetAverageGrade()}");
    }
}
