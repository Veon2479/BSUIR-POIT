unit Curves;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, engine, Vcl.StdCtrls, Vcl.ExtCtrls,
  VclTee.TeeGDIPlus, VCLTee.TeEngine, VCLTee.TeeProcs, VCLTee.Chart,
  VCLTee.Series;

type
  TfrmCurves = class(TForm)
    chrtShiftOver: TChart;
    Chart1: TChart;
    Series1: TLineSeries;
    Series2: TLineSeries;
    Series3: TLineSeries;
    Series4: TLineSeries;
    lbOptimal: TLabel;


    procedure FormPaint(Sender: tObject);
   // procedure plotShiftCurves;
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmCurves: TfrmCurves;

implementation

{$R *.dfm}


  procedure setLCaption(var lab: tLabel; const str: string);
    begin
      lab.Caption:=str;
    end;

  procedure TfrmCurves.FormPaint(Sender: tObject);
    var
      count, i, j, delta, tmpDen, tmpOver: int64;
      tmpShift1, tmpNotat1, tmpShift2, tmpNotat2: int64;
      tmp, minS, minN, pacS, pacN, tmpS: int64;
      str1, str2: String;
      fDen, fOver: textFile;
    begin
      assignFile(fDen, PATH_DENSITY);
      reset(fDen);
      assignFile(fOver, PATH_OVER);
      reset(fOver);

      count:=getFilesNumber;

      Series1.Clear;
      Series2.Clear;
      Series3.Clear;
      Series4.Clear;


      pacS:=0;
      pacN:=0;
      minS:=MAXPACS*101;
      minN:=minS;
      //for j := 1 to count do
      while (not EoF(fOver)) and (not EoF(fDen)) do      
        begin
          {getShiftData(j, tmpDen, tmpOver);
          Series1.addXY(i, tmpDen);
          Series3.addXY(i, tmpOver);

          tmp:=(tmpOver+1)*(101-tmpDen);
          if tmp<minS then
            begin
              minS:=tmp;
              pacS:=i;
            end;


         getNotationData(j, tmpDen, tmpOver);
          Series2.addXY(i, tmpDen);
          Series4.addXY(i, tmpOver);

          tmp:=(tmpOver+1)*(101-tmpDen);
          if tmp<minN then
            begin
              minN:=tmp;
              pacN:=i;
            end;                     }

          readln(fDen, i);             //Density
          readln(fDen, tmpShift1);
          readln(fDen, tmpNotat1);
          Series1.addXY(i, tmpShift1);
          Series2.addXY(i, tmpNotat1);

          readln(fOver, i);
          readln(fOver, tmpShift2);         //Overloaded
          readln(fOver, tmpNotat2);
          Series3.addXY(i, tmpShift2);
          Series4.addXY(i, tmpNotat2);

          tmp:=(tmpShift2+1)*(101-tmpShift1);
          if tmp<minS then
            begin
              minS:=tmp;
              pacS:=i;
            end;

          tmp:=(tmpNotat2+1)*(101-tmpNotat1);
          if tmp<minN then
            begin
              minN:=tmp;
              pacN:=i;
            end;


        end;
      str(pacS, str1);
      str(pacN, str2);
      setLCaption(lbOptimal, ' Оптимальное количество пакетов для метода сдвига: '+str1+', для метода преобразования с.с.: '+str2);
      closeFile(fDen);
      closeFile(fOver);
    end;

end.
