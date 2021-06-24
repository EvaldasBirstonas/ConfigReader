using System;

namespace ConfigReader
{
    class Program
    {
        static void Main(string[] args)
        {
            DataModel dataModel = new DataModel();
            Console.WriteLine("Use commands printResult or read {fileLocation} to use the program");
            Console.WriteLine("Try using ../../../Project_Config.txt");
            dataModel.ReadFromFile("../../../Base_Config.txt"); //Base config loading
            while (true)
            {
                Console.WriteLine("-----------------------------------------------------------------------------");
                string[] command = Console.ReadLine().Split(' ');
                switch (command[0].ToLower())
                {
                    case "printresult":
                        if (command.Length > 1)
                            Console.WriteLine("Pop doesn't take any arguments");
                        else
                            dataModel.PrintResults();
                        break;
                    case "read":
                        if (command.Length != 2)
                            Console.WriteLine("Too many or too few arguments passed in.");
                        else
                            dataModel.ReadFromFile(command[1]);
                        break;
                    default:
                        Console.WriteLine("Unknown command: " + command);
                        break;
                }
            }
        }
    }
}
