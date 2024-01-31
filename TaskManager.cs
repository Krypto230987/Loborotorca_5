using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp
{
    public class TaskManager
    {
        private List<Task> tasks;

        public TaskManager()
        {
            tasks = new List<Task>();
        }

        public void Run()
        {
            LoadTasksFromJson();

            while (true)
            {
                Console.WriteLine("1. Показать все задачи");
                Console.WriteLine("2. Добавить новую задачу");
                Console.WriteLine("3. Обновить описание или статус задачи");
                Console.WriteLine("4. Удалить задачу");
                Console.WriteLine("5. Выход");

                Console.Write("Введите свой выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllTasks();
                        break;
                    case "2":
                        AddNewTask();
                        break;
                    case "3":
                        UpdateTask();
                        break;
                    case "4":
                        DeleteTask();
                        break;
                    case "5":
                        SaveTasksToJson();
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Пожалуйста, попробуйте еще раз.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private void ShowAllTasks()
        {
            Console.WriteLine("All tasks:");

            foreach (var task in tasks)
            {
                string status = task.Status == Status.Done ? "Сделанный" : task.Status.ToString();
                string priority = task.Priority.ToString();
                string overdue = task.DueDate < DateTime.Now ? " (Просрочено)" : "";

                Console.WriteLine($"Заголовок: {task.Title}");
                Console.WriteLine($"\r\nОписание: {task.Description}");
                Console.WriteLine($"\r\nСрок оплаты: {task.DueDate.ToShortDateString()}{overdue}");
                Console.WriteLine($"Дата создания: {task.CreationDate.ToShortDateString()}");
                Console.WriteLine($"\r\nПриоритет: {priority}");
                Console.WriteLine($"Cостояние: {status}");
                Console.WriteLine();
            }
        }

        private void AddNewTask()
        {
            Console.Write("\r\nВведите название задачи: ");
            string title = Console.ReadLine();

            Console.Write("\r\nВведите описание задачи: ");
            string description = Console.ReadLine();

            DateTime dueDate;
            while (true)
            {
                Console.Write("Введите дату сдачи (dd.mm.yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out dueDate))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Неверный формат даты. Пожалуйста, попробуйте еще раз.");
                }
            }

            Priority priority;
            while (true)
            {
                Console.Write("\r\nВведите приоритет задачи (низкий/средний/высокий): ");
                if (Enum.TryParse(Console.ReadLine(), out priority) && Enum.IsDefined(typeof(Priority), priority))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("\r\nНеверный приоритет. Пожалуйста, попробуйте еще раз.");
                }
            }

            Task newTask = new Task
            {
                Title = title,
                Description = description,
                DueDate = dueDate,
                CreationDate = DateTime.Now,
                Priority = priority,
                Status = Status.New
            };

            tasks.Add(newTask);
            SortTasksByPriority();
            Console.WriteLine("Новая задача успешно добавлена.");
        }

        private void UpdateTask()
        {
            Console.Write("Введите индекс задачи, которую вы хотите обновить: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < tasks.Count)
            {
                Task task = tasks[index];
                Console.WriteLine($"Выбранная задача: {task.Title}");

                Console.WriteLine("1. Обновить описание");
                Console.WriteLine("2. Обновить состояние");
                Console.WriteLine("3. Отмена");

                Console.Write("Введите свой выбор: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Введите новое описание: ");
                        string newDescription = Console.ReadLine();
                        task.Description = newDescription;
                        Console.WriteLine("\r\nОписание успешно обновлено.");
                        break;
                    case "2":
                        if (task.Status == Status.New)
                        {
                            Console.WriteLine("1. In Progress");
                            Console.WriteLine("2. Cancel");
                            Console.Write("Enter new status: ");
                            string statusChoice = Console.ReadLine();

                            switch (statusChoice)
                            {
                                case "1":
                                    task.Status = Status.InProgress;
                                    Console.WriteLine("Status updated successfully.");
                                    break;
                                case "2":
                                    Console.WriteLine("Operation canceled.");
                                    break;
                                default:
                                    Console.WriteLine("Invalid choice. Operation canceled.");
                                    break;
                            }
                        }
                        else if (task.Status == Status.InProgress)
                        {
                            Console.WriteLine("1. Сделанный");
                            Console.WriteLine("2. Отмена");
                            Console.Write("Введите новый статус: ");
                            string statusChoice = Console.ReadLine();

                            switch (statusChoice)
                            {
                                case "1":
                                    task.Status = Status.Done;
                                    Console.WriteLine("Статус успешно обновлен.");
                                    break;
                                case "2":
                                    Console.WriteLine("Операция отменена.");
                                    break;
                                default:
                                    Console.WriteLine("Неверный выбор. Операция отменена.");
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Задача уже выполнена. Статус не может быть изменен.");
                        }
                        break;
                    case "3":
                        Console.WriteLine("Операция отменена");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Операция отменена.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid task index.");
            }
        }

        private void DeleteTask()
        {
            Console.Write("Введите индекс задачи, которую хотите удалить: ");
            if (int.TryParse(Console.ReadLine(), out int index) && index >= 0 && index < tasks.Count)
            {
                Task task = tasks[index];
                if (task.Status == Status.New)
                {
                    tasks.RemoveAt(index);
                    Console.WriteLine("\r\nЗадача успешно удалена.");
                }
                else
                {
                    Console.WriteLine("Удалять можно только новые задачи.");
                }
            }
            else
            {
                Console.WriteLine("Неверный индекс задачи.");
            }
        }

        private void SortTasksByPriority()
        {
            tasks = tasks.OrderBy(t => t.Priority).ToList();
        }

        private void LoadTasksFromJson()
        {
            string filePath = "tasks.json";

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                tasks = JsonConvert.DeserializeObject<List<Task>>(json);
            }
            else
            {
                Console.WriteLine("Задачи не найдены. Создавайте новые задачи.");
            }
        }

        private void SaveTasksToJson()
        {
            string filePath = "tasks.json";
            string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
