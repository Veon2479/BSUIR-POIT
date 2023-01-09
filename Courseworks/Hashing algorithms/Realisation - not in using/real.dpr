program real;

{$APPTYPE CONSOLE}

{$R *.res}

uses
  System.SysUtils,
  engine in '..\RealUnit\engine.pas';

const
  RECS = 1000000;

type
  tKey = String[6];

  tString = String[40];

  tMyField =  record
                ID: tKey;
                field1: integer;
                field2: tString;
              end;

  tAdr = ^tMyField;



  tElAdr = ^tElement;
  tElement = record
      adrNext:  tElAdr;
      field: tMyField;
  end;


function randomStr: tString;
  var
    i, j: integer;
  begin
    Result:='';
    for j := 1 to random(15) do
      begin
        i:=random(1000);
        while i>1 do
          begin
            Result:=Result+chr((i mod 10)+ord(' ')+1+random(69));
            i:=i div 10;
          end;
      end;

        Result:=result+chr(random(94)+33);

  end;

function Key(n: integer): tKey;

  begin
    Result:='';
    while n>0 do
      begin
        Result:=chr((n mod 26)+ord('A'))+Result;
        n:=n div 26;
      end;

  end;



var
  fin, terms: file of tMyField;

  tmpKey: tKey;
  i: integer;
  tmpField: tMyField;





begin
  assignFile(fin, 'source.txt');
  assignFile(terms, 'terms.txt');
  reset(fin);
  rewrite(terms);

  {for i := 1 to RECS do
    begin
            with tmpField do
              begin
                ID:=key(i);
                field1:=random(200000);
                field2:=randomStr;
                writeln(i:10,ID:7, field1:7, field2:41);
              end;
      write(fin, tmpField);
    end; }

  for i := 1 to RECS do
      begin
        read(fin, tmpField);
        write(terms, tmpField);
      end;

  for i := RECS+1 to RECS+200 do
    begin
            with tmpField do
              begin
                ID:=key(i);
                field1:=random(200000);
                field2:=randomStr;
                writeln((i-RECS):10,ID:7, field1:7, field2:41);
              end;
      write(terms, tmpField);
    end;


  closeFile(fin);

 readln;
end.   //генераци€ исходного файла

{  tFunc = function(t1, t2: tElAdr): boolean;    //true, если t1 ссылаетс€ эл-т с большим признаком



procedure createElement(var lastElement, newElement: tElAdr);
  begin
    new(newElement);
    lastElement^.adrNext:=newElement;
    newElement^.adrNext:=nil;
  end;

procedure deleteElement(var prevElement: tAdr);
  var
    tmp1, tmp2: tAdr;
  begin
    tmp1:=prevElement^.adrNext;
    tmp2:=tmp1^.adrNext;
    if tmp2<>nil then
        begin
          prevElement^.adrNext:=tmp2;
          dispose(tmp1);
        end
      else
        begin
          dispose(tmp1);
          prevElement^.adrNext:=nil;
        end;


  end;


procedure swapElements(prevAdr1, prevAdr2: tAdr);
  var
    tmp1_1, tmp1_2, tmp2_1, tmp2_2, tmp: tAdr;
  begin


    tmp1_1:=prevAdr1^.adrNext;               //сохранение ссылок на и от дл€ 2 элементов
    tmp1_2:=tmp1_1^.adrNext;

    tmp2_1:=prevAdr2^.adrNext;
    tmp2_2:=tmp2_1^.adrNext;



    if not ((tmp1_1=prevAdr2)or(tmp1_2=prevAdr1)) then
        begin




          prevAdr1^.adrNext:=tmp2_1;  //переброска, если элементы далеко
          prevAdr2^.adrNext:=tmp1_1;

          tmp1_1^.adrNext:=tmp2_2;
          tmp2_1^.adrNext:=tmp1_2;


        end
      else
        begin
                                          //переброска, если элементы р€дом
          prevAdr1^.adrNext:=tmp1_2;
          tmp1_2^.adrNext:=tmp1_1;
          prevAdr2^.adrNext:=tmp2_2;

        end;

  end;

function sravn0(t1, t2: tAdr): boolean;
  begin
    if (t1^.field>t2^.field) then Result:=true
      else Result:=false;
  end;





 procedure sortList(prevFirstAdr, LastAdr: tAdr; func: tFunc);//lastAdr - адресное поле в последнем элементе сортируемого участка
  var
    prevTmp1, prevTmp2, prevMax: tAdr;                                 //что, если мен€ютс€ граничные элементы? последний - всЄ в пор€дке
  begin
        begin                                            //если первый.. тоже всЄ в пор€дке
          prevTmp1:=prevFirstAdr;
          while (prevTmp1^.adrNext^.adrNext^.adrNext<>LastAdr) do
            begin
              prevTmp2:=prevTmp1;
              prevMax:=prevTmp1;
              while (prevTmp2^.adrNext^.adrNext<>LastAdr) do
                begin
                  prevTmp2:=prevTmp2^.adrNext;
                  if func(prevTmp2^.adrNext, prevMax^.adrNext)
                    then
                      begin
                        prevMax:=prevTmp2;


                      end;

                end;
              swapElements(prevTmp1, prevMax);
              prevTmp1:=prevTmp1^.adrNext;
            end;
        end;
  end;

var
  headAdr, tmpAdr1, tmpAdr2: tAdr;
  f: textFile;
  tmp, sum, k, i: integer;

begin
  assignFile(f, 'file.txt');
  reset(f);

  new(headAdr);
  headAdr^.adrNext:=nil;
  tmpAdr1:=headAdr;

  while not EOF(f) do                           //read list from file
    begin
      readln(f, tmp);
      createElement(tmpAdr1, tmpAdr2);
      tmpAdr2^.field:=tmp;
      tmpAdr1:=tmpAdr2;
      tmpAdr2:=nil;
      inc(k);
    end;
  tmpAdr1:=headAdr;


  repeat
    begin                                 //works
      tmpAdr1:=tmpAdr1^.adrNext;
      with tmpAdr1^ do
        begin
          sum:=sum+field;
        end;
    end;
  until tmpAdr1^.adrNext=nil;

  while (tmpAdr1^.adrNext<>nil) do
     begin                              //works too
      tmpAdr1:=tmpAdr1^.adrNext;
      with tmpAdr1^ do
        begin
          sum:=sum+field;
        end;
    end;
  writeln(sum);



  sortList(HeadAdr, nil, sravn0);
  deleteElement(HeadAdr^.adrNext);

  tmpAdr1:=headAdr;
  while (tmpAdr1^.adrNext<>nil) do
     begin
       tmpAdr1:=tmpAdr1^.adrNext;
      writeln(tmpAdr1^.field);

    end;


  closeFile(f);
  readln;
end.
}
