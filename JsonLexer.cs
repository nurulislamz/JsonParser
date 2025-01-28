using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParser
{
    public class JsonLexer
    {
        private readonly string _input;
        private int _position { get; set; }

        public JsonLexer(string input)
        {
            this._input = input;
            this._position = 0;
        }

        public List<JsonToken> Tokenize()
        {
            var tokens = new List<JsonToken>();

            while (_position < _input.Length)
            {
                char current = _input[_position];

                if (char.IsWhiteSpace(current))
                {
                    _position++;
                    continue;
                }

                switch (current)
                {
                    case '{':
                        tokens.Add(new JsonToken(JsonTokenType.CurlyOpen, "{", _position));
                        _position++;
                        break;
                    case '}':
                        tokens.Add(new JsonToken(JsonTokenType.CurlyClose, "}", _position));
                        _position++;
                        break;
                    case '[':
                        tokens.Add(new JsonToken(JsonTokenType.SquareOpen, "[", _position));
                        _position++;
                        break;
                    case ']':
                        tokens.Add(new JsonToken(JsonTokenType.SquareClose, "]", _position));
                        _position++;
                        break;
                    case ':':
                        tokens.Add(new JsonToken(JsonTokenType.Colon, ":", _position));
                        _position++;
                        break;
                    case ',':
                        tokens.Add(new JsonToken(JsonTokenType.Comma, ",", _position));
                        _position++;
                        break;
                    case '"':
                        tokens.Add(ReadString());
                        _position++;
                        break;
                }
            }
        }

        private JsonToken ReadString()
        {
            int start = _position;
            _position++;

            while (_position < _input.Length && _input[_position] != '"') 
            {
                _position++;
            }

            if (_position >= _input.Length || _input[_position] != '"')
            {
                throw new Exception("Unterminated string literal");
            }

            _position++;
            string value = _input.Substring(start + 1, _position - start - 2);
            return new JsonToken(JsonTokenType.String, value, start);
        }

        private JsonToken ReadNumber()
        {
            int start = _position;
            _position++;

            while (_position < _input.Length && char.IsDigit(_input[_position])) 
            {
                _position++;
            }

            if (_position < _input.Length && _input[_position] == '.') 
            {
                _position++;
                while (_position < _input.Length & char.IsDigit(_input[_position])) _position++;
            }

            string value = _input.Substring(start, _position - start);
            return new JsonToken(JsonTokenType.String, value, start);
        }

        private JsonToken ReadBooleanOrNull()
        {
            int start = _position;
            if (_input.Substring(_position).StartsWith("true"))
            {
                _position += 4;
                return new JsonToken(JsonTokenType.Boolean, "true", start);
            }
            if (_input.Substring(_position).StartsWith("false"))
            {
                _position += 5;
                return new JsonToken(JsonTokenType.Boolean, "false", start);
            }
            if (_input.Substring(_position).StartsWith("null"))
            {
                _position += 4;
                return new JsonToken(JsonTokenType.Boolean, "null", start);
            }
        }
    }
}
