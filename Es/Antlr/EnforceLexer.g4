lexer grammar EnforceLexer;

@header { namespace PakExplorer.Ex.Antlr; }

SINGLE_LINE_COMMENT: '//' ~[\r\n]*           -> skip;
EMPTY_DELIMITED_COMMENT: ('/*/' | '/**/')    -> skip;
DELIMITED_COMMENT: '/*' .*? '*/'             -> skip;
WHITESPACES: [\r\n \t]                       -> channel(HIDDEN);
PREPROCESS: '#' ~[\r\n]*                      -> channel(HIDDEN);

AUTO:          'auto';
AUTOPTR:       'autoptr';
PRIVATE:       'private';
PROTECTED:     'protected';
STATIC:        'static';
OVERRIDE:      'override';
PROTO:         'proto';
NATIVE:        'native';
NOTNULL:       'notnull';
EVENT:         'event';
EXTERNAL:      'external';
REF:           'ref';
REFERENCE:     'reference';
CONST:         'const';
OUT:           'out';
OWNED:         'owned';
INOUT:         'inout';
MODDED:        'modded';
NEW:           'new';
DELETE:        'delete';
CLASS:         'class';
ENUM:          'enum';
EXTENSION:     'extends';
TYPEDEF:       'typedef';
RETURN:        'return';
THIS:          'this';
SUPER:         'super';
THREAD:        'thread';
TYPE_INT:      'int';
TYPE_FLOAT:    'float';
TYPE_BOOL:     'bool';
TYPE_STRING:   'string';
TYPE_VECTOR:   'vector';
VOID:          'void';
VOLATILE:      'volatile';
IF:            'if';
ELSE:          'else';
SWITCH:        'switch';
CASE:          'case';
BREAK:         'break';
FOR:           'for';
FOREACH:       'foreach';
WHILE:         'while';
CONTINUE:      'contine';
//TRUE:          'true';
//FALSE:         'false';
DEFAULT:       'default';
SEALED:        'sealed';
LOCAL:         'local';

OPEN_BRACE:          '{';
CLOSE_BRACE:         '}';
OPEN_BRACKET:        '[';
CLOSE_BRACKET:       ']';
OPEN_PARENS:         '(';
CLOSE_PARENS:        ')';
DOT:                 '.';
COMMA:               ',';
COLON:               ':';
SEMICOLON:           ';';
QMARK:               '?';
PLUS:                '+';
MINUS:               '-';
DQUOTE:              '"';
STAR:                '*';
DIV:                 '/';
PERCENT:             '%';
AMP:                 '&';
BITWISE_OR:          '|';
CARET:               '^';
BANG:                '!';
TILDE:               '~';
ASSIGNMENT:          '=';
LT:                  '<';
GT:                  '>';
OP_INC:              '++';
OP_DEC:              '--';
OP_AND:              '&&';
OP_OR:               '||';
OP_EQ:               '==';
OP_NE:               '!=';
OP_LE:               '<=';
OP_GE:               '>=';
OP_LEFTSHIFT:        '<<';
OP_RIGHTSHIFT:       '>>';
OP_ADD_ASSIGN:       '+=';
OP_SUB_ASSIGN:       '-=';
OP_MULT_ASSIGN:      '*=';
OP_DIV_ASSIGN:       '/=';
//OP_MOD_ASSIGN:        '%=';
OP_OR_ASSIGN:        '|=';
OP_AND_ASSIGN:       '&=';
OP_LEFTSHFT_ASSIGN:  '<<=';
SPACE: ' ';
OP_RIGHTSHFT_ASSIGN: '>>=';

LITERAL_STRING: '"' (EnforceEscapeSequence | .)*? '"';
LITERAL_INTEGER:  Diget+;
LITERAL_FLOAT: FloatingPoint;
LITERAL_BOOLEAN: 'true' | 'false';
LITERAL_NULL: 'null' | 'NULL';

IDENTIFIER: [a-zA-Z0-9_]+;

REUSED_MODIFIERS: PRIVATE | PROTECTED | STATIC;

FUNC_MODIFIER: REUSED_MODIFIERS | OVERRIDE | PROTO | NATIVE;
    FUNC_PARAM_MODIFIER: OUT | INOUT;
VARIABLE_MODIFIER: REUSED_MODIFIERS | AUTOPTR | PROTO | REF | CONST;
CLASS_MODIFIER: MODDED;


fragment EnforceEscapeSequence:
    '\\\\' |
    '\\"' |
    '\\\'';

fragment Diget: [0-9];

fragment FloatingPoint: Diget* '.' Diget*;