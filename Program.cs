

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

            if (args[1] == "-s") JsonParserFactory.CreateFromString(args[1]); 
            if ( args[1] == "-f" ) JsonParserFactory.CreateFromFile(args[1]); 

            return 0;
        }

    }
}