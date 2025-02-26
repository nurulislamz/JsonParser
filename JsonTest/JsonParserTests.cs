using JsonParser;
using Xunit;

public class JsonParserTests
{
    [Fact]
    public void Parse_Should_Return_CorrectDict_For_EmptyJson()
    {
        // Arrange
        string jsonInput = "{}";
        JsonLexer lexer = new JsonLexer(jsonInput);
        List<JsonToken> listJsonTokens = lexer.Tokenize();
        JsonParser.JsonParser jsonParser = new JsonParser.JsonParser(listJsonTokens);

        // Act
        jsonParser.Parse();
        Dictionary<string, object> json = jsonParser.GetJsonObject();

        // Assert
        Assert.NotNull(json);
        Assert.Empty(json); // Empty JSON should return an empty dictionary
    }

   [Fact]
    public void Parse_Should_Return_CorrectDict_For_SimpleJson()
    {
        // Arrange
        string jsonInput = "{ \"name\": \"ChatGPT\", \"age\": 4 }";
        JsonLexer lexer = new JsonLexer(jsonInput);
        List<JsonToken> listJsonTokens = lexer.Tokenize();
        JsonParser.JsonParser jsonParser = new JsonParser.JsonParser(listJsonTokens);

        // Act
        jsonParser.Parse();
        Dictionary<string, object> json = jsonParser.GetJsonObject();

        // Assert
        Assert.NotNull(json);
        Assert.Equal(2, json.Count);
        Assert.Equal("ChatGPT", json["name"]);
        Assert.Equal(4, json["age"]);
    }
}
