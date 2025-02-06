namespace JsonParser
{
    public class JsonParser
    {
        private readonly List<JsonToken> jsonTokens;
        private Stack<JsonTokenType> bracketTrack = new Stack<JsonTokenType>();
        public JsonParser(List<JsonToken> jsonTokens)
        {
            this.jsonTokens = jsonTokens;
        }
      }
}