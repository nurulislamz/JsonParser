using System.Net.Http.Headers;

namespace JsonParser
{
    public class JsonParser
    {
        private int position;
        private readonly List<JsonToken> jsonTokens;
        private Stack<JsonTokenType> bracketTrack = new Stack<JsonTokenType>();

        private Dictionary<string, object> jsonObject = new Dictionary<string, object>();

        public JsonParser(List<JsonToken> jsonTokens)
        {
            this.position = 0;
            this.jsonTokens = jsonTokens;
        }

        private JsonToken GetCurrentJsonToken() => jsonTokens[position];

        private JsonToken ConsumeToken(JsonTokenType expectedType)
        {
            JsonToken token = GetCurrentJsonToken();
            if ( token.Type != expectedType)
            {
                throw new InvalidOperationException($"Expected token {expectedType} but found {token.Type}");
            }
            position++;
            return token;
        }

        public bool Parse()
        {
            //if (!ValidateBrackets()) throw new Exception("Invalid brackets");
            ParseJson();
            return true;
        }

        public bool ParseJson()
        {
            ConsumeToken(JsonTokenType.CurlyOpen);
            jsonObject = ParseObject();
            return true;
        }


        public Dictionary<string, object> ParseObject()
        {
            // handle Empty Json
            if (GetCurrentJsonToken().Type == JsonTokenType.CurlyClose)
            {
                ConsumeToken(JsonTokenType.CurlyClose);
                return jsonObject;
            }

            KeyValuePair<string, object> pair = ParsePair();

            // handle KeyValue pair
            while (GetCurrentJsonToken().Type != JsonTokenType.CurlyClose);
            {
                ConsumeToken(JsonTokenType.Comma);
                pair = ParsePair();
                jsonObject.Add(pair.Key, pair.Value);
            } 

            ConsumeToken(JsonTokenType.CurlyClose);
            return jsonObject;
        }
        public KeyValuePair<string, object> ParsePair()
        {
            if (GetCurrentJsonToken().Type != JsonTokenType.String) throw new InvalidOperationException("Excepted string for a key.");

            string key = ConsumeToken(JsonTokenType.String).Value;
            ConsumeToken(JsonTokenType.Colon);
            var value = ParseValue();
                
            return new KeyValuePair<string, object>(key,value);
        }

        public object ParseValue()
        {
            var current = GetCurrentJsonToken();
            switch (current.Type)
            {
                case JsonTokenType.CurlyOpen:
                    ConsumeToken(JsonTokenType.CurlyOpen);
                    var obj = ParseObject();
                    return obj;
                case JsonTokenType.SquareOpen:
                    return ParseArray();
                case JsonTokenType.String:
                    return ConsumeToken(JsonTokenType.String).Value;
                case JsonTokenType.Number:
                    return ParseNumber();
                case JsonTokenType.Boolean:
                    return ParseBoolean();
                default:
                    throw new InvalidOperationException("Unexpected token type.");
            }
        }

        public List<object> ParseArray()
        {
            List<object> arr = new List<object>();
            ConsumeToken(JsonTokenType.SquareOpen);
            
            while (GetCurrentJsonToken().Type != JsonTokenType.SquareClose)
            {
                arr.Add(ParseValue());
                ConsumeToken(JsonTokenType.Comma);
            }
            ConsumeToken(JsonTokenType.SquareClose);
            return arr;
        }


        public int ParseNumber()
        {
            JsonToken token = ConsumeToken(JsonTokenType.Number);
            if (int.TryParse(token.Value, out int result)) return result;
            throw new Exception("Number is invalid. Check lexer");
        }

        public string ParseString()
        {
            return ConsumeToken(JsonTokenType.String).Value;
        }

        public bool ParseBoolean()
        {
            var token = ConsumeToken(JsonTokenType.Boolean);
            if (bool.TryParse(token.Value, out bool result)) return result;
            else throw new Exception("Could not parse Boolean");
        }

        public bool ValidateBrackets()
        {
            for (int i = 0; i < this.jsonTokens.Count; i++)
            {
                JsonTokenType topOfStack;
                JsonTokenType currentToken = this.jsonTokens[i].Type;

                if (currentToken == JsonTokenType.SquareOpen)
                {
                    bracketTrack.Push(currentToken);
                }
                else if (currentToken == JsonTokenType.SquareClose && bracketTrack.TryPeek(out topOfStack) && topOfStack == JsonTokenType.SquareClose)
                {
                    bracketTrack.Pop();
                }

                else if (currentToken == JsonTokenType.CurlyOpen)
                {
                    bracketTrack.Push(currentToken);
                }
                else if (currentToken == JsonTokenType.CurlyClose && bracketTrack.TryPeek(out topOfStack) && topOfStack == JsonTokenType.CurlyClose)
                {
                    bracketTrack.Pop();
                }
            }

            if (bracketTrack.Count > 0) { return false; }
            else { return true; };
        }
    }
}