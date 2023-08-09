using System.IO;
using System.Text.RegularExpressions;

namespace CombinedLetters
{
    public interface ILetterService
    {
        void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile);
    }

    public class LetterService : ILetterService
    {
        public void CombineTwoLetters(string inputFile, string inputFile2, string resultFile)
        {
            return;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            static void GenTests(int fileNumber)
            {
                string currDirectory = Directory.GetCurrentDirectory();

                Random rand = new Random();
                //Create directories 
                if(!Directory.Exists(currDirectory + @"\input\"))
                {
                    Directory.CreateDirectory(currDirectory + @"\Input\Admission");
                    Directory.CreateDirectory(currDirectory + @"\Input\Scholarship");
                }
                //Create date directories for 30 days
                for(int i = 0; i < 30; i++)
                {
                    Directory.CreateDirectory(currDirectory + @"\Input\Admission\" + (20230501 + i));
                    Directory.CreateDirectory(currDirectory + @"\Input\Scholarship\" + (20230501 + i));
                }
                //Create files with random ids
                foreach(string day in Directory.EnumerateDirectories(currDirectory + @"\input\Admission"))
                {
                    for(int i = 0; i < fileNumber; i++){
                    int randId = rand.Next(99999999);
                    string idString = randId.ToString("D6");
                    string dayString = day.Substring(day.Length - 8, 8);
                    File.WriteAllText(day + @"\admission-" + idString + ".txt", "admission-" + idString + ".txt");
                    //Every other file put a corresponding scholarship file
                    if(i % 2 == 0)
                    {
                        File.WriteAllText(currDirectory + @"\Input\Scholarship\" + dayString + @"\scholarship-" + idString + ".txt", "scholarship-" + idString + ".txt");
                    }


                    }

                }

                

                Environment.Exit(0);
            }

            if(args.Length > 0)
            {
                int fileNumber;

                if (args[0] == "-gentest")
                {
                    bool numberConvert = int.TryParse(args[1], out fileNumber);
                    if (numberConvert && fileNumber > 0)
                    {
                        GenTests(fileNumber);
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid Arguments");
                        
                    }

                }
            }

            

        }
    }
}
