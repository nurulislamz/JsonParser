
namespace JsonParser
{
    public class JsonParser
    {
        private int position;
        private readonly List<JsonToken> jsonTokens;
        private Stack<JsonTokenType> bracketTrack;

        private Dictionary<string, object> jsonObject { get; set; }

        public JsonParser(List<JsonToken> jsonTokens)
        {
            this.position = 0;
            this.jsonTokens = jsonTokens;

            this.jsonObject = new Dictionary<string, object>();
            this.bracketTrack = new Stack<JsonTokenType>();
        }

        private JsonToken GetCurrentJsonToken() => jsonTokens[position];

        public Dictionary<string, object> GetJsonObject() => jsonObject;

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

        private bool ParseJson()
        {
            ConsumeToken(JsonTokenType.CurlyOpen);
            jsonObject = ParseObject();
            return true;
        }


        private Dictionary<string, object> ParseObject()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();

            // handle Empty Json
            if (GetCurrentJsonToken().Type == JsonTokenType.CurlyClose)
            {
                ConsumeToken(JsonTokenType.CurlyClose);
                return dict;
            }

            KeyValuePair<string, object> pair = ParsePair();
            dict.Add(pair.Key, pair.Value);

            // handle KeyValue pair
            while (GetCurrentJsonToken().Type != JsonTokenType.CurlyClose)
            {
                ConsumeToken(JsonTokenType.Comma);
                pair = ParsePair();
                dict.Add(pair.Key, pair.Value);
            } 

            ConsumeToken(JsonTokenType.CurlyClose);
            return dict;
        }

        private KeyValuePair<string, object> ParsePair()
        {
            if (GetCurrentJsonToken().Type != JsonTokenType.String) throw new InvalidOperationException("Excepted string for a key.");

            string key = ConsumeToken(JsonTokenType.String).Value;
            ConsumeToken(JsonTokenType.Colon);
            var value = ParseValue();
                
            return new KeyValuePair<string, object>(key,value);
        }

        private object ParseValue()
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
                case JsonTokenType.Null:
                    return ParseNull();
                default:
                    throw new InvalidOperationException("Unexpected token type.");
            }
        }

        private List<object> ParseArray()
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


        private int ParseNumber()
        {
            JsonToken token = ConsumeToken(JsonTokenType.Number);
            if (int.TryParse(token.Value, out int result)) return result;
            throw new Exception("Number is invalid. Check lexer");
        }

        private string ParseString()
        {
            return ConsumeToken(JsonTokenType.String).Value;
        }

        private bool ParseBoolean()
        {
            var token = ConsumeToken(JsonTokenType.Boolean);
            if (bool.TryParse(token.Value, out bool result)) return result;
            else throw new Exception("Could not parse Boolean");
        }

        private object? ParseNull()
        {
            var token = ConsumeToken(JsonTokenType.Null);
            if (token?.Value == "null") return null; // default for reference types is null
            throw new Exception("Expected null token but found unexpected value.");
        }

        private bool ValidateBrackets()
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