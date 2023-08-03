using System;
using TaskManager;
using TaskManager.Cli;

// TaskItem task = new TaskItem() {Name = "Task1", Category = TaskCategory.Personal};
//
// TaskManagerApp taskManager = new TaskManagerApp();

Cli cli = new Cli();
await cli.Start();
