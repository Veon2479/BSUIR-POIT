%{
    #include <stdio.h>
    #include <stdlib.h>
    extern int yylineno;
    extern int yylex();
    int errCnt = 0;
    void yyerror(char *s) {
       fprintf(stderr, "<%s>, line %d\n", s, yylineno);
       ++errCnt;
       exit(1);
    #define YYSTYPE char*
    }
%}

%token OPENBR CLOSEBR
%token OPER DEFVAR SETQ
%token NAME ID

%%

PROGRAM:     OPS;

OPS:    OP 
        | OPS OP;
        
OP:     OPENBR DEFVAR NAME OBJ CLOSEBR
        | OPENBR SETQ NAME OBJ CLOSEBR
        | OPENBR OPER OBJ OBJ CLOSEBR;
        
OBJ:    ID 
        | NAME 
        | OPENBR LIST CLOSEBR;
        | OP;
        
LIST:   ID LIST
        |
        | NAME LIST;

%%

int main()
{
    int num = yyparse();
    if (errCnt == 0)
        printf("Code is correct!\n");
    return num;
}
