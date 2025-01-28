using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParser
{
    public class JsonToken
    {
        public JsonTokenType Type { get; set; }
        public string Value { get; set; }
        public int Position { get; set; }

        public JsonToken(JsonTokenType type, string value, int position)
        {
            this.Type = type;
            this.Value = value;
            this.Position = position;
        }
    }
}
