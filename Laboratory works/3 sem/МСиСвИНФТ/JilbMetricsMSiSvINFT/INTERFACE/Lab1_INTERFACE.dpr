program Lab1_INTERFACE;

uses
  Vcl.Forms,
  MainUnit in 'MainUnit.pas' {TMainForm};

{$R *.res}

begin
  Application.Initialize;
  Application.MainFormOnTaskbar := True;
  Application.CreateForm(TTMainForm, TMainForm);
  Application.Run;
end.
