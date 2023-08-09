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
            Console.WriteLine("Test");

        }
    }
}
