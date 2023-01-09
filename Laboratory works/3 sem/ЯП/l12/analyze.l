%option noyywrap

%{
int nAnon = 0, nGlobal = 0, nLocal = 0;
%}

spaces   (\t|" ")*
symbAny [a-zA-Z?@_$]
name {symbAny}({symbAny}|[0-9])*
type  {spaces}("dd"|"dw"|"du"|"dd"|"dp"|"df"|"dq"|"dt"|"file")
label  {spaces}{name}{spaces}(":"|{type})

%%

^{spaces}"@@"{spaces}":"     { ++nAnon; }
^{label}                 { ++nGlobal; }
^"."{label}              { ++nLocal; }
\n  
.   

%%

int main()
{
    puts("Started working!");
    yylex();
    printf("Anon. labels: %d, Global: %d, Local: %d\n", nAnon, nGlobal, nLocal);
    return 0;
}
