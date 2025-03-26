using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParser
{
    public record JsonToken(JsonTokenType Type, string Value, int Position);
}
