unit Plot;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, VclTee.TeeGDIPlus, VCLTee.TeEngine,
  Vcl.ExtCtrls, VCLTee.TeeProcs, VCLTee.Chart, VCLTee.Series,
  Vcl.StdCtrls, engine;

type
  TfrmTimePlot = class(TForm)
    Chart1: TChart;
    Series1: TLineSeries;
    Series2: TLineSeries;
    lbRes: TLabel;
    procedure FormPaint(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmTimePlot: TfrmTimePlot;

implementation

{$R *.dfm}

  procedure setLCaption(var lab: tLabel; const str: string);
    begin
      lab.Caption:=str;
    end;

  procedure TfrmTimePlot.FormPaint(Sender: TObject);
  var
    fTime: textFile;
    i, tmpSHift, tmpNotat: cardinal;
    minSi, minNi, minShift, minNotat: cardinal;
    str1, str2: tString;
  begin
    assignFile(fTime, PATH_TIME);
    reset(fTime);
    Series1.Clear;
    Series2.Clear;
    minSi:=0;
    minNi:=0;
    //minShift:=10000000;
    //minNotat:=minShift;
    while not EoF(fTime) do
      begin
        readln(fTime, i);
        readln(fTime, tmpShift);
        readln(fTime, tmpNotat);
        if i=20 then
          begin
            minShift:=tmpShift;
            minNotat:=tmpNotat;
          end;
        Series1.addXY(i, tmpShift);
        Series2.addXY(i, tmpNotat);
        if tmpShift<minSHift then
          begin
            minSi:=i;
            minShift:=tmpShift;
          end;
        if tmpNotat<minNotat then
          begin
            minNi:=i;
            minNotat:=tmpNotat;
          end;
      end;
    str(minSi, str1);
    str(minNi, str2);
    setLCaption(lbRes, ' Оптимальное количество пакетов для метода сдвига: '+str1+', для метода преобразования с.с.: '+str2);
    closeFile(fTime);
  end;

end.
