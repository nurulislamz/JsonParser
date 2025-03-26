namespace JsonParser
{

    public interface IJsonLexerFactory
    {
        JsonLexer CreateFromFile(string filePath);
        JsonLexer CreateFromString(string json);
    }
    public class JsonLexerFactory  : IJsonLexerFactory
    {
        public JsonLexer CreateFromFile(string filePath)
        {
            return new JsonLexer(new FileJsonSource(filePath).GetJson());
        }

        public JsonLexer CreateFromString(string jsonString)
        {
            return new JsonLexer(new StringJsonSource(jsonString).GetJson());
        }
    }
}