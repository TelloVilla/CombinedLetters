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
        
        public static void GenTests(int fileNumber)
            {
                string currDirectory = Directory.GetCurrentDirectory();

                Random rand = new Random();
                //Create directories 
                Directory.CreateDirectory(currDirectory + @"\Input\Admission");
                Directory.CreateDirectory(currDirectory + @"\Input\Scholarship");
                Directory.CreateDirectory(currDirectory + @"\Output");
                Directory.CreateDirectory(currDirectory + @"\Archive");
                
                //Create date directories for 30 days
                for(int i = 0; i < 30; i++)
                {
                    Directory.CreateDirectory(currDirectory + @"\Input\Admission\" + (20230501 + i));
                    Directory.CreateDirectory(currDirectory + @"\Input\Scholarship\" + (20230501 + i));
                    Directory.CreateDirectory(currDirectory + @"\Output\" + (20230501 + i));
                    Directory.CreateDirectory(currDirectory + @"\Archive\Admission\" + (20230501 + i));
                    Directory.CreateDirectory(currDirectory + @"\Archive\Scholarship\" + (20230501 + i));
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
            public static void ClearTestFiles()
            {
                string currDirectory = Directory.GetCurrentDirectory();

                if(Directory.Exists(currDirectory + @"\Input"))
                {
                    Directory.Delete(currDirectory + @"\Input", true);
                }
                if(Directory.Exists(currDirectory + @"\Archive"))
                {
                    Directory.Delete(currDirectory + @"\Archive", true);
                }
                if(Directory.Exists(currDirectory + @"\Output"))
                {
                    Directory.Delete(currDirectory + @"\Output", true);
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

        public void ArchiveTwoLetters(string inputFile1, string output1, string inputFile2, string output2)
        {
            try
            {
                File.Move(inputFile1, output1);
                File.Move(inputFile2, output2);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Archiving Error: " + ex.Message);
            }
        }

        public void ArchiveDay(string date)
        {
            string currDirectory = Directory.GetCurrentDirectory();

            

            if(Directory.Exists(currDirectory + @"\Input\Admission\" + date)){
                string[] admissionsToArchive = Directory.GetFiles(currDirectory + @"\Input\Admission\" + date, "*.txt");
                 foreach (string admission in admissionsToArchive)
                {
                    try
                    {
                        File.Move(admission, currDirectory + @"\Archive\Admission\" + date + @"\" + Path.GetFileName(admission));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Archiving Error: " + ex.Message);
                    }
                    
                    
                }
                
                Directory.Delete(currDirectory + @"\Input\Admission\" + date);
            }
            if(Directory.Exists(currDirectory + @"\Input\Scholarship\" + date))
            {
                string[] scholarshipToArchive = Directory.GetFiles(currDirectory + @"\Input\Scholarship\" + date, "*.txt");
                foreach (string scholarship in scholarshipToArchive)
                {
                    try
                    {
                        File.Move(scholarship, currDirectory + @"\Archive\Scholarship\" + date + @"\" + Path.GetFileName(scholarship));
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Archiving Error: " + ex.Message);
                    }

                }
                Directory.Delete(currDirectory + @"\Input\Scholarship\" + date);
            }
        }
    }
    class Progam
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
                        LetterService.GenTests(fileNumber);
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid Arguments");
                    }
                }
                else if (args[0] == "-clear")
                {
                    LetterService.ClearTestFiles();
                }
                else
                {
                    Console.WriteLine("Error: Invalid Arguments");
                    Environment.Exit(-1);
                }
            }

            LetterService letterService= new LetterService();

            //Regex pattern to match university ids in file names
            string idPattern = @"[0-9]{8}";
            string currDirectory = Directory.GetCurrentDirectory();

            if(!Directory.Exists(currDirectory + @"\Input\Admission"))
            {
                Console.WriteLine("Error: Input folders missing. Generate test files first.");
                Environment.Exit(-1);
            }



            string[] admissionDirectories = Directory.GetDirectories(currDirectory + @"\Input\Admission");
            

            foreach(string day in admissionDirectories)
            {
                //Variables for Reports
                List<string> combinedIds = new List<string>();
                int totalCombined = 0;

                //For each day get All File paths and names
                string daySubstring = day.Substring(day.Length - 8, 8);
                string[] admissionFiles = Directory.GetFiles(day, "*.txt");

                /* If Scholarship does not have the same date folder then archive day and move on to next day
                as there will be no matches */
                if(!Directory.Exists(currDirectory + @"\Input\Scholarship\" + daySubstring))
                {
                    letterService.ArchiveDay(daySubstring);
                    try
                    {
                        File.WriteAllText(currDirectory + @"\Output\" + daySubstring + @"\" + daySubstring + "-Report.txt",
                        DateTime.Now.ToString("MM/dd/yyyy") + " Report\n" 
                        + "----------\n"
                        + "Number of Combined Letters: " + totalCombined
                        + "\n"
                        );
                        File.AppendAllLines(currDirectory + @"\Output\" + daySubstring + @"\" + daySubstring + " Report.txt", combinedIds);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Report Generation Error: " + ex.Message);
                    }

                    continue;

                }

                string[] scholarshipFiles = Directory.GetFiles(currDirectory + @"\Input\Scholarship\" + daySubstring);
                string[] admissionFileNames = admissionFiles.Select<string, string>(Path.GetFileName).ToArray();
                string[] scholarshipFileNames = scholarshipFiles.Select<string, string>(Path.GetFileName).ToArray();

                

                
                

                for(int i = 0; i < admissionFiles.Length; i++)
                {
                    //Capture university id from filename
                    Match id = Regex.Match(admissionFileNames[i], idPattern);
                    if(id.Success)
                    {
                        for(int j = 0; j < scholarshipFiles.Length; j++)
                        {
                            Match sId = Regex.Match(scholarshipFileNames[j], idPattern);
                            if(sId.Success)
                            {
                                //Check for match in corresponding Scholarship folder
                                if(id.Value == sId.Value)
                                {
                                    totalCombined++;
                                    combinedIds.Add(id.Value);
                                    letterService.CombineTwoLetters(
                                        admissionFiles[i], scholarshipFiles[j],
                                        currDirectory + @"\Output\" + daySubstring + @"\combined-" + id.Value + ".txt"
                                    );

                                    letterService.ArchiveTwoLetters(
                                        admissionFiles[i], currDirectory + @"\Archive\Admission\" + daySubstring + @"\" + admissionFileNames[i],
                                        scholarshipFiles[j], currDirectory + @"\Archive\Scholarship\" + daySubstring + @"\" + scholarshipFileNames[j]
                                    );
                                    
                                }
                            }
                        }
                    }
                    
                }
                //Archive Last files without matches for the day
                letterService.ArchiveDay(daySubstring);
                try
                {
                    File.WriteAllText(currDirectory + @"\Output\" + daySubstring + @"\" + daySubstring + "-Report.txt",
                    DateTime.Now.ToString("MM/dd/yyyy") + " Report\n" 
                    + "----------\n"
                    + "Number of Combined Letters: " + totalCombined
                    + "\n"
                    );
                    File.AppendAllLines(currDirectory + @"\Output\" + daySubstring + @"\" + daySubstring + "-Report.txt", combinedIds);
                }
                catch(Exception ex)
                {
                    Console.WriteLine("Report Generation Error: " + ex.Message);
                }
                
            }

        }

    }
}
