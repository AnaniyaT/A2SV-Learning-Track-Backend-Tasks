using System;
using System.IO;
using TaskManager.Util;

namespace TaskManager;

public class TaskManagerApp
{
    public List<TaskItem> tasks { get; } = new List<TaskItem>();
    async Task AddTask(String name, String? description, TaskCategory category, bool completed = false, bool addToLocal = false)
    {
        TaskItem task = new TaskItem() { 
            Name = name,
            Description = description, 
            Category = category, 
            IsCompleted = completed
        };
        
        tasks.Add(task);
        if (addToLocal)
            await WriteTaskToLocal(task);
    }

    public async Task DeleteTask(int pos)
    {
        if (pos < tasks.Count)
        {
            tasks.RemoveAt(pos);
            await UpdateLocal();
        }
    }

    public async Task CompleteTask(int pos)
    {
        if (pos < tasks.Count)
        {
            tasks[pos].IsCompleted = true;
            await UpdateLocal();
        }
    }

    TaskCategory TaskCategoryFromString(String word)
    {
        word = word.ToLower();
        TaskCategory category;
        switch (word)
        {
            case ("school"):
                category = TaskCategory.School;
                break;
            case ("work"):
                category = TaskCategory.Work;
                break;
            case("personal"):
                category = TaskCategory.Personal;
                break;
            default:
                category = TaskCategory.Personal;
                break;
        }

        return category;
    }
    
    public async Task AddTaskFromInput()
    {
        String name = InputHandler.GetValidatedInput(
            "Enter the name of the Task",
            name => name.Length > 2 ? null : "Name should be longer than 2 characters"
        );

        String description = InputHandler.GetValidatedInput(
            "Enter the description",
            input => null
        );

        String typeInWords = InputHandler.GetValidatedInput(
            "Enter the type of the task. 'Work' for work, 'Personal' for Personal, and 'school' for School",
            (input) => new List<String>() {"personal", "school", "work"}.Contains(input.ToLower()) ? null :
                "Your input must be one of the following: 'Personal', 'School' and 'Work'"
        );
        TaskCategory category = TaskCategoryFromString(typeInWords);

        await AddTask(name, description, category, false, true);
        Console.WriteLine("Your Task was added Successfully!");
    }

    public void DisplayTasks(List<TaskItem>? givenTasks = null )
    {
        Console.WriteLine("############## YOUR TASKS ###############");
        List<TaskItem> tasksToDisplay = givenTasks ?? tasks;
        for (int indx = 0; indx < tasksToDisplay.Count; indx++)
        {
            Console.WriteLine($"No. {indx + 1}");
            Console.WriteLine(tasksToDisplay[indx]);
            Console.WriteLine("---------------------------------------------");
        }
    }

    public List<TaskItem> FilterByCategory(TaskCategory category)
    {
        List<TaskItem> filtered = tasks.Where(task => task.Category == category).ToList();
        return filtered;
    }

    public List<TaskItem> FilterByCompleted(bool isCompleted)
    {
        List<TaskItem> filtered = tasks.Where(task => task.IsCompleted == isCompleted).ToList();
        return filtered;
    }
    public async Task WriteTaskToLocal(TaskItem task)
    {
        try
        {
            await using (FileStream file = new FileStream("tasks.csv", FileMode.Append))
            {
                await using (StreamWriter writer = new StreamWriter(file))
                {
                    String line = $"{task.Name},{task.Description},{task.Category},{task.IsCompleted}\n";
                    await writer.WriteAsync(line);
                }
            }
        }
        catch (FileNotFoundException)
        {
            await using FileStream file = new FileStream("tasks.csv", FileMode.Create);
            await WriteTaskToLocal(task);
        }
    }
    
    async Task UpdateLocal()
    {
        await using (FileStream file = new FileStream("tasks.csv", FileMode.Create))
        {
            StreamWriter writer = new StreamWriter(file);
            foreach (var task in tasks)
            {
                String line = $"{task.Name},{task.Description},{task.Category},{task.IsCompleted}";
                await writer.WriteLineAsync(line);
            }
            writer.Close();
        }
    }

    public async Task StartApplication()
    {
        try
        {
            using StreamReader reader = new("tasks.csv");
            String line;
            while ((line = await reader.ReadLineAsync()) != null && line.Length > 1)
            {
                List<String> split = line.Split(",").ToList();
                await AddTask(
                    split[0],
                    split[1],
                    TaskCategoryFromString(split[2]),
                    split[3] == "true"
                );
            }
            
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("CSV file not found, creating...");
            await using FileStream file = new ("tasks.csv", FileMode.Create);
            Console.WriteLine("CSV file created.\n");
        }
    }
    
}