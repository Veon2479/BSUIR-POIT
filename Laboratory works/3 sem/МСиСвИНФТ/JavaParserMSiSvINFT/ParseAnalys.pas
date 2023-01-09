unit ParseAnalys;



interface
  uses
    customTypes, Math;


  var
    numLength: integer = 0;

  procedure countLex(var COUNT: tCountAr; const LEXEMS: tArray);
  procedure formOut(const COUNT: tCountAr; var f: textFile);

implementation

  procedure modifyLen(var COUNT: tCountAr; var maxLen: integer );
    begin
      if (maxLen<>0) then
          inc(maxLen, maxLen)
        else
          maxLen:=1;
      setLength(COUNT, maxLen);
    end;

  function isExist(var COUNT: tCountAr; const TMP: String): integer;
    var
      i: integer;
      f: boolean;
    begin
      if length(COUNT)<>0 then f:=true
        else f:=false;
      i:=0;
      RESULT:=-1;
      while f do
        begin
          if COUNT[i].lex=TMP then
            begin
              f:=false;
              RESULT:=i;
            end;
          inc(i);
          if i=length(COUNT) then f:=false;
        end;
    end;

  procedure addlex(var COUNT: tCountAr; var CURLEN, MAXLEN: integer; const TMP: String; const IS_OPERATOR: boolean);
    var
      num: integer;
    begin

      num:=isExist(COUNT, TMP);
      if pos(TMP, BLACKLIST)=0 then
      if num<>-1 then inc(COUNT[num].num)
        else
          begin
            if CURLEN>=MAXLEN-1 then modifyLen(COUNT, MAXLEN);

            with COUNT[CURLEN] do
              begin
                lex:=copy(TMP, 1, length(TMP));
                isOperator:=IS_OPERATOR;
                num:=1;
              end;
            inc(CURLEN);
          end;
    end;

  function findTerm(const tmp: String): integer;
    begin         //for, while - '(', method - '{', other - ';'    //скобочные последовательности???
      if tmp='(' then RESULT:=1
        else if tmp='{' then RESULT:=2
          else if tmp='<' then RESULT:=3
              else if ((tmp='assert') or (tmp='throw') or (tmp=':')) then RESULT:=4
                else if tmp='[' then RESULT:=6
                else RESULT:=5;
    end;

  function isLexEnd(const LEXEMS: tArray; const POSIT: integer; const MODE: integer): boolean;
      var
        tmp: String;
      begin
        RESULT:=false;
        tmp:=LEXEMS[POSIT];
        case MODE of
          0: if POSIT=length(LEXEMS) then RESULT:=true;
          1: if tmp=')' then RESULT:=true;
          2: if tmp='}' then RESULT:=true;
          3: if tmp='>' then RESULT:=true;
          4: if tmp=';' then RESULT:=true;
          5: if (tmp=';')or(tmp='}') then RESULT:=true;
          6: if tmp=']' then RESULT:=true;
        end;
      end;

  function isRightLex(LEX, DICT: String): boolean;
    var
      p, i: integer;

    begin
      RESULT:=true;
      LEX:=' '+LEX+' ';
      p:=pos(LEX, DICT);
      if p=0 then RESULT:=false;
      if RESULT then
        begin
          i:=p;
          while DICT[i]<>' ' do
            inc(i);
          if pos(copy(DICT, p, i-p+1), LEX)=0 then RESULT:=false;
        end;
    end;

  procedure countLex(var COUNT: tCountAr; const LEXEMS: tArray);
    var
      maxLen, curLen: integer;



    procedure crawl(var POSIT: integer; const MODE: integer);   //0 - until the EoF, 1-), 2-}, 3->, 4-;, 5-; or }  6-]
      var
        tmp: String;
        cur, len, number: integer;
        isWaiting, isEnd: boolean;
      begin
        cur:=POSIT;
        isEnd:=false;
        len:=length(LEXEMS);
        isWaiting:=false;
        while not isEnd do
          begin
           // if cur=nLexems then break;
            tmp:=LEXEMS[cur];
            if tmp='' then break;

            if (isRightLex(tmp,MAJORENTRIES)) then
                begin
                  if tmp='(' then addLex(COUNT, curLen, maxLen, '(..)', true)
                    else if tmp='{' then addLex(COUNT, curLen, maxLen, '{..}', true)
                      else if tmp='<' then addLex(COUNT, curLen, maxLen, '<..>', true)
                        else if tmp='[' then addLex(COUNT, curLen, maxLen, '[..]', true)
                          else if tmp='if' then addLex(COUNT, curLen, maxLen, 'if..then..else', true)
                            else if tmp='switch' then addLex(COUNT, curLen, maxLen, 'switch..case', true)
                              else if tmp='try' then addLex(COUNT, curLen, maxLen, 'try..catch..finally', true)
                                 else if tmp='?' then addLex(COUNT, curLen, maxLen, '?..:..', true)
                                  else addLex(COUNT, curLen, maxLen, tmp, true);


                  inc(cur);
                  crawl(cur, findTerm(tmp));
                end
              else if (isRightLex(tmp,MINORENTRIES)) then
                  begin
                    inc(cur);
                    crawl(cur, findTerm(tmp));
                  end
                else if ( isLexEnd(LEXEMS, cur, MODE) ) then
                    begin
                      if (MODE<>0)and(tmp<>')')and(tmp<>']')and(tmp<>'}') then
                          begin
                            addLex(COUNT, curLen, maxLen, tmp, true);

                          end;
                      inc(cur);
                      isEnd:=true;
                      POSIT:=cur;
                    end
                  else if (isRightLex(tmp,SUPER_IGNORED)) then
                      begin
                        if tmp<>'enum' then
                            while (LEXEMS[cur]<>';')and(LEXEMS[cur]<>'{') do
                              inc(cur)
                          else
                            begin
                              inc(cur, 2);
                              crawl(cur, findTerm(LEXEMS[cur]));
                            end;
                      end
                    else if (isRightLex(tmp,IGNORED)) then
                        begin
                          inc(cur);
                        end

                    else if (isRightLex(tmp,DSIGNS)) then
                        begin
                          addLex(COUNT, curLen, maxLen, tmp, true);
                          inc(cur);
                        end

                    else if ((isRightLex(tmp,OP_SIGNS)) )or ( (isRightLex(tmp,JUMPES))) then
                        begin
                          addLex(COUNT, curLen, maxLen, tmp, true);
                          inc(cur);
                        end
                      else  if (isRightLex(tmp,CYCLES)) then
                          begin
                            if tmp='do' then
                                begin
                                  addLex(COUNT, curLen, maxLen, 'do..while', true);
                                  isWaiting:=true;
                                  inc(cur);
                                  crawl(cur, findTerm(tmp));
                                  inc(cur);
                                end
                              else if tmp='while' then
                                  begin
                                    if not isWaiting then
                                        begin
                                          addLex(COUNT, curLen, maxLen, 'while', true);
                                          inc(cur);
                                          crawl(cur, findTerm(tmp));
                                          inc(cur);
                                          crawl(cur, findTerm(LEXEMS[cur]));
                                          inc(cur);
                                        end
                                      else
                                        begin
                                          isWaiting:=false;
                                          inc(cur);
                                          crawl(cur, findTerm(LEXEMS[cur]));
                                          inc(cur);
                                        end;
                                  end
                                else
                                  begin
                                    addLex(COUNT, curLen, maxLen, 'for', true);
                                    inc(cur);
                                    crawl(cur, findTerm(LEXEMS[cur]));
                                    inc(cur);
                                  end;
                          end

                        else
                          begin

                            if not ((LEXEMS[cur][1]='/')and( (LEXEMS[cur][2]='/')or(LEXEMS[cur][2]='*') ) ) then
                              begin
                                number := isExist(COUNT, tmp);
                                if number<>-1 then
                                    begin
                                      inc(COUNT[number].num);
                                    end
                                  else
                                    begin
                                      if (LEXEMS[cur+1]='(') then
                                          begin
                                            addLex(COUNT, curLen, maxLen, tmp, true);
                                            inc(cur);
                                            crawl(cur, 1);
                                          end
                                        else
                                          begin
                                            addLex(COUNT, curLen, maxLen, tmp, false);
                                          end;
                                    end;
                              end;
                            inc(cur);
                          end;

          end;


         POSIT:=cur;

      end;

    var
      curPos: integer;
    begin

      maxLen:=0;
      curLen:=0;
      curPos:=0;
      modifyLen(COUNT, maxLen);
      crawl(curPos, 0);
      numLength:=curLen;
    end;


  procedure formOut(const COUNT: tCountAr; var f: textFile);
    var
      i, k, len: integer;
      operatorDict, operandDict, operatorGen, operandGen: integer;
    begin
      operatorDict:=0;
      operandDict:=0;
      operatorGen:=0;
      operandGen:=0;
      i:=0;
      k:=1;
      writeln(f, 'Operators:');
      writeln(f, 'num':5,'entries  ':10,' lexem');
      for i:=0 to numLength-1 do
        begin
          if (COUNT[i].isOperator = true) then
            begin
              writeln(f, k:5, COUNT[i].num:8,'   ',COUNT[i].lex);
              inc(k);
              inc(operatorGen, COUNT[i].num);
            end;
        end;

      operatorDict:=k-1;
      writeln(f);
      k:=1;
      i:=0;
      writeln(f, 'Operands:');
      writeln(f, 'num':5,'entries  ':10,' lexem');
      for i:=0 to numLength-1 do
        begin
          if (COUNT[i].isOperator = false) then
            begin
              writeln(f, k:5, COUNT[i].num:8,'   ',COUNT[i].lex);
              inc(k);
              inc(operandGen, COUNT[i].num);
            end;

        end;
      operandDict:=k-1;
      writeln(f);
      writeln(f,'Operators dictionary: ',operatorDict,' - Operators, in total: ',operatorGen);
      writeln(f,'Operands dictionary: ',operandDict,' - Operands, in total: ',operandGen);
      writeln(f,'Program dictionary: ',operatorDict+operandDict);
      writeln(f,'Program length: ',operatorGen+operandGen);
      writeln(f,'Program volume: ', (operatorGen+operandGen)*Round(log2(operatorDict+operandDict)) );
    end;
end.
