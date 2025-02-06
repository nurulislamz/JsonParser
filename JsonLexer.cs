using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParser
{
    public class JsonLexer
    {
        private readonly string input;
        private int position { get; set; }

        public JsonLexer(string input)
        {
            this.input = input;
            this.position = 0;
        }

        public List<JsonToken> Tokenize()
        {
            var tokens = new List<JsonToken>();

            while (position < input.Length)
            {
                char current = input[position];

                if (char.IsWhiteSpace(current))
                {
                    position++;
                    continue;
                }

                switch (current)
                {
                    case '{':
                        tokens.Add(new JsonToken(JsonTokenType.CurlyOpen, "{", position));
                        position++;
                        break;
                    case '}':
                        tokens.Add(new JsonToken(JsonTokenType.CurlyClose, "}", position));
                        position++;
                        break;
                    case '[':
                        tokens.Add(new JsonToken(JsonTokenType.SquareOpen, "[", position));
                        position++;
                        break;
                    case ']':
                        tokens.Add(new JsonToken(JsonTokenType.SquareClose, "]", position));
                        position++;
                        break;
                    case ':':
                        tokens.Add(new JsonToken(JsonTokenType.Colon, ":", position));
                        position++;
                        break;
                    case ',':
                        tokens.Add(new JsonToken(JsonTokenType.Comma, ",", position));
                        position++;
                        break;
                    case '"':
                        tokens.Add(ReadString());
                        position++;
                        break;
                    default:
                        if (char.IsDigit(current) || current == '-')
                        {
                            tokens.Add(ReadNumber());
                        }
                        else if (IsBooleanOrNull())
                        {
                            tokens.Add(ReadBooleanOrNull());
                        }
                        else
                        {
                            throw new Exception($"Unexpedcted character at {current}");
                        }
                        break;
                }
            }

            tokens.Add(new JsonToken(JsonTokenType.EOF, null, position));
            return tokens;
        }

        private JsonToken ReadString()
        {
            int start = position;
            position++;

            while (position < input.Length && input[position] != '"') 
            {
                char test = input[position];
                position++;
            }

            if (position >= input.Length || input[position] != '"')
            {
                throw new Exception("Unterminated string literal");
            }

            position++;
            string value = input.Substring(start + 1, position - start - 2);
            return new JsonToken(JsonTokenType.String, value, start);
        }

        private JsonToken ReadNumber()
        {
            int start = position;
            position++;

            if (input[position] == '-') { position++; }
            while (position < input.Length && char.IsDigit(input[position])) 
            {
                position++;
            }

            if (position < input.Length && input[position] == '.') 
            {
                position++;
                while (position < input.Length & char.IsDigit(input[position])) position++;
            }

            string value = input.Substring(start, position - start);
            return new JsonToken(JsonTokenType.Number, value, start);
        }

        private JsonToken ReadBooleanOrNull()
        {
            int start = position;
            if (input.Substring(position).StartsWith("true"))
            {
                position += 4;
                return new JsonToken(JsonTokenType.Boolean, "true", start);
            }
            if (input.Substring(position).StartsWith("false"))
            {
                position += 5;
                return new JsonToken(JsonTokenType.Boolean, "false", start);
            }
            if (input.Substring(position).StartsWith("null"))
            {
                position += 4;
                return new JsonToken(JsonTokenType.Boolean, "null", start);
            }

            throw new Exception($"Unexpected token starting at position {start}");
        }

        private bool IsBooleanOrNull()
        {
            return input.Substring(position).StartsWith("true") ||
                input.Substring(position).StartsWith("false") ||
                input.Substring(position).StartsWith("null");
        }
    }
}
