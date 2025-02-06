namespace JsonParser
{
    public static class JsonLexerFactory
    {
        public static JsonLexer CreateFromFile(string filePath)
        {
            return new JsonLexer(new FileJsonSource(filePath).GetJson());
        }

        public static JsonLexer CreateFromString(string jsonString)
        {
            return new JsonLexer(new StringJsonSource(jsonString).GetJson());
        }
    }
}