object frmTimePlot: TfrmTimePlot
  Left = 0
  Top = 0
  Align = alTop
  BorderStyle = bsToolWindow
  Caption = #1043#1088#1072#1092#1080#1082
  ClientHeight = 372
  ClientWidth = 1324
  Color = clBtnFace
  CustomTitleBar.CaptionAlignment = taCenter
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnPaint = FormPaint
  PixelsPerInch = 96
  TextHeight = 13
  object lbRes: TLabel
    Left = 8
    Top = 341
    Width = 3
    Height = 13
  end
  object Chart1: TChart
    Left = 0
    Top = 0
    Width = 1314
    Height = 342
    SubTitle.Text.Strings = (
      '')
    Title.Text.Strings = (
      #1047#1072#1090#1088#1072#1095#1077#1085#1085#1086#1077' '#1085#1072' '#1087#1086#1080#1089#1082' '#1074#1088#1077#1084#1103)
    BottomAxis.Title.Caption = #1087#1072#1082#1077#1090#1099
    Chart3DPercent = 8
    LeftAxis.Title.Caption = #1084#1080#1083#1083#1080#1089#1077#1082#1091#1085#1076#1099
    TabOrder = 0
    DefaultCanvas = 'TGDIPlusCanvas'
    ColorPaletteIndex = 13
    object Series1: TLineSeries
      Title = #1052#1077#1090#1086#1076' '#1089#1076#1074#1080#1075#1072
      Brush.BackColor = clDefault
      Pointer.InflateMargins = True
      Pointer.Style = psRectangle
      XValues.Name = 'X'
      XValues.Order = loAscending
      YValues.Name = 'Y'
      YValues.Order = loNone
      Data = {
        00190000000000000000587B400000000000B078400000000000887340000000
        0000106D400000000000805B4000000000008041400000000000805640000000
        000080414000000000007062400000000000D06B400000000000E06A40000000
        0000E05F400000000000306B4000000000000074400000000000907040000000
        000010634000000000007062400000000000803B400000000000C05C40000000
        0000D06640000000000048724000000000008071400000000000707240000000
        00004872400000000000607340}
      Detail = {0000000000}
    end
    object Series2: TLineSeries
      Title = #1052#1077#1090#1086#1076' '#1087#1088#1077#1086#1073#1088'. '#1089'.'#1089'.'
      Brush.BackColor = clDefault
      Pointer.InflateMargins = True
      Pointer.Style = psRectangle
      XValues.Name = 'X'
      XValues.Order = loAscending
      YValues.Name = 'Y'
      YValues.Order = loNone
    end
  end
end
