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
    BOpenFile: TButton;
    BCount: TButton;
    ResultGrid: TStringGrid;
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
    absDiff, relDiff, mHeight: integer;

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

    // anCode -- get all lexems
    //  lexems  -- dyn array of str with lexems
    //  nLexems -- amount of lexems
    reset(fileIn);
    anCode(fileIn, lexems, nLexems);
    closeFile(fileIn);

    if (nLexems < 1) then
        exit;

    // create output

    // absDiff -- абс сложность
    // relDiff -- отн сложность
    // mHeight -- макс вложенность
    absDiff:= 0; relDiff := 0; mHeight:= 0;
    countLex(absDiff, relDiff, mHeight, lexems, nLexems);
    //TempRes
    //writeln (' absDiff ', absDiff);
    //writeln (' relDiff ', relDiff);
    //writeln (' height  ', mHeight);
    with ResultGrid do
    begin
        Cells[1, 1] := IntToStr(absDiff);
        Cells[2, 1] := IntToStr(relDiff) + '%';
        Cells[3, 1] := IntToStr(mHeight);
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
        Cells[0, 0] := 'RESULTS';
        Cells[1, 0] := 'absDiff';
        Cells[2, 0] := 'relDiff';
        Cells[3, 0] := 'mHeight';
    end;
end;

end.
