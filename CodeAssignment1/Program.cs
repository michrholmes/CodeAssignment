using System;
using System.IO;
using System.Threading;


namespace CodeAssignment1
{
    class Program
    {
        private const int FILECHECK_DELAY_MS = 500;
        private const int FILE_EXISTS_CHECK_COUNT_BEFORE_WARNING = 3;

        private static string folderPath = null;
        private static bool quitApp = false;
        private static string file = null;


        public static void Main(string[] args)
        {
            RunUI();
        }

        #region UI
        private static void RunUI()
        {
            DisplayWelcome();
            file = (GetValidPathToFolder() + "\\input.txt");

            while (!quitApp) //Future enhancement will use/modify this as needed, for now app will run until console window closed
            {
                FileExistsUI(); //Loop until file found; has delay       
                ProcessFile(file);

                //Thread.Sleep(1000);
            }
        }
        private static void DisplayWelcome()
        {
            Console.WriteLine("You are now using the Fibonacci calculator");
            Console.WriteLine("");
            Console.WriteLine("Close the console window when you want application to stop.");
            Console.WriteLine("");
        }
        private static void FileExistsUI()
        {
            int totalChecksBeforeWarning = FILE_EXISTS_CHECK_COUNT_BEFORE_WARNING;

            while (!File.Exists(file))
            {
                Thread.Sleep(FILECHECK_DELAY_MS);
                if (totalChecksBeforeWarning == 0)
                {
                    Console.WriteLine("File not found for the last {0} checks", FILE_EXISTS_CHECK_COUNT_BEFORE_WARNING);
                    totalChecksBeforeWarning = FILE_EXISTS_CHECK_COUNT_BEFORE_WARNING;
                }
                else
                {
                    totalChecksBeforeWarning--;
                }
            }
        }
        private static string GetValidPathToFolder()
        {
            folderPath = null;
            while (folderPath == null)
            {
                Console.Write("Path to folder> ");
                DirectoryInfo path = new DirectoryInfo(Console.ReadLine().Replace("\"", ""));
                if (!path.Exists)
                {
                    Console.WriteLine("Please provide a path that exists");
                }
                else
                {
                    folderPath = path.FullName;
                }
            }
            return folderPath;
        }
        #endregion //End of UI

        #region Logic
        private static bool IsInputValueValid(int n)
        {
            if (n <= 0 || n > 46)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private static int Fibonacci(int n)
        {
            int[] fib = new int[n + 1];
            fib[0] = 0;
            fib[1] = 1;

            for (int i = 2; i <= n; i++)
            {
                fib[i] = fib[i - 1] + fib[i - 2];
            }


            return fib[n];

        }
        private static int GetIntegerFromFile(string file)
        {
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    //assertion is file is not empty;
                    bool result = Int32.TryParse(sr.ReadLine(), out int number);
                    if (result)
                    {
                        return number;
                    }
                    else
                    {
                        throw new Exception("Attempted conversion to int failed");
                    }
                }
            }
        }
        private static void WriteOutput(string file, int n)
        {
            using (FileStream fs = new FileStream(file, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    sr.Write(n);
                }
            }
        }
        private static void ProcessFile(string file)
        {
            int inputValue = GetIntegerFromFile(file);

            if (!IsInputValueValid(inputValue))
            {
                throw new ArgumentOutOfRangeException("The number retrieved was <= 0 or was > than 46 ");
            }
            else
            {
                WriteOutput(folderPath + "\\output.txt", Fibonacci(inputValue));
            }
            File.Delete(file);
        }
        #endregion //End of Logic
    }
}

