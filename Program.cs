using CanSatLogsDecryptor;
using Orbipacket;

namespace CanSatLogsDecryptor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string filepath = args[0];
            if (string.IsNullOrEmpty(filepath))
            {
                Console.WriteLine("Please provide a file path as an argument.");
                return;
            }
            Decrpytor decryptor = new();
            decryptor.Decryptor(filepath, $"{filepath}-DECRYPTED.txt");
        }
    }
}
