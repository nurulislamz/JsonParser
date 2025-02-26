

using System.Collections;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;

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


            JsonLexer lexer = Lexer(args);
            List<JsonToken> listJsonTokens = lexer.Tokenize();

            foreach (var token in listJsonTokens)
            {
                Console.WriteLine(token.Type);
            }

            JsonParser jsonParser = new JsonParser(listJsonTokens);
            jsonParser.Parse();

            Dictionary<string, object> json = jsonParser.GetJsonObject();
            PrintNestedDictionary(json);
            return 0;
        }

        private static JsonLexer Lexer(string[] args)
        {
            if (args[0] == "-s")
            {
                return JsonLexerFactory.CreateFromString(args[1]);
            }
            else if (args[0] == "-f")
            {
                return JsonLexerFactory.CreateFromFile(args[1]);
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