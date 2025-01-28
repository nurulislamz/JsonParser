using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonParser
{
    public enum JsonTokenType
    {
        CurlyOpen,
        CurlyClose,
        SquareOpen,
        SquareClose,
        Colon, 
        Comma,
        String,
        Number,
        Boolean,
        Null,
        EOF
    }
}
