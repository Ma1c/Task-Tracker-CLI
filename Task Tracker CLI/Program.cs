using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Program
{
    private static readonly string FilePath = "Tasks.cs";

    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        EnsureFileExists();

        while (true)
        {
            ShowMenu();
            string choise = Console.ReadLine();

            switch (choise)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    ViewAllTasks();
                    break;
                case "3":
                    DeleteTask();
                    break;
                case "4":
                    MarkTaskDone();
                    break;
                case "5":
                    MarkTaskInProgress();
                    break;
                case "6":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Incorrect input. Try again.");
                    break;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }

    private static void ShowMenu()
    {
        Console.Clear();
        Console.WriteLine("===== Task Tracker =====");
        Console.WriteLine("1. Add new task");
        Console.WriteLine("2. View all tasks");
        Console.WriteLine("3. Delete task");
        Console.WriteLine("4. Mark task as done");
        Console.WriteLine("5. Mark task as in progress");
        Console.WriteLine("6. Exit");
        Console.Write("\nSelect an option: ");
    }

    private static void EnsureFileExists()
    {
        if (!File.Exists(FilePath))
        {
            File.Create(FilePath).Close();
        }
    }

    private static List<Tasks> ReadTasks()
    {
        var task = new List<Tasks>();
        var lines = File.ReadAllLines(FilePath, Encoding.UTF8);

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length == 5)
            {
                task.Add(new Tasks
                {
                    id = int.Parse(parts[0]),
                    description = parts[1],
                    status = parts[2],
                    createdAt = parts[3],
                    updatedAt = parts[4]
                });
            }
        }
        return task;
    }

    private static void WriteTask(List<Tasks> tasks)
    {
        var lines = tasks.Select(c => $"{c.id},{c.description},{c.status},{c.createdAt},{c.updatedAt}");
        File.WriteAllLines(FilePath, lines, Encoding.UTF8);
    }

    private static void ViewAllTasks()
    {
        Console.Clear();
        Console.WriteLine("===== List of all tasks =====");
        var tasks = ReadTasks();

        if (tasks.Count == 0)
        {
            Console.WriteLine("There're no tasks.");
            return;
        }

        foreach (var task in tasks)
            Console.WriteLine(task);
    }

    private static void AddTask()
    {
        var tasks = ReadTasks();

        Console.Write("Write task description: ");
        var description = Console.ReadLine();

        int newId = (tasks.Count == 0) ? 1 : tasks.Max(t => t.id) + 1;
        var now = DateTime.Now.ToString();

        var newTask = new Tasks
        {
            id = newId,
            description = description,
            status = "todo",
            createdAt = now,
            updatedAt = now
        };

        tasks.Add(newTask);
        WriteTask(tasks);

        Console.WriteLine("Task added!");
    }

    private static void DeleteTask()
    {
        var tasks = ReadTasks();
        Console.Clear();

        foreach (var task in tasks)
            Console.WriteLine(task);

        Console.WriteLine("\n\nDeleting task");
        Console.Write("\nWrite Task Id: ");
        string input = Console.ReadLine();

        if (!int.TryParse(input, out int searchId))
        {
            Console.WriteLine("Invalid Id format!");
            return;
        }

        var tasksToDel = tasks.FirstOrDefault(c => c.id == searchId);


        if (tasksToDel == null)
        {
            Console.WriteLine("No tasks to delete");
            return;
        }

        tasks.Remove(tasksToDel);
        WriteTask(tasks);

        Console.WriteLine($"Task with Id={searchId} deleted.");
    }
    private static void MarkTaskDone()
    {
        Console.Clear();
        foreach (var task in ReadTasks())
            Console.WriteLine(task);

        Console.WriteLine("\n\nMarking task as done");
        Console.Write("\nWrite Task Id: ");
        string input = Console.ReadLine();
        if (!int.TryParse(input, out int searchId))
        {
            Console.WriteLine("Invalid Id format!");
            return;
        }
        UpdateTaskStatus(searchId, "done");
    }

    private static void MarkTaskInProgress()
    {
        Console.Clear();
        foreach (var task in ReadTasks())
            Console.WriteLine(task);

        Console.WriteLine("\n\nMarking task as in progress");
        Console.Write("\nWrite Task Id: ");
        string input = Console.ReadLine();
        if (!int.TryParse(input, out int searchId))
        {
            Console.WriteLine("Invalid Id format!");
            return;
        }
        UpdateTaskStatus(searchId, "in progress");
    }


    private static void UpdateTaskStatus(int searchId, string newStatus)
    {
        var tasks = ReadTasks();
        var now = DateTime.Now.ToString();
        var task = tasks.FirstOrDefault(t => t.id == searchId);

        if (task == null)
        {
            Console.WriteLine($"No task with Id={searchId} found.");
            return;
        }
        task.status = newStatus;
        task.updatedAt = now;
        WriteTask(tasks);
        Console.WriteLine($"Task with Id={searchId} marked as {newStatus}.");
    }

}
