unit engine;

interface

  const
    RECS = 1000000;
    MAXPACS = 200000;
    {RECS = 100000;
    MAXPACS = 20000; }
    MAXRECS = trunc(1.2*RECS);

    PATH_TIME = 'time.txt';
    PATH_TERMS = 'terms.txt';
    PATH_SOURCE = 'source.txt';
    PATH_DENSITY = 'Density.txt';
    PATH_OVER = 'Over.txt';
    PATH_COUNT = 'Out\count.txt';

  type
    tKey = String[6];

    tString = String[40];

    tMyField =  record
                  ID: tKey;
                  field1: integer;
                  field2: tString;
                end;

    tAdr = ^tMyField;

    tFile = file of tMyField;

    tElAdr = ^tElement;
    tElement =  record
                  adrNext:  tElAdr;
                  fieldAdr: tAdr;
                end;

    procedure Initialize;
  //  procedure readFile(var Head: tElAdr; const PATH: tString);
    procedure findPlaces;
    function getFilesNumber: int64;
   { procedure getShiftData(const FILENUM: int64; var DENSITY, OVERLOADED: int64);
    procedure getNotationData(const FILENUM: int64; var DENSITY, OVERLOADED: int64); }

    procedure searching;
  function testRec(const TERM: tSTring; var res: tAdr): boolean;



implementation

  uses Windows;



  type
    tArr = array of array of tAdr;
    tMethod = function(const numKey: int64; const packCount: int64): int64;          //число с масштабированием
  var
    nullF: tAdr;
    Matrix, MatrixSearch: tArr;
    Head, TermsHead: tElAdr;

  procedure createElement(var lastElement, newElement: tElAdr);
    begin
      new(newElement);
      lastElement^.adrNext:=newElement;
      newElement^.adrNext:=nil;
    end;



  procedure readFile(var Head: tElAdr; const PATH: tString);
    var
      tmp1, tmp2: tElAdr;
      tmp: tMyField;
      fAdr: tAdr;
      f: tFile;
    begin
      assignFile(f, PATH);
      reset(f);


      tmp1:=Head;

      while not EOF(f) do                           //read list from file
        begin
          fAdr:=nil;
          createElement(tmp1, tmp2);
          new(fAdr);
          read(f, fAdr^);
          tmp2^.fieldAdr:=fAdr;
          tmp1:=tmp2;
          tmp2:=nil;
        end;

      closeFile(f);
    end;


  procedure deleteList(var HEAD: tElAdr);
    var
      tmp1, tmp2: tElAdr;
    begin
      while HEAD^.adrNext<>nil do
        begin
          tmp1:=HEAD;
          tmp2:=HEAD^.adrNext;
          while tmp2^.adrNext<>nil do
            begin
              tmp1:=tmp2;
              tmp2:=tmp1^.adrNext;
            end;
          dispose(tmp2^.fieldAdr);
          dispose(tmp2);
          tmp1^.adrNext:=nil;
        end;
    end;

  procedure resetMatrix(var MATRIX: tArr; const A, B: int64; const NULLF: tAdr);
    var
      i, j: int64;
    begin
      setLength(Matrix, a);
      for i := 0 to a-1 do
        begin
          setLength(Matrix[i], b);
          for j := 0 to b-1 do
            Matrix[i, j]:=nullF;
        end;
    end;



  function addField(const TMP: tAdr; var MATRIX: tArr; const PACKAGE: int64): boolean;
    var
      i: int64;
      fl: boolean;
    begin
      i:=(length(MATRIX[PACKAGE])-1);
      if (MATRIX[PACKAGE, i ]^.ID='') then
          begin
            fl:=true;
            Result:=true;
          end
        else
          begin
            fl:=false;
            result:=false;
          end;
      i:=i div 2;
      if (MATRIX[PACKAGE, i ]^.ID='') then i:=0;

      while fl do
        begin
          if (MATRIX[PACKAGE, i]^.ID='') then
            begin
              MATRIX[PACKAGE, i]:=TMP;
              fl:=false;   //сюда мы зашли только если в пакете есть место
            end;
          inc(i);
        end;

    end;


  function cypNumber(N: int64): int64;
    begin
      Result:=0;
      while N>0 do
        begin
          inc(Result);
          N:=N div 10;
        end;
    end;

  function powZ(const BASE, COUNT: int64): int64;
    var
      i: int64;
    begin
      Result:=1;
      for i := 1 to COUNT do
        Result:=Result*BASE;
    end;

  function shiftFunc(const numKey: int64; const PACKCOUNT: int64): int64;          //число с масштабированием
    var
      tmp: int64;
      t, k, d: int64;
    begin


      tmp:=NUMKEY;
      Result:=0;
      d:=cypNumber(PACKCOUNT);
      k:=cypNumber(tmp);
      while k>d do                 //  nk=12345
        begin                                                         //   k=5 k=2 d=100
                                               //  nk=(123)+(45)=168       nk=24
          k:=k div 2;
          t:=powZ(10, k);
          tmp:=(tmp div t)+(tmp mod t);
          k:=cypNumber(tmp);
        end;                                                                         //[0;a] |-> [0;b], a=99..9, b=packCount-1

     Result:=tmp mod PACKCOUNT;
    end;

  function notationFunc(const numKey: int64; const packCount: int64): int64;      //число с масштабированием
    var
      tmp: int64;
      i, curDig, d: int64;
    begin
      tmp:=NUMKEY;
      Result:=0;
      curDig:=1;
      d:=PACKCOUNT-1;
      for i := 1 to d do
        begin
          Result:=Result+(tmp mod 10)*curDig;
          curDig:=curDig*37;                   //17-247'50 (2)| 23-1793'610 (3)| 29-4026'5316 (4)| 35-457'73670(5)               or (packCount)?
          tmp:=tmp div 10;
        end;
     // write(f, numKey:10, packCount:6, d:6, Result:20);                                   //0?
     // Result:=((Result mod powZ(10, d))*(packCount-1))div(powZ(10, d+1)-1);         //[0;a] |-> [0;b], a=99..9, b=packCount-1
     // Result:=Round( ( (Result)*(packCount-1) )/(powZ(10, cypNumber(Result))-1) );
    //  writeln(f, Result:6);
      Result:=Result mod PACKCOUNT;
    end;


  function convert(strID: tKey): int64;
    var
      i: int64;
    begin

      Result:=0;
      for i := 1 to length(strID) do
         Result:=Result*100+ord(strID[i])-50;

    end;

  function Place(Adr: tAdr; var MATRIX: tArr; func: tMethod): boolean;     // в переполнении или нет
    var
      num: int64;
      fl: boolean;
    begin

      num:=func(convert(Adr^.ID), length(MATRIX));


      fl:=false;
      Result:=True;
      repeat
        fl:=addField(Adr, MATRIX, num);



        if not fl then
            begin

              inc(num);                                //проверять зацикленность (прошли ли мы этот num) нету смысла, т.к. мест для записей гарантировано больше
              if (num=length(MATRIX)) then num:=0;
              Result:=false;
            end
      until fl;

    end;



  procedure writeMatrix(const MATRIX: tArr; var f: tFile);
    var
      i, j: int64;
    begin
      for i := 0 to length(MATRIX)-1 do
        begin
          for j := 0 to length(MATRIX[i])-1 do
            write(f, MATRIX[i, j]^);
        end;
    end;

  function findDensity(const MATRIX: tArr; const NUM: int64): real;
    var
      i, len: int64;
    begin
      if MATRIX[NUM, 0]^.ID='' then Result:=0
        else
          begin
            i:=1;
            len:=length(MATRIX[NUM]);
            while  (i<len-1)and(MATRIX[NUM, i].ID<>'') do
              inc(i);
            Result:=(1+i)/len;
          end;
      {Result:=0;
      len:=Length(MATRIX[NUM]);
      i:=0;
      while  (i<len-1)and(MATRIX[NUM, i].ID<>'*') do
        inc(i);
      Result:=i / len;}
    end;

  function minDensity(const MATRIX: tArr): int64;
    var
      i: int64;
      tmp, min: real;
    begin
      Result:=0;
      min:=100;
      for i := 0 to length(MATRIX)-1 do
        begin
          tmp:=findDensity(MATRIX, i);
          if tmp<min then min:=tmp;
        end;
       Result:=round(100*(min));
    end;

  function averageOverload(const OVERLOADED: int64): real;
    begin
      Result:=(OVERLOADED*100)/ RECS;                  //simply result?
    end;

  procedure compute(var MATRIX: tArr; const PATH: tString; const HEAD: tElAdr;  func: tMethod; var DENS, OVER: int64);
    var
      isNotOver: boolean;
      tmpAdr: tElAdr;
      overLoaded: int64;
      i: int64;
      f: tFile;
    begin
      assignFile(f, PATH);
      rewrite(f);
      tmpAdr:=HEAD;
      overLoaded:=0;

      for i := 1 to RECS do
        begin
                 //   writeln(i);
          tmpAdr:=tmpAdr^.adrNext;
          isNotOver:=Place(tmpAdr^.fieldAdr, Matrix, func);
          if not isNotOver then inc(overLoaded);
        end;
      DENS:=minDensity(MATRIX);
      OVER:=overLoaded;



      writeMatrix(Matrix, f);
      closeFile(f);
    end;


  procedure writeLog(const c1, c2, c3: int64; const PATH: tSTring);
    var
      f: textFile;
    begin
      assignFile(f, PATH);
      append(f);
      writeln(f, c1);
      writeln(f, c2);
      writeln(f, c3);
      closeFile(f);
    end;

  procedure disposeMatrix(var MATRIX: tArr);
    var
      i, j: integer;
    begin
      for i := 0 to length(MATRIX)-1 do
        for j := 0 to length(MATRIX[i])-1 do
            if (MATRIX[i, j]^.ID<>'') then dispose(MATRIX[i, j]);
    end;

  procedure findPlaces;
    var
      i, j, k, delta: int64;
      strNum: String;
      isCont: boolean;
      tmpDens1, tmpOver1, tmpDens2, tmpOver2: int64;
      fout: textFile;
    begin
      readFile(Head, PATH_SOURCE);

      assignFile(fout,PATH_DENSITY);
      rewrite(fout);
      closeFile(fout);
      assignFile(fout,PATH_OVER);
      rewrite(fout);
      closeFile(fout);

      k:=0;
      delta:=20;                   //2 4 6 8 10 12 14 16 18 20      //200
      i:=20;                      //1 2 3 4 5  6  7  8  9  10
      strNum:='';

      assignFile(fout, PATH_COUNT);
      rewrite(fOut);
      writeln(fout, 0);

      while (i<=MAXPACS) do                                                 // 20 40 60 80 100 120 140 160 180 200 400 600 800 1000 1200
        begin
           inc(k);                                                           //  1  2 3  4   5   6   7   8   9   10   11 12  13  14   15
          j:=MAXRECS div i;

          str(k, strNum);

          resetMatrix(Matrix, i, j, nullF);



          compute(Matrix,'Out\shift\'+strNum+'.txt', Head, shiftFunc, tmpDens1, tmpOver1);


          resetMatrix(Matrix, i, j, nullF);
          compute(Matrix,'Out\notation\'+strNum+'.txt', Head, notationFunc,  tmpDens2, tmpOver2);

          writeLog(i, tmpDens1, tmpDens2, PATH_DENSITY);
          writeLog(i, tmpOver1, tmpOver2, PATH_OVER);

          rewrite(fout);
          writeln(fout, k);


          inc(i, delta);
          if i=delta*10 then
            delta:=i;
        end;
      deleteList(Head);
      resetMatrix(Matrix, 0, 0, nullF);
      //disposeMatrix(Matrix);                 //очищение списка
      //Head^.adrNext:=nil;

      closeFile(fout);
    end;

  function getFilesNumber: int64;
    var
      f: textFile;
      code: int64;
      strNum: tSTring;
    begin
      assignFile(f, PATH_COUNT);
      reset(f);
      readln(f, Result);
      closeFile(f);
    end;

  {procedure getShiftData(const FILENUM: int64; var DENSITY, OVERLOADED: int64);
    var
      f: textFile;
      strNum: tString;
      code: integer;
    begin
      str(FILENUM, strNum);
      assignFile(f,'Out\shift\'+strNum+'.txt');
      reset(f);
      readln(f, strNum);
      val(strNum, DENSITY, code);
      readln(f, strNum);
      val(strNum, OVERLOADED, code);
      closeFile(f);
    end;

  procedure getNotationData(const FILENUM: int64; var DENSITY,  OVERLOADED: int64);
    var
      f: textFile;
      strNum: tString;
      code: integer;
    begin
      str(FILENUM, strNum);
      assignFile(f,'Out\notation\'+strNum+'.txt');
      reset(f);
      readln(f, strNum);
      val(strNum, DENSITY, code);
      readln(f, strNum);
      val(strNum, OVERLOADED, code);
      closeFile(f);
    end;   }


  {function position(const TERM, STR: String): int64;
    var
      i, j, len, len0: int64;
      f1: boolean;

    begin
      f1:=true;
      i:=1;
      len:=length(STR);
      len0:=length(TERM);
      RESULT:=0;
      while f1 do
        begin
          if STR[i]=' ' then
            begin
              j:=i;
              while STR[j]<>':' do
                inc(j);
              if (j-i+1)>len0 then f1:=false
                else
                  begin
                    if TERM=copy(STR, i, j-i+1) then RESULT:=i;
                  end;
            end;


          inc(i);
          if i>len-len0 then f1:=false;
        end;
    end;  }



  procedure loadFile(var MATRIX: tArr; const PACS: integer; const PATH: tString);
    var
      f: tFile;
      i, j, count: integer;
      tmp: tMyField;
    begin
      assignFile(f, PATH);
      reset(f);
      count:= MAXRECS div PACS;
      resetMatrix(MATRIX, PACS, count, NULLF);
      //writeln(PACS, ' ', count);
      for i := 0 to PACS-1 do
        for j := 0 to count-1 do
          begin
            read(f, tmp);
            if tmp.ID<>'' then
                begin
                  MATRIX[i,j]:=nil;
                  new(MATRIX[i,j]);
                  MATRIX[i,j]^:=tmp;
                end;

          end;

      closeFile(f);
    end;

  function findRec(const MATRIX: tArr; const TERM: tKey; hash: tMethod; var rez: tAdr): boolean;
  var
    a, b, j: int64;
    pac, pac0: int64;
    fl: boolean;
  begin
    a:=length(MATRIX);
    b:=length(MATRIX[0]);
    pac:=hash( convert(term), a );
    pac0:=pac;
    RESULT:=false;
    fl:=true;
    while fl do
      begin

       j:=0;
       while (j<b)and(not RESULT) do
        begin
          if (MATRIX[pac, j]^.ID=TERM) then
            begin
              RESULT:=true;
              fl:=false;
              RESULT:=true;
              REZ:=MATRIX[pac, j];
            end;
          inc(j);
        end;

       inc(pac);
       if pac=a then
           pac:=0;
       if pac=pac0 then
           fl:=false;

      end;

  end;


  function searchRecs(var MATRIX: tArr; hash: tMethod): cardinal;
    var
      t1, t2: cardinal;
      tmpAdr: tElAdr;
      fl: boolean;
      res, rez: tAdr;

    begin

      tmpAdr:=TermsHead;

      t1:=getTickCount;

      while (tmpAdr^.adrNext<>nil) do
        begin

          tmpAdr:=tmpAdr^.adrNext;

          fl:=findRec(MATRIX, tmpAdr^.fieldAdr^.ID, hash, rez);
          //writeln(tmpAdr^.fieldAdr^.ID);
        end;

      t2:=getTickCount;
      RESULT:=t2-t1;
    end;

  procedure searching;
    var
      i, j, count, pacs: int64;
      k: integer;
      fs, res: textFile;
      strNum: tSTring;
      time1, time2: cardinal;
      ft: tFile;
    begin
      assignFile(res, PATH_TIME);
      rewrite(res);
      closeFile(res);

      readFile(termsHead, PATH_TERMS);


      count:=getFilesNumber;
      //count:=37;

      k:=1;
      pacs:=20;
      j:=20;

    //  disposeMatrix(MatrixSearch);

      for k := 1 to count do
        begin
          //  writeln('Ring ',k);
          str(k, strNum);


          loadFile(MatrixSearch, pacs, 'Out\shift\'+strNum+'.txt');
          time1:=searchRecs(MatrixSearch, shiftFunc);
          disposeMatrix(MatrixSearch);

          loadFile(MatrixSearch, pacs, 'Out\notation\'+strNum+'.txt');
          time2:=searchRecs(MatrixSearch, notationFunc);
          disposeMatrix(MatrixSearch);

          writeLog(pacs, time1, time2, PATH_TIME);



          inc(pacs, j);
          if pacs=j*10 then
            j:=pacs;

        end;

      deleteList(termsHead);


    end;



  function testRec(const TERM: tSTring; var RES: tAdr): boolean;
    begin
      loadFile(Matrix, 20, 'Out\notation\1.txt');

      Result:=findRec(Matrix, TERM, notationFunc, RES);

    end;


  procedure Initialize;
    begin
     new(nullF);
      with nullF^ do
        begin
          ID:='';
          field1:=0;
          field2:='';
        end;


      new(Head);
      Head^.adrNext:=nil;

      new(termsHead);
      termsHead^.adrNext:=nil;



    end;

end.


