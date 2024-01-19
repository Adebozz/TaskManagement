using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

class Program
{
    static List<TaskItem> tasks = new List<TaskItem>();
    static string connectionString = "your_sql_server_connection_string";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Task Manager");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View Tasks");
            Console.WriteLine("3. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTask();
                    break;
                case "2":
                    ViewTasks();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void AddTask()
    {
        Console.Write("Enter Task Name: ");
        string name = Console.ReadLine();

        Console.Write("Enter Deadline (yyyy-mm-dd): ");
        if (DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
        {
            TaskItem newTask = new TaskItem { Name = name, Deadline = deadline };
            tasks.Add(newTask);

            // Save the task to the database
            SaveTaskToDatabase(newTask);

            Console.WriteLine("Task added successfully!");
        }
        else
        {
            Console.WriteLine("Invalid date format. Task not added.");
        }
    }

    static void ViewTasks()
    {
        Console.WriteLine("Tasks:");

        foreach (var task in tasks.OrderBy(t => t.Deadline))
        {
            Console.WriteLine($"- {task.Name} (Deadline: {task.Deadline:yyyy-MM-dd})");
        }

        Console.WriteLine();
    }

    static void SaveTaskToDatabase(TaskItem task)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string insertQuery = "INSERT INTO Tasks (Name, Deadline) VALUES (@Name, @Deadline)";
            using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@Name", task.Name);
                cmd.Parameters.AddWithValue("@Deadline", task.Deadline);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

class TaskItem
{
    public string Name { get; set; }
    public DateTime Deadline { get; set; }
}
