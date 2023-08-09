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
        static void Main(string[] args)
        {
            

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
                else
                {
                    Console.WriteLine("Error: Invalid Arguments");
                    Environment.Exit(-1);
                }
            }

            string idPattern = @"[0-9]{8}";
            string currDirectory = Directory.GetCurrentDirectory();

            string[] admissionDirectories = Directory.GetDirectories(currDirectory + @"\Input\Admission");
            

            foreach(string day in admissionDirectories)
            {
                string daySubstring = day.Substring(day.Length - 8, 8);
                string[] admissionFiles = Directory.GetFiles(day, "*.txt");
                string[] scholarshipFiles = Directory.GetFiles(currDirectory + @"\Input\Scholarship\" + daySubstring, "*.txt");
                string[] admissionFileNames = admissionFiles.Select<string, string>(Path.GetFileName).ToArray();
                string[] scholarshipFileNames = scholarshipFiles.Select<string, string>(Path.GetFileName).ToArray();

                for(int i = 0; i < admissionFiles.Length; i++){
                    Match id = Regex.Match(admissionFileNames[i], idPattern);
                    if(id.Success)
                    {
                        for(int j = 0; j < scholarshipFiles.Length; j++)
                        {
                            Match sId = Regex.Match(scholarshipFileNames[j], idPattern);
                            if(sId.Success)
                            {
                                if(id.Value == sId.Value)
                                {
                                    
                                }
                            }
                        }
                    }
                }

            }

        }
        public static void GenTests(int fileNumber)
            {
                string currDirectory = Directory.GetCurrentDirectory();

                Random rand = new Random();
                //Create directories 
                if(!Directory.Exists(currDirectory + @"\Input\"))
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
                foreach(string day in Directory.EnumerateDirectories(currDirectory + @"\Input\Admission"))
                {
                    for(int i = 0; i < fileNumber; i++)
                    {
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
        public void CombineTwoLetters(string inputFile1, string inputFile2, string resultFile)
        {
            try{
                string content1 = File.ReadAllText(inputFile1);
                string content2 = File.ReadAllText(inputFile2);

                try
                {
                    File.WriteAllText(resultFile, content1 + content2);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
    }
    
}
