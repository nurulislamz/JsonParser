JSON         ::= VALUE

VALUE        ::= OBJECT
              | ARRAY
              | STRING
              | NUMBER
              | "true"
              | "false"
              | "null"

OBJECT       ::= "{" [PAIR ("," PAIR)*] "}"
PAIR         ::= STRING ":" VALUE

ARRAY        ::= "[" [VALUE ("," VALUE)*] "]"

STRING       ::= '"' CHAR* '"'
NUMBER       ::= DIGIT+
CHAR         ::= any UTF-8 character except " and control characters
DIGIT        ::= [0-9]
