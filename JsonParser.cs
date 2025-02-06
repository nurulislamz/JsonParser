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

        public bool Parse()
        {
            if (!ValidateBrackets()) throw new Exception("Invalid brackets");

            return true;
        }

        public bool RecursiveDescentParser()
        {

            return true;
        }

        public bool ParseJson()
        {
            JsonToken currentToken = jsonTokens[position];

            if (currentToken.Type == JsonTokenType.CurlyOpen) ParseObject(currentToken);
            if (currentToken.Type == JsonTokenType.SquareOpen) ParseArray(currentToken);

            return true;
        }


        public bool ParseObject(JsonToken currentToken)
        {
            bracketTrack.Push(currentToken.Type);
            return true;
        }

        public bool ParseArray(JsonToken currentToken)
        {
            bracketTrack.Push(currentToken.Type);
            return true;
        }

        public bool ParsePair()
        {
            return true;
        }

        public bool ParseString()
        {
            return true;
        }

        public bool ParseNumber()
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