unit Main;

interface

uses
  Winapi.Windows, Winapi.Messages, System.SysUtils, System.Variants, System.Classes, Vcl.Graphics,
  Vcl.Controls, Vcl.Forms, Vcl.Dialogs, engine, Vcl.StdCtrls, Vcl.Grids;

type
  TfrmForm = class(TForm)
    btnCompute: TButton;
    btnFind: TButton;
    btnPlot: TButton;
    btnTimePlot: TButton;
    test: TButton;
    procedure btnComputeClick(Sender: TObject);



    procedure btnPlotClick(Sender: TObject);
    procedure btnFindClick(Sender: TObject);
    procedure FormCreate(Sender: TObject);
    procedure btnTimePlotClick(Sender: TObject);
    procedure testClick(Sender: TObject);


  private
    { Private declarations }
  public
    { Public declarations }
  end;

var
  frmForm: TfrmForm;


implementation

  uses Curves, Plot;



{$R *.dfm}

  procedure TfrmForm.FormCreate(Sender: TObject);
    begin
     Initialize;
    end;

  procedure TfrmForm.btnPlotClick(Sender: TObject);
    begin
      frmCurves.Show;
    end;


  procedure TfrmForm.btnComputeClick(Sender: TObject);
    begin
      showMessage('Пожалуйста, подождите!');
      findPlaces;
    end;

  procedure TfrmForm.btnFindClick(Sender: TObject);
    begin
      showMessage('Пожалуйста, подождите!');
      Searching;
    end;

  procedure TfrmForm.btnTimePlotClick(Sender: TObject);
    begin
      frmTimePlot.Show;
    end;


procedure TfrmForm.testClick(Sender: TObject);
var
  tmp: tAdr;
  term: tSTring;
  fl: boolean;
begin
  term:='CEXHO';
  fl:=testRec('CEXHO', tmp);



    if fl then
    begin
      with tmp^ do
        showMessage(ID+' - '+field2);
    end
    else showMessage('no such record');
end;

end.
