unit CodeParser;

interface
    uses
        System.SysUtils,
        Math,
        System.ioutils, types, StrUtils, Vcl.Graphics,  Vcl.ExtCtrls,
        Vcl.Dialogs, classes,
        customTypes;

 //   type
       // TArray = array of string;

    //    TChar = set of char;

    //    TErrors = (ENoError, EInvChar {= $0001}, ELongOp {= $0002}, ENotEnoughOps {= $0004},
      //      ENotEnoughBrackets, ELexemsBoundaryExeeded {= $0008});
      //  TCharType = (CUnexpected, CSpecial, CLetter, CSign, CDelimeter);


    

    procedure parseLexem(var lexems: TArray; var lexem: string; var nLexems: integer);

    procedure anLexems(const lexems: TArray; const nLexems: integer; var i: integer);

    procedure anCode(const input: textFile; out lexems: TArray; out nLexems: integer);

implementation

    // main
    procedure parseLexem(var lexems: TArray; var lexem: string; var nLexems: integer);
    begin
        if lexem <> '' then
        begin
            inc(nLexems);
            if nLexems = 0 then
                setLength(lexems, 1)
            else if length(lexems) <= nLexems then
                setLength(lexems, length(lexems) * 2);
            lexems[nLexems] :=  lexem;
            lexem:= '';
        end;
    end;

    //=========Code Analysis================
    procedure anLexems(const lexems: TArray; const nLexems: integer; var i: integer);
    var lexem: string;
        counter: integer;
        atEnd: boolean;

        ERROR: word;
        procedure handleError(const err: TErrors);
        begin
            if ERROR = 0 then
            begin
                ERROR:= ord(err);
            end;
            ShowMessage('WoopS! An error occured while parsing primary code!'#13#10' Message: ' + ERRORMSG[err]);
        end;

        procedure anOperator; //(lexems, nLexems, i, counter)
        var lexem: string;
            atEnd: boolean;
      begin

//            result:= iniBlock(BProcess);
//            atEnd:= false;
//
//            //все проверить
//            while (not atEnd) and (ERROR = 0) do
//            begin
//                //read new lexem
//                if (i < nLexems) then
//                    inc(i);
//                //else
//                    //handleError(ELexemsBoundaryExeeded);
//                lexem:= lowerCase(lexems[i]);
//
//                //ShowMessage(lexem);
//                // an lexem
//
//            {}  if (lexem = 'begin') {}or ((copy(lexem, 1, 8) = '//$begin')){} then //new operator
//                begin
//                    result.btype:= BComplex;
//                    if (lexem[1] = '/') then //special bcomplex
//                    begin
//                        result.text := TStringList.Create;
//                        result.text.add( copy(lexem, 10, length(lexem) - 9));
//                    end;
//
//                    counter:= 0;
//                    // detect end of complex operator
//                    repeat
//                        temp:= anOperator; // detect end of complex operator
//                        //check and merdge multiple BProcess
//                        if (temp <> nil) and (temp.btype = BProcess) then
//                        begin
//                            if counter > 0 then
//                            begin
//                                result.childs[result.nChilds].text.addStrings(temp.text);
//                                temp.text.Free;
//                                dispose(temp);
//                                temp:= nil;
//                            end;
//                            counter:= (counter + 1) mod setup.linesAtOnce; //(3 ops in block)
//                        end
//                        else
//                            counter:= 0;
//                        //
//                        pushBlock(temp, result);
//                        lexem:= lowercase(lexems[i]);
//                    until (lexem = 'end') or (lexem = 'end.') {}or (lexem = '//$end'){};
//                    if lexem = '//$end' then
//                    begin
//                        atEnd:= true;
//                        lexems[i]:= '//$ [terminated]'; // изменение для предотвращ беск цикла (тк лексема не меняется, и мы продолж выходить)
//                    end;
//                end
//                else if (lexem = 'end') or (lexem = 'end.') or (lexem = 'until') or (lexem = ';') or (lexem = 'else') {}or (lexem = '//$end'){} then
//                begin
//                    if checkOnDefault(result) then
//                    begin
//                        dispose(result);
//                        result:= nil;
//                    end;
//                    atEnd := true;
//                end
//                else if copy(lexem, 1, 4) = '//$c' then
//                begin
//                    counter:= 0; // сброс
//                    result.btype:= BComment;  result.text.AddStrings( convStrToList(copy(lexem, 5, length(lexem) - 4), '$') );
//                    atEnd:= true;
//                end
//                else if ((copy(lexem, 1, 2) = '//') or (lexem[1] = '{') or (copy(lexem, 1, 2) = '(*')) {}and ((copy(lexem, 1, 3) <> '//$')){} then    //handle comment
//                begin
//                    counter:= 0; // сброс
//                end
//                else if (lexem = 'with') then
//                begin
//                    repeat
//                        inc(i);
//                        lexem:= lowerCase(lexems[i]);
//                    until lexem = 'do';
//                end
//            {}  else if (lexem = 'while') or (lexem = 'for') then
//                begin
//                    result.btype:= BCycle;  result.text.add(lexem + ' ');
//                end
//                else if (lexem = 'do') then
//                begin
//                    pushBlock(anOperator, result);
//                    atEnd:= true; //??
//                end
//            {}  else if (lexem = 'repeat') then //as BEGIN
//                begin
//                    result.btype:= BRCycle;
//
//                    counter:= 0;//an multiple line BProcess
//                    repeat
//                        temp:= anOperator; // detect end of complex operator
//
//                        //check and merdge multiple BProcess    (as BComplex)
//                        if (temp <> nil) and (temp.btype = BProcess) then
//                        begin
//                            if counter > 0 then
//                            begin
//                                result.childs[result.nChilds].text.addStrings(temp.text);
//                                dispose(temp);
//                                temp:= nil;
//                            end;
//                            counter:= (counter + 1) mod setup.linesAtOnce; //(3 ops in block)
//                        end
//                        else
//                            counter:= 0;
//
//                        pushBlock(temp, result); //auto protects from nil
//                    until (lowercase(lexems[i]) = 'until');
//                    result.text.add(lowercase(lexems[i]) + ' ');
//                end
//                else if (lexem = 'if') then
//            {}  begin
//                    result.btype:= BIf;  result.text.add(lexem + ' ');
//                end
//                else if (lexem = 'then') then
//                begin
//                    pushBlock(anOperator, result);
//                    if lowercase(lexems[i]) = 'else' then
//                        pushBlock(anOperator, result);
//                    atEnd:= true;
//                end
//                else if (lexem = 'case') then
//            {}  begin
//                    result.btype:= BSwitch; result.text.add(lexem + ' ');
//                end
//                else if (lexem = 'of') then //as BEGIN
//                begin
//                    temp:= anOperator; // detect end of complex operator
//                    while (lowercase(lexems[i]) <> 'end') do
//                    begin
//                        pushBlock(temp, result);
//                        temp:= anOperator;
//
//                        if lowercase(lexems[i]) = 'else' then   //if have else case -- forse read next op
//                            temp:= anOperator;
//                    end;
//                end
//                else if (lexem = ':') then
//                begin
//                    result.btype:= BCase;
//                    pushBlock(anOperator, result);
//                    atEnd:= true;
//                end
//              {}else
//                    with result^ do
//                    begin
//                        if text.count = 0 then
//                            text.add('');
//                        text[result.text.Count - 1]:=  result.text[result.text.Count - 1] + lexems[i] + ' ';
//                    end;
//            end;
        end;

        procedure anFunction();  // enter with lexem begin/procedure
        var name, args: TStrings; tmps: string;
        begin
            
        end;

    begin
        ERROR:= 0;  atEnd:= false;
        while not atEnd and (ERROR = 0) do
        begin
            //read new lexem
            if (i < nLexems) then
                inc(i);

            lexem:= lowerCase(lexems[i]);

            if pos(lexem, STR_VARIABLE_HEADER) <> 0 then

        end;
    end;

    procedure anCode(const input: textFile; out lexems: TArray; out nLexems: integer);
    var comment, finish, ignoreBracket: boolean;
        //c, c_last: char;  //
        chrType: TCharType;
        tmp_chr, last_tmp_chr: string; // complex char buffer
        lexem: string; // cur lexem

        ERROR: word;
        procedure handleError(const err: TErrors);
        var se: string;
        begin
            if ERROR = 0 then
            begin
                ERROR:= ord(err);
                lexem:= 'lexem <' + lexem + '>, last char met <' + tmp_chr + '>, previous sign char met <' + last_tmp_chr + '>';
                parseLexem(lexems, lexem, nLexems);
                se:= ERRORMSG[err];
                parseLexem(lexems, se, nLexems);
            end;
            //ShowMessage('WoopS! An error occured while parsing primary code! Code: ' + IntToStr(ord(err)));
            writeln('WoopS! An error occured while parsing primary code! '#13#10'Error msg: ', ERRORMSG[err], ' at lexem #', nLexems ,', '#13#10'caused by ', lexems[nLexems-1]);
            finish := true;
        end;
        function readNextChar(out next_char: string) : TCharType;// protected way to read next char
        var c: char;
        begin
            if Eof(input) then
            begin
                handleError(ELexemsBoundaryExeeded);  // throw error
                result:= CUnexpected
            end
            else
            begin
                read(input, c);
                next_char := c;

                if (c <= #$20) then     // detect char type
                    result := CSpecial
                else if c in Letters then
                    result:= CLetter
                else if c in Signs then
                    result:= CSign
                else if c in Delimeters then
                    result:= CDelimeter
                else
                    result:= CUnexpected;
                case c of  //isnt protected!! // this case is for detection of complex comment symbols (as /*,  //, \", \')
                    // <first char of complex symbol>: code that handles reading next symbol
                    '\': begin
                        read(input, c);
                        next_char:= next_char + c;
                    end;
                    '/': begin
                        read(input, c);    // Danger!! unprotected reading!
                        if (c = '*') or (c = '/') then  // next char belongs to complex symbol
                            next_char:= next_char + c
                        else  // next char is different
                        begin
                            parseLexem(lexems, next_char, nLexems);
                            next_char:= c;
                        end;
                    end;
                    '*': begin
                        read(input, c);   // Danger!! unprotected reading!
                        if (c = '/') then // next char belongs to complex symbol
                            next_char:= next_char + c
                        else    // next char is different
                        begin
                            parseLexem(lexems, next_char, nLexems);
                            next_char:= c;
                        end;
                    end;
                end;

            end;

        // temp unexpected char met
        if result = CUnexpected then
            handleError(EInvChar);
        end;

    begin
        nLexems:= -1; ERROR:= 0;

        comment:= false; finish:= false; // if were parsing comment; if ended b4 reaching Eof

        chrType:= readNextChar(tmp_chr);
        while not Eof(input) and not finish do//read file chr by chr
        begin
            while chrType = CLetter do  //parse word
            begin
                lexem := lexem + tmp_chr;

                chrType:= readNextChar(tmp_chr);// read next
            end;
            parseLexem(lexems, lexem, nLexems);

            while (chrType = CSign) and not comment and not finish do   //parse signs
            begin
                //check on "comment" start (grouped chars)
                if (tmp_chr = '/*') or (tmp_chr = '//') or (tmp_chr = '"') or (tmp_chr = '''') then
                    comment:= true ;
                lexem := lexem + tmp_chr;   // add symbol to lexem
                last_tmp_chr:= tmp_chr;     //save 1st comment symbol

                chrType:= readNextChar(tmp_chr); // read next
            end;

            while comment and not finish do                 //parse comment
            begin
                if chrType = CSpecial then
                    lexem := lexem + ' ' // replace control symbols with ' '
                else
                    lexem := lexem + tmp_chr;

                //if comment ended
                if ((last_tmp_chr = '''') and (tmp_chr = ''''))
                 or ((last_tmp_chr = '"') and (tmp_chr = '"'))
                 or ((last_tmp_chr = '/*') and (tmp_chr = '*/'))
                 or ((last_tmp_chr = '//') and (tmp_chr = #13)) then
                    comment := false;   // stop loop

                 chrType:= readNextChar(tmp_chr); // read next
            end;
            parseLexem(lexems, lexem, nLexems); // read next

            while (chrType = CDelimeter) and not finish do  //parse delimeters
            begin
                lexem := tmp_chr;
                parseLexem(lexems, lexem, nLexems);

                chrType:= readNextChar(tmp_chr); // read next
            end;

            while (chrType = CSpecial) and not finish do  //while "space" symbols wait for normal ones
                chrType:= readNextChar(tmp_chr); // read next
        end;

        //handle error (if proc returns negative num of lexems -- smth went wrong, 2 last lexems in array -- description of error
        if ERROR <> 0 then
            nLexems:= - nLexems;
    end;

initialization
end.
