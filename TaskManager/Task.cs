using System;

namespace TaskManager;
public enum TaskCategory
{
    Personal,
    Work,
    School
}

public class TaskItem
{
    public String Name { get; set; }
    public String? Description { get; set; }
    public TaskCategory Category { get; set; }
    public bool IsCompleted { get; set; }

    public override string ToString()
    {
        return $""""
            Name: {Name}
            Description: {(Description?.Length >= 1 ? Description : "No description provided")}
            Category: {Category}
            Status: {(IsCompleted ? "Completed": "Not Completed")}
        """";
    }
}