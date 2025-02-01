namespace JsonParser
{
    public static class JsonParserFactory
    {
        public static JsonParser CreateFromFile(string filePath)
        {
            return new JsonParser(new FileJsonSource(filePath).GetJson());
        }

        public static JsonParser CreateFromString(string jsonString)
        {
            return new JsonParser(new StringJsonSource(jsonString).GetJson());
        }
    }
}