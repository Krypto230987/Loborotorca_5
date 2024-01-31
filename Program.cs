using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TaskManagementApp
{

    public enum Priority
    {
        Low,
        Medium,
        High
    }

    public enum Status
    {
        New,
        InProgress,
        Done
    }



    class Program
    {
        static void Main(string[] args)
        {
            TaskManager taskManager = new TaskManager();
            taskManager.Run();
            Console.ReadLine();
        }
    }
}