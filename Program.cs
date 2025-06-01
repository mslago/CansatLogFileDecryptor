using CanSatLogsDecryptor;
using Orbipacket;

namespace CanSatLogsDecryptor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filepath = args[0];
            string outputfile;

            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please provide a file path as an argument.");
                return;
            }

            Decrpytor decryptor = new();

            if (!File.Exists(filepath))
            {
                Console.WriteLine($"File not found: {filepath}");
                return;
            }
            int i = 1;
            while (File.Exists($"{filepath}-{i}-DECRYPTED.txt"))
            {
                i++;
            }
            outputfile = $"{filepath}-{i}-DECRYPTED.txt";

            Console.WriteLine($"Output will be saved to: outputfile");

            decryptor.Decryptor(filepath, outputfile);
        }
    }
}
