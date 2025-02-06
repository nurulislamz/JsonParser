

using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace JsonParser
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length == 0 || args.Length == 1) 
            {
                Console.WriteLine("Missing file or string argument.");
                return 2;
            }

            Console.WriteLine($"Looking for file: {args[1]}");

            if (args[0] == "-s")
            {
                JsonLexer jsonLexer = JsonLexerFactory.CreateFromString(args[1]);
            }
            if (args[0] == "-f")
            {
                JsonLexer jsonLexer = JsonLexerFactory.CreateFromFile(args[1]);
                var listJsonTokens = jsonLexer.Tokenize();

                foreach (var token in listJsonTokens)
                {
                    Console.WriteLine(token.Type);
                }
            }

            return 0;
        }
    }
}