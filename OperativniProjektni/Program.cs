using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace OperativniProjektni
{
    class Program
    {
        static void Main(string[] args)
        {
            bool logged = false;
            bool run = true;
            String input = "";

            // Welcome
            Console.WriteLine("Operativni projektni");
            Console.WriteLine("Student: Ognjen Stefanovic");
            Console.WriteLine("Indeks: 1121/17");
            Console.WriteLine("---------------------------");
            Console.WriteLine();
            while (run)
            {
                Console.Write("->");
                input = Console.ReadLine();
                if (logged)
                {
                    switch (input)
                    {
                        case "go":
                            string dir = @"C:\Users\ognje\Desktop\";
                            Directory.SetCurrentDirectory(dir);
                            break;
                        case "where":
                            Console.WriteLine(AppContext.BaseDirectory);
                            break;
                        case "list":
                            Console.WriteLine(ListFolder(new DirectoryInfo("./../"), "——"));
                            break;
                        case "exit":
                            run = false;
                            break;
                        case "logout":
                            logged = false;
                            break;
                        default:
                            Console.WriteLine("Error, Try Again!");
                            break;
                    }
                } else
                {
                    switch (input)
                    {
                        case "login":
                            logged = true;
                            break;
                        case "exit":
                            run = false;
                            break;
                        default:
                            Console.WriteLine("You are not logged in!");
                            break;
                    }
                }
            }
        }

        static string ListFolder(DirectoryInfo directory, string indentation = "\t", int maxLevel = -1, int deep = 0)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(string.Concat(Enumerable.Repeat(indentation, deep)) + directory.Name);

            if (maxLevel == -1 || maxLevel < deep)
            {
                foreach (var subdirectory in directory.GetDirectories())
                    builder.Append(ListFolder(subdirectory, indentation, maxLevel, deep + 1));
            }

            foreach (var file in directory.GetFiles())
                builder.AppendLine(string.Concat(Enumerable.Repeat(indentation, deep + 1)) + file.Name);

            return builder.ToString();
        }
    }
}
