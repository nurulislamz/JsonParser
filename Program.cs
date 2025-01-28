

using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace JsonParser
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0) 
            {
                Console.WriteLine("No file parsed.");
                return 2;
            }
            Console.WriteLine($"Looking for file: {args[0]}");
            
            string filePath = args[0];
            JsonParser parseJson = new JsonParser(args[0]);

            if (parseJson.ValidateJson()) Console.WriteLine("valid Json");

            return 0;
        }

    }
}