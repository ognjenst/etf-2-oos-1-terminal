using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace OperativniProjektni
{
    class Program
    {

        static void PrintDat(String fileName)
        {
            String[] temp = fileName.Split(".");
            if(temp[temp.Length - 1] != "txt")
            {
                Console.WriteLine("Error. File {0} with extention {1} is not a text file.", fileName, temp[temp.Length - 1]);
                return;
            }

            try
            {
                if (File.Exists(fileName))
                {
                    Console.WriteLine(File.ReadAllText(fileName));
                }
                else
                {
                    Console.WriteLine("Error: File does not exist.");
                }
            } catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e.ToString());
            }
        }
        static void ListDirectory(String[] param)
        {
            if (param.Length == 1)
            {
                try
                {
                    Console.WriteLine(ListFolder(new DirectoryInfo("./"), "——"));
                } catch(Exception e)
                {
                    Console.WriteLine("Error:{0} ", e.ToString());
                }
            }
            else
            {
                try
                {
                    if (Directory.Exists(param[1]))
                    {
                        Console.WriteLine(ListFolder(new DirectoryInfo(@"./" + param[1]), "——"));
                    }
                    else
                    {
                        Console.WriteLine("Directory does not exists.");
                    }
                } catch (Exception e)
                {
                    Console.WriteLine("Error: {0}", e.ToString());
                }
            }
                
        }
        static String ChangeDir(String dir, String currentDir)
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

            builder.AppendLine(string.Concat(Enumerable.Repeat(indentation, deep)) + directory.Name);

            if (maxLevel == -1 || maxLevel < deep)
            {
                foreach (var subdirectory in directory.GetDirectories())
                    builder.Append(ListFolder(subdirectory, indentation, maxLevel, deep + 1));
            }

            foreach (var file   in directory.GetFiles())
                builder.AppendLine(string.Concat(Enumerable.Repeat(indentation, deep + 1)) + file.Name);

            return builder.ToString();
        }

        static bool Login(ref String user)
        {
            Console.Write("Enter you username: ");
            String username = Console.ReadLine();
            Console.Write("Enter you password: ");
            String password = Console.ReadLine();

            try
            {
                String lib = File.ReadAllText("../../../users.json");
                Dictionary<String, String>[] users = JsonConvert.DeserializeObject<Dictionary<string, string>[]>(lib);
                foreach(Dictionary<String, String> u in users)
                {
                    if (u["username"] == username && u["password"] == password)
                    {
                        user = u["username"];
                        // A bit of an art
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
                        return true;
                    }
                }
                Console.WriteLine("Error. Username and password don't match!");
                return false;

            }
            catch (Exception e)
            {
                Console.WriteLine("Error {0}", e.ToString());
                return false;
            }
        }

        static bool Logout()
        {
            Console.Clear();
            Console.WriteLine("Operativni projektni");
            Console.WriteLine("Student: Ognjen Stefanovic");
            Console.WriteLine("Indeks: 1121/17");
            Console.WriteLine("---------------------------");
            Console.WriteLine();
            return false;
        }

        static void MakeFile(String fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    Console.WriteLine("File exists");
                    return;
                }

                File.Create(fileName);
                Console.WriteLine("File was created successfully at {0}", File.GetCreationTime(fileName));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static void MakeFolder(String dir)
        {
            try
            {
                if (Directory.Exists(dir))
                {
                    Console.WriteLine("That path exists already.");
                    return;
                }

                // Try to create the directory.
                DirectoryInfo di = Directory.CreateDirectory(dir);
                Console.WriteLine("The directory was created successfully at {0}.", Directory.GetCreationTime(dir));
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }

        static void FindDat(DirectoryInfo directory, String fileName, string indentation = "\t", int maxLevel = -1, int deep = 0)
        {
            if (maxLevel == -1 || maxLevel < deep)
            {
                foreach (var subdirectory in directory.GetDirectories())
                    FindDat(subdirectory, fileName);
            }

            foreach (var file in directory.GetFiles())
            {
                if (file.Name == fileName)
                    Console.WriteLine(file.FullName);
            }
        }

        static void FindInFile(String[] param)
        {
            String fileName = param[param.Length - 1];
            String[] temp = fileName.Split(".");
            if (temp[temp.Length - 1] != "txt")
            {
                Console.WriteLine("Error. File {0} with extention {1} is not a text file.", fileName, temp[temp.Length - 1]);
                return;
            }

            try
            {
                if (File.Exists(fileName))
                {
                    int counter = 0;
                    string line;
                    var listArray = new List<String>(param);
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
        static void Main(string[] args)
        {
            String currentDir = "";
            String username = "";
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
                if (logged)
                    Console.Write(username + "@" + currentDir + "->");
                else
                    Console.Write("Guest ->");
                input = Console.ReadLine();
                if (logged)
                {
                    String[] param = input.Split(null);
                    switch (param[0])
                    {
                        case "go":
                            currentDir = ChangeDir(param[1], currentDir);
                            break;
                        case "where":
                            Console.WriteLine(currentDir);
                            break;
                        case "list":
                            ListDirectory(param);
                            break;
                        case "exit":
                            logged = Logout();
                            run = false;
                            break;
                        case "logout":
                            logged = Logout();
                            username = "Guest";
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
                            logged = Login(ref username);
                            currentDir = AppContext.BaseDirectory;
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
    }
}
