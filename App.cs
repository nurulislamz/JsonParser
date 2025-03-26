using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace JsonParser
{
    public class App
    {
        private readonly IJsonLexerFactory lexerFactory;
        private readonly IJsonParser parser;

        public App(IJsonLexerFactory lexerFactory, IJsonParser parser)
        {
            this.lexerFactory = lexerFactory;
            this.parser = parser;
        }

        public int Run(string[] args)
        { 

            if (args.Length == 0 || args.Length == 1)
            {
                Console.WriteLine("Missing file or string argument.");
                return 2;
            }

            Console.WriteLine($"Looking for file: {args[1]}");


            JsonLexer lexer = CreateLexerFromArgs(args);
            List<JsonToken> listJsonTokens = lexer.Tokenize();

            foreach (var token in listJsonTokens)
            {
                Console.WriteLine(token.Type);
            }

            parser.SetTokens(listJsonTokens);
            parser.Parse();

            Dictionary<string, object> json = parser.GetJsonObject();
            PrintNestedDictionary(json);
            Console.WriteLine("Success");
            return 0;
        }

        private JsonLexer CreateLexerFromArgs(string[] args)
        {
            if (args[0] == "-s")
            {
                return lexerFactory.CreateFromString(args[1]);
            }
            else if (args[0] == "-f")
            {
                return lexerFactory.CreateFromFile(args[1]);
            }
            else
            {
                throw new Exception("Invalid Argument For File Type");
            }
        }

        static void PrintNestedDictionary(object obj, int indent = 0)
        {
            string indentStr = new string(' ', indent * 2);

            if (obj is Dictionary<string, object> dict)
            {
                foreach (var kvp in dict)
                {
                    Console.WriteLine($"{indentStr}{kvp.Key}:");
                    PrintNestedDictionary(kvp.Value, indent+1);
                }
            }
            else if (obj is IList list)
            {
                foreach (var item in list)
                {
                    PrintNestedDictionary(item, indent+1);
                }
            }
            else
            {
                Console.WriteLine($"{indentStr}{obj}");
            }
        }
    }
}