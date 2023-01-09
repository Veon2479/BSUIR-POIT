unit customTypes;

interface


  type
    TArray = array of string;
    TChar = set of char;

    TErrors = (ENoError, EInvChar {= $0001}, ELongOp {= $0002}, ENotEnoughOps {= $0004},
               ENotEnoughBrackets, ELexemsBoundaryExeeded {= $0008});
    TCharType = (CUnexpected, CSpecial, CLetter, CSign, CDelimeter);

    tCountAr = array of record           //stores operands and operators and their count
                      lex: String;
                      num: integer;
                      isOperator: boolean;
                    end;


  const
        ERRORMSG: array [TErrors] of string = (
            'Everything is good! ',
            'ERROR! Invalid character detected',
            'ERROR! Too long operand detected',
            'ERROR! Not enough operands! Last readed:',
            'ERROR! Number of ''('' and '')'' symbols doesnt match',
            'ERROR! Parser exeeded number of readed lexems!');

        Letters : TChar = ['A'..'Z', 'a'..'z', '_', '0'..'9', '@', '^', '.', '#', '$'];
        Signs : TChar = ['~', ':', '=', '/', '\', '+', '-', '*', '%', '&', '|', '<', '>', '?', {';',} '''', ',', '"'];
        Delimeters : TChar = ['{', '}', '[', ']', '(', ')', ';', ','];

        STR_VARIABLE_HEADER = ' int byte short long boolean char double float ';

      {  TYPES = ' int byte short long boolean char double float void ';
        PREFIXES = ' final private public protected static volatile transient native strictfp abstract synchronized new ';
        POSTFIXES = ' extends throws implements ';    // if classes ignored?
        STRUCTURES = ' class interface package enum ';                 //enum????
        CYCLES = ' do for while ';
        JUMPES = ' break return continue ';
        IGNORED = ' import class package'; //until the EoL
        ENTRIES = ' ( { = < : ? assert catch if else case switch default try catch finally throw';                  //;?????
                                                                 }

        MAJORENTRIES = ' ( { [ = ? : assert if switch try throw ';
        MINORENTRIES = ' else case default catch finally ';
        CYCLES = ' do for while ';
        JUMPES = ' break return continue ';
        SUPER_IGNORED = ' import class package enum '; //until the EoL
        IGNORED = 'new class interface package extends throws implements final private public protected static volatile transient native strictfp abstract synchronized new  int byte short long boolean char double float void String ';

        OP_SIGNS = ' ~ / \ +  - * % & | '' , " ; < > ';
        DSIGNS = ' <= >= == != ++ -- || && ';
        BLACKLIST = ' } ] ) ';
  var
    nLexems: integer;
implementation

end.
