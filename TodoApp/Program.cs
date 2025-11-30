using System;
using Todo.Core;

class Program
{
    static void Main()
    {
        var list = new TodoList();

        Console.WriteLine("Enter the path to a JSON file to load (leave empty if there is no file):");
        string loadPath = Console.ReadLine()?.Trim();
        if (!string.IsNullOrEmpty(loadPath))
        {
            try
            {
                list.Load(loadPath);
                Console.WriteLine($"Loaded {list.Count} tasks from {loadPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while loading: {ex.Message}");
            }
        }

        Console.WriteLine("Enter tasks (empty line to finish):");
        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine()?.Trim() ?? "";
            if (string.IsNullOrEmpty(input))
                break;

            list.Add(input);
        }

        if (list.Count == 0)
        {
            Console.WriteLine("Task list is empty. Exiting.");
            return;
        }

        while (true)
        {
            Console.WriteLine("\nCurrent tasks:");
            for (int i = 0; i < list.Count; i++)
            {
                var item = list.Items[i];
                Console.WriteLine($"{i + 1}. {item.Title} - Done: {item.IsDone}");
            }

            Console.WriteLine("\nCommands: done <number>, undone <number>, add <task>, remove <number>, save, exit");
            Console.Write("> ");
            string command = Console.ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(command))
                continue;

            var parts = command.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string cmd = parts[0].ToLower();

            switch (cmd)
            {
                case "done":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int doneIndex) || doneIndex < 1 || doneIndex > list.Count)
                    {
                        Console.WriteLine("Invalid task number.");
                        break;
                    }
                    list.Items[doneIndex - 1].MarkDone();
                    break;

                case "undone":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int undoneIndex) || undoneIndex < 1 || undoneIndex > list.Count)
                    {
                        Console.WriteLine("Invalid task number.");
                        break;
                    }
                    list.Items[undoneIndex - 1].MarkUndone();
                    break;

                case "add":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Enter the task text after the add command.");
                        break;
                    }
                    list.Add(parts[1]);
                    break;

                case "remove":
                    if (parts.Length < 2 || !int.TryParse(parts[1], out int removeIndex) || removeIndex < 1 || removeIndex > list.Count)
                    {
                        Console.WriteLine("Invalid task number.");
                        break;
                    }
                    list.Remove(list.Items[removeIndex - 1].Id);
                    break;

                case "save":
                    Console.Write("Enter the path to save the JSON file: ");
                    string savePath = Console.ReadLine()?.Trim() ?? "tasks.json";
                    try
                    {
                        list.Save(savePath);
                        Console.WriteLine($"Task list successfully saved to {savePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error while saving: {ex.Message}");
                    }
                    break;

                case "exit":
                    return;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }
}
