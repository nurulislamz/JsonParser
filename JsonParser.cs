using System.Net.Http.Headers;

namespace JsonParser
{
    public class JsonParser
    {
        private int position;
        private readonly List<JsonToken> jsonTokens;
        private Stack<JsonTokenType> bracketTrack = new Stack<JsonTokenType>();
        public JsonParser(List<JsonToken> jsonTokens)
        {
            this.position = 0;
            this.jsonTokens = jsonTokens;
        }

        public JsonToken GetCurrentJsonToken() => jsonTokens[position];

        public bool Parse()
        {
            if (!ValidateBrackets()) throw new Exception("Invalid brackets");

            return true;
        }

        // parses each symbol with a seperate recursive function.
        public bool RecursiveDescentParser()
        {
            ParseJson();
            return true;
        }

        public bool ParseJson()
        {
            JsonToken currentToken = jsonTokens[position];

            if (currentToken.Type == JsonTokenType.CurlyOpen) ParseObject();
            return true;
        }


        public Dictionary<string, object> ParseObject()
        {
            ParsePair();
        }

        public List<string> ParseArray()
        {
            List<string> arr = new List<string>();

            if (GetCurrentJsonToken().Type == JsonTokenType.SquareOpen) position++;

            while (GetCurrentJsonToken().Type != JsonTokenType.SquareClose)
            {
                if (GetCurrentJsonToken().Type == JsonTokenType.String) arr.Add(ParseString());
                else if (GetCurrentJsonToken().Type == JsonTokenType.String) arr.Add(ParseNumber());
            }
            return arr;
        }

        public KeyValuePair ParsePair()
        {
            string key;
            if (GetCurrentJsonToken().Type == JsonTokenType.String)
            {
                key = GetCurrentJsonToken().Value;
                position++;
            }
            if (GetCurrentJsonToken().Type == JsonTokenType.Colon) position++;

            var value = ParseValue();
                
            return true;
        }

        public object ParseValue()
        {
            if (GetCurrentJsonToken().Type == JsonTokenType.CurlyOpen)
            {
                ParseObject();
            }
            if (GetCurrentJsonToken().Type == JsonTokenType.SquareOpen)
            {
                ParseArray();
            }
            if (GetCurrentJsonToken().Type == JsonTokenType.String)
            {
                return GetCurrentJsonToken().Value;
            }
            if (GetCurrentJsonToken().Type == JsonTokenType.Number)
            {
                ParseNumber();
            }
            if (GetCurrentJsonToken().Type == JsonTokenType.Boolean)
            {
                ParseBoolean();
            }
            return true;
        }

        public int ParseNumber()
        {
            string currentValue = GetCurrentJsonToken().Value;
            int pos = 0;

            char c = currentValue[pos];

            if (c == '"') pos++;
            while (pos < currentValue.Length)
            {
                c = currentValue[pos];

                if (char.IsDigit(c)) pos++;
                else throw new Exception($"Invalid characters {c}");
            }

            if (currentValue[currentValue.Length - 1] != '"') throw new Exception("Missing colon \"");
            
            return currentValue;
        }

        public string ParseString()
        {
            string currentValue = GetCurrentJsonToken().Value;
            int pos = 0;

            while (pos < currentValue.Length)
            {
                char c = currentValue[pos];
                if (char.IsLetter(c)) pos++;
                if (c == '"') pos++;
                else if (c == '\"') pos += 2;
                else if (c == '\\') pos += 2;
                else if (c == '\n') pos += 2;
                else throw new Exception($"Invalid characters {c}");
            }

            if (currentValue[currentValue.Length - 1] != '"') throw new Exception("Missing colon \"");
            return currentValue;
        }

        public bool ParseBoolean()
        {
            return true;
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