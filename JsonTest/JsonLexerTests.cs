using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonParser.JsonTest
{
    public class JsonLexerTests
    {

        [Fact]
        public void Tokenize_Should_Return_CorrectTokens_For_EmptyJson()
        {
            // Arrange
            string json = "{}";
            var lexer = new JsonLexer(json);

            // Act
            List<JsonToken> tokens = lexer.Tokenize();

            // Assert
            Assert.Equal(3, tokens.Count);
            Assert.Equal(JsonTokenType.CurlyOpen, tokens[0].Type);
            Assert.Equal(JsonTokenType.CurlyClose, tokens[1].Type);
            Assert.Equal(JsonTokenType.EOF, tokens[2].Type);
        }

        [Fact]
        public void Tokenize_Should_Return_CorrectTokens_For_SimpleJson()
        {
            // Arrange
            string json = """{"key" : 123}""";
            var lexer = new JsonLexer(json);

            // Act
            List<JsonToken> tokens = lexer.Tokenize();

            // Assert
            Assert.Equal(6, tokens.Count);
            Assert.Equal(JsonTokenType.CurlyOpen, tokens[0].Type);
            Assert.Equal(JsonTokenType.String, tokens[1].Type);
            Assert.Equal("key", tokens[1].Value);
            Assert.Equal(JsonTokenType.Colon, tokens[2].Type);
            Assert.Equal(JsonTokenType.Number, tokens[3].Type);
            Assert.Equal("123", tokens[3].Value);
            Assert.Equal(JsonTokenType.CurlyClose, tokens[4].Type);
            Assert.Equal(JsonTokenType.EOF, tokens[5].Type);
        }
    }
}
