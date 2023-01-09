program Entry;

uses
  Vcl.Forms,
  Main in 'Main.pas' {frmForm},
  engine in '..\RealUnit\engine.pas',
  Curves in 'Curves.pas' {frmCurves},
  Plot in 'Plot.pas' {frmTimePlot};

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TfrmForm, frmForm);
  Application.CreateForm(TfrmCurves, frmCurves);
  Application.CreateForm(TfrmTimePlot, frmTimePlot);
  Application.Run;
end.
