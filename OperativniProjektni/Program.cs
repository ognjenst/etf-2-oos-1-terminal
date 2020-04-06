using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OperativniProjektni
{
    class Program
    {
        static void PrintDat(string fileName)
        {
            string[] temp = fileName.Split(".");
            if(temp[^1] != "txt")
            {
                Console.WriteLine("Error. File {0} with extention {1} is not a text file.", fileName, temp[^1]);
                return;
            }

            try
            {
                if (File.Exists(fileName))
                    Console.WriteLine(File.ReadAllText(fileName));
                else
                    Console.WriteLine("Error: File does not exist.");
            } catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }
        }
        static void ListDirectory(string[] param)
        {
            if (param.Length == 1)
                try
                { 
                    Console.WriteLine(ListFolder(new DirectoryInfo("./"), "——"));
                } 
                catch(Exception e)
                {
                    Console.WriteLine("Error:{0} ", e.ToString());
                }
            else
                try
                {
                    if (Directory.Exists(param[1]))
                        Console.WriteLine(ListFolder(new DirectoryInfo(@"./" + param[1]), "——"));
                    else
                        Console.WriteLine("Directory does not exists.");
                } catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.ToString());
                } 
        }
        static string ChangeDir(string dir, string currentDir)
        {
            try
            {
                Directory.SetCurrentDirectory(dir);
                return Directory.GetCurrentDirectory();
            }
            catch
            {
                Console.WriteLine("That directory dosen't exist.");
                return currentDir;
            }
        }
        static string ListFolder(DirectoryInfo directory, string indentation = "\t", int maxLevel = -1, int deep = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(String.Concat(Enumerable.Repeat(indentation, deep)) + directory.Name);
            if (maxLevel == -1 || maxLevel < deep)
                foreach (var subdirectory in directory.GetDirectories())
                    builder.Append(ListFolder(subdirectory, indentation, maxLevel, deep + 1));

            foreach (var file in directory.GetFiles())
                builder.AppendLine(String.Concat(Enumerable.Repeat(indentation, deep + 1)) + file.Name);

            return builder.ToString();
        }

        static void MakeFile(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    Console.WriteLine("File exists");
                    return;
                }

                _ = File.Create(fileName);
                Console.WriteLine("File was created successfully at {0}", File.GetCreationTime(fileName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static void MakeFolder(string dir)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                DirectoryInfo di = Directory.CreateDirectory(dir);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(dir));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        static void FindDat(DirectoryInfo directory, string fileName,  int maxLevel = -1, int deep = 0)
        {
            if (maxLevel == -1 || maxLevel < deep)
                foreach (var subdirectory in directory.GetDirectories())
                    FindDat(subdirectory, fileName);

            foreach (var file in directory.GetFiles())
                if (file.Name == fileName)
                    Console.WriteLine(file.FullName);
        }

        static void FindInFile(string[] param)
        {
            string fileName = param[^1];
            string[] temp = fileName.Split(".");
            if (temp[^1] != "txt")
            {
                Console.WriteLine("Error. File {0} with extention {1} is not a text file.", fileName, temp[^1]);
                return;
            }

            try
            {
                if (File.Exists(fileName))
                {
                    int counter = 0;
                    string line;
                    var listArray = new List<string>(param);
                    listArray.RemoveAt(0);
                    listArray.RemoveAt(listArray.Count - 1);

                    StreamReader file = new System.IO.StreamReader(fileName);
                    while((line = file.ReadLine()) != null)
                    {
                        if (line.Contains(String.Join(" ", listArray.ToArray())))
                            Console.WriteLine("Text was found on line {0}", counter + 1);
                        counter++;
                    }
                    file.Close();
                }
                else
                {
                    Console.WriteLine("Error: File does not exist.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }
        }

        static void BasicInfoPrint()
        {
            Console.Clear();
            Console.WriteLine("Operativni projektni");
            Console.WriteLine("Student: Ognjen Stefanovic");
            Console.WriteLine("Indeks: 1121/17");
            Console.WriteLine("---------------------------");
            Console.WriteLine();
        }
        static void WelcomePrint()
        {
            Console.Clear();
            Console.WriteLine("CDMLike interface 1.0");
            Console.WriteLine("====================================================");
            Console.WriteLine(" __      __       .__                               ");
            Console.WriteLine("/  \\    /  \\ ____ |  |   ____  ____   _____   ____  ");
            Console.WriteLine("\\   \\/\\/   // __ \\|  | _/ ___\\/  _ \\ /     \\_/ __ \\ ");
            Console.WriteLine(" \\        /\\  ___/|  |_\\  \\__(  <_> )  Y Y  \\  ___/ ");
            Console.WriteLine("  \\__/\\  /  \\___  >____/\\___  >____/|__|_|  /\\___  >");
            Console.WriteLine("       \\/       \\/          \\/            \\/     \\/ ");
            Console.WriteLine("====================================================");
            Console.WriteLine(); Console.WriteLine();
        }
        static void Main()
        {
            Authentication Auth = Authentication.MakeAuthClient();
            BasicInfoPrint();

            string currentDir = "";
            bool run = true;
            string input;

           
            while (run)
            {
                if (Auth.IsAuth())
                    Console.Write(Auth.getUsername() + "@" + currentDir + "->");
                else
                    Console.Write("Guest ->");
                input = Console.ReadLine();
                if (Auth.IsAuth())
                {
                    string[] param = input.Split(null);
                    switch (param[0])
                    {
                        case "go":
                            if (param.Length < 2)
                                Console.WriteLine("Error: Missing path.");
                            else
                                currentDir = ChangeDir(param[1], currentDir);
                            break;
                        case "where":
                            Console.WriteLine(currentDir);
                            break;
                        case "list":
                            ListDirectory(param);
                            break;
                        case "exit":
                            Auth.Logout();
                            run = false;
                            break;
                        case "logout":
                            Auth.Logout();
                            BasicInfoPrint();
                            break;
                        case "print":
                            if (param.Length == 1)
                                Console.WriteLine("Error: Missing file name");
                            else
                                PrintDat(param[1]);
                            break;
                        case "find":
                            if(param.Length < 2)
                                Console.WriteLine("Error, missing parameters");
                            else
                                FindInFile(param);
                            break;
                        case "findDat":
                            if (param.Length == 1)
                                Console.WriteLine("Error: No file name.");
                            else
                                FindDat(new DirectoryInfo(currentDir), param[1]);
                            break;
                        case "create":
                            if(param.Length == 1)
                            {
                                Console.WriteLine("Error. Missing all parameters");
                            }
                            else if(param.Length == 2)
                            {
                                if (param[1] == "-d")
                                    Console.WriteLine("Missing the directory name.");
                                else
                                    MakeFile(param[1]);
                            } else if(param.Length == 3)
                            {
                                if (param[1] != "-d")
                                    Console.WriteLine("Error. Try again!");
                                else
                                    MakeFolder(param[2]);
                            }
                            break;
                        default:
                            Console.WriteLine("Error, Try Again!");
                            break;
                    }
                }
                else
                {
                    switch (input)
                    {
                        case "login":
                            Console.Write("Enter you username: ");
                            string authUsername = Console.ReadLine();
                            Console.Write("Enter you password: ");
                            string authPassword = Console.ReadLine();

                            if (Auth.Login(authUsername, authPassword))
                            { 
                                currentDir = AppContext.BaseDirectory;
                                WelcomePrint();
                            }

                            break;
                        case "exit":
                            Auth.Logout();
                            run = false;
                            break;
                        default:
                            Console.WriteLine("You are not logged in!");
                            break;
                    }
                }
            }
        }
    }
}
