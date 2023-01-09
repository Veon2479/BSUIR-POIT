%{
   #include <string.h>
   #define YYSTYPE char*
   #include "bison.tab.h"
   void yyerror(char *s);
%}

%option yylineno
%option noyywrap

%%

";"[^\n]*\n                 {  }
"("                         { return OPENBR; }
")"                         { return CLOSEBR; }
"defvar"                    { return DEFVAR; }
"setq"                      { return SETQ; }
([0-9])+                    { return ID; }
([a-zA-Z])(([a-zA-Z0-9])*)  { return NAME; }
("+"|"-"|"*"|"/")           { return OPER; }
[ \t\r\n]                   {    }
.                           { yyerror("ERR: Invalid character!"); }

%%
