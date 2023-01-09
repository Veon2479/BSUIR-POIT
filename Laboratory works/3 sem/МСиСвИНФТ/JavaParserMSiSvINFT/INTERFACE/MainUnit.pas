unit MainUnit;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, Vcl.StdCtrls, Vcl.ExtCtrls, Vcl.ExtDlgs,
  Vcl.Grids, CodeParser,
  Math,
  System.ioutils,
  types,
  ShellApi,
  ParseAnalys,
  customTypes;


type
  TTMainForm = class(TForm)
    CodeInput: TMemo;
    LoadCodeFromFile: TOpenTextFileDialog;
    ResultGrid: TStringGrid;
    BOpenFile: TButton;
    BCount: TButton;
    procedure KeyPressed(Sender: TObject; var Key: Char);
    procedure KeyDown(Sender: TObject; var Key: Word; Shift: TShiftState);
    procedure Created(Sender: TObject);
    procedure BOpenFileClick(Sender: TObject);
    procedure BCountClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  TMainForm: TTMainForm;
  var filename: string;
  lexems          : TArray;

    fileIn, fileOut : TextFile;

    count           : tCountAr;

implementation

{$R *.dfm}

procedure TTMainForm.BOpenFileClick(Sender: TObject);
begin
    // CLICK
    LoadCodeFromFile.Execute();
    // get name clicked
    with LoadCodeFromFile.Files do
        filename:= LoadCodeFromFile.Files.Strings[count - 1];


    CodeInput.Lines.LoadFromFile(filename);

end;

procedure TTMainForm.BCountClick(Sender: TObject);
var i, j, tmp1, tmp2, maxLen: integer;
begin

    // clear old
    with ResultGrid do
        for i := 2 to RowCount do
            for j := 0 to ColCount do
                Cells[j, i]:= '';


    // load file
    CodeInput.Lines.SaveToFile('cache.txt');

    AssignFile(fileIn, 'cache.txt', CP_UTF8);   // open file

    reset(fileIn);
    anCode(fileIn, lexems, nLexems);
    closeFile(fileIn);

    // create output
    AssignFile(fileOut, filename + '_out' + '.txt', CP_UTF8);   // open file
    rewrite(fileOut);
    for i := 0 to nLexems do
        writeln(fileOut, lexems[i]);
    closeFile(fileOut);


    countLex(count, lexems);

    // form output to file
    assignFile(fileOut, filename + '_out_count' + '.txt', CP_UTF8);
    rewrite(fileOut);
    formOut(count, fileOut);
    closeFile(fileOut);

    // form output to table
    tmp1:= 0; tmp2 := 0;
    for I := 0 to numLength - 1 do
        if count[i].isOperator then
            inc(tmp1)
        else
            inc(tmp2);

    maxLen:= max(tmp1, tmp2);
    ResultGrid.RowCount:=  maxLen + 5;

    tmp1:= 2; tmp2 := 2;  j:= -1; // num of lines
    with ResultGrid do
    begin
        // count & write output
        for I := 0 to numLength - 1 do
        begin
            if i < maxLen then
                Cells[0, i + 2]:= inttostr(i);
            if count[i].isOperator then
            begin
                Cells[1, tmp1]:= inttostr(count[i].num);
                Cells[2, tmp1]:= count[i].lex;
                inc(tmp1);
            end
            else
            begin
                Cells[3, tmp2]:= inttostr(count[i].num);
                Cells[4, tmp2]:= count[i].lex;
                inc(tmp2);
            end
        end;
        i:= maxLen + 2;
        Cells[1, i    ]:= 'Operators dictionary';
        Cells[1, i + 1]:= inttostr(operatorDict);
        Cells[2, i    ]:= 'Operators in total';
        Cells[2, i + 1]:= inttostr(operatorGen);
        Cells[3, i    ]:= 'Operands dictionary';
        Cells[3, i + 1]:= inttostr(operandDict);
        Cells[4, i    ]:= 'Operands in total';
        Cells[4, i + 1]:= inttostr(operandGen);

        Cells[1, i + 2]:= 'Program length';
        Cells[2, i + 2]:= inttostr(operatorGen+operandGen);
        Cells[3, i + 2]:= 'Program volume';
        if (operatorDict+operandDict) = 0 then
            Cells[4, i + 2]:= 'INVALID'
        else
            Cells[4, i + 2]:= inttostr((operatorGen+operandGen)*Round(log2(operatorDict+operandDict)));
    end;
end;

procedure TTMainForm.Created(Sender: TObject);
var i, j : integer;
begin
    // setup file input filtrer
    LoadCodeFromFile.Filter := 'Java proj files (*.java)|*.java|Txt files (*.txt)|*.txt|All files (*.*)|*.*';
    // setup headers
    with ResultGrid do
    begin
        Cells[1, 0]:='OPERATORS';
        Cells[0, 1]:='num';
        Cells[1, 1]:='entries';
        Cells[2, 1]:='lexems';
        Cells[3, 1]:='entries';
        Cells[4, 1]:='lexems';
        Cells[3, 0]:='OPERANDS';
    end;
end;

end.
