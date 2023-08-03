using System;
using System.Runtime.InteropServices.ComTypes;
using TaskManager.Util;
namespace TaskManager.Cli;

public class Cli
{
    private TaskManagerApp taskManager = new TaskManagerApp();

    void PrintCommandList()
    {
        Console.WriteLine(""" 
        List of Commands:
            ls     :  List All Tasks.
            add    :  Add a Task.
            filter :  Filter Tasks based on a category. eg. 'filter Work', 'filter Completed'.
            del    :  Delete a task. Type del followed by task number. eg. 'del 1', 'del 2'.
            comp   :  Complete a task. Type comp followed by task number. eg. 'comp 1'.
            h      :  Help.
            q      : quit.
        """);
    }

    async Task CommandHandler(String[] command)
    {
        switch (command[0].ToLower())
        {
            case ("ls"):
                taskManager.DisplayTasks();
                break;
            case("add"):
                await taskManager.AddTaskFromInput();
                break;
            case("h"):
                PrintCommandList();
                break;
            case("del"):
                if (command.Length < 2)
                {
                    Console.WriteLine("Delete position needed");
                    break;
                }

                int pos;
                if (!int.TryParse(command[1], out pos) )
                {
                    Console.WriteLine("Invalid position");
                }

                if (pos > taskManager.tasks.Count || pos < 1)
                {
                    Console.WriteLine("Invalid position");
                }

                await taskManager.DeleteTask(pos - 1);
                Console.WriteLine("Task deleted successfully");
                break;
            
            case("comp"):
                if (command.Length < 2)
                {
                    Console.WriteLine("Complete position needed");
                    break;
                }

                int pos1;
                if (!int.TryParse(command[1], out pos1) )
                {
                    Console.WriteLine("Invalid position");
                }

                if (pos1 > taskManager.tasks.Count || pos1 < 1)
                {
                    Console.WriteLine("Invalid position");
                }

                await taskManager.CompleteTask(pos1 - 1);
                Console.WriteLine("Task completed successfully");
                break;

            case("filter"):
                if (command.Length < 2)
                {
                    Console.WriteLine("Filter category needed");
                    break;
                }

                switch (command[1].ToLower())
                {
                    case("completed"):
                        List<TaskItem> filtered = taskManager.FilterByCompleted(true);
                        taskManager.DisplayTasks(filtered);
                        break;
                    case("notcompleted"):
                        List<TaskItem> filtered1 = taskManager.FilterByCompleted(false);
                        taskManager.DisplayTasks(filtered1);
                        break;
                    case("personal"):
                        List<TaskItem> filtered2 = taskManager.FilterByCategory(TaskCategory.Personal);
                        taskManager.DisplayTasks(filtered2);
                        break;
                    case("work"):
                        List<TaskItem> filtered3 = taskManager.FilterByCategory(TaskCategory.Work);
                        taskManager.DisplayTasks(filtered3);
                        break;
                    case("school"):
                        List<TaskItem> filtered4 = taskManager.FilterByCategory(TaskCategory.School);
                        taskManager.DisplayTasks(filtered4);
                        break;
                    default:
                        Console.WriteLine("Invalid filter category");
                        break;
                }
                break;

            default:
                Console.WriteLine("Unknown Command.");
                break;
        }
        
    }

    public async Task Start()
    {
        Console.WriteLine("Welcome To Task Manager App.");
        await taskManager.StartApplication();
        PrintCommandList();

        bool running = true;
        while (running)
        {
            String inputCommand = InputHandler.GetValidatedInput(
                "\nInput Command (ls, add, filter, del, comp, h, q)",
                        input => input.Length >= 1 ? null : "Invalid command"
            );

            String[] splitInput = inputCommand.Split();
            if (splitInput[0].ToLower() == "q")
            {
                running = false;
            }
            else
            {
                await CommandHandler(splitInput);
            }
        }
    }
    
}
