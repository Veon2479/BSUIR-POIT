object frmCurves: TfrmCurves
  Left = 0
  Top = 0
  Align = alTop
  BorderStyle = bsToolWindow
  Caption = #1043#1088#1072#1092#1080#1082#1080
  ClientHeight = 705
  ClientWidth = 1329
  Color = clBtnFace
  CustomTitleBar.CaptionAlignment = taCenter
  ParentFont = True
  OldCreateOrder = False
  OnPaint = FormPaint
  PixelsPerInch = 96
  TextHeight = 13
  object lbOptimal: TLabel
    Left = 0
    Top = 688
    Width = 3
    Height = 13
  end
  object chrtShiftOver: TChart
    Left = 0
    Top = 336
    Width = 1329
    Height = 346
    Title.Text.Strings = (
      #1047#1072#1087#1080#1089#1080' '#1087#1086#1087#1072#1074#1096#1080#1077' '#1074' '#1086#1073#1083#1072#1089#1090#1100' '#1087#1077#1088#1077#1087#1086#1083#1085#1077#1085#1080#1103)
    BottomAxis.ExactDateTime = False
    BottomAxis.LabelsSeparation = 0
    BottomAxis.Title.Caption = #1055#1072#1082#1077#1090#1099
    Chart3DPercent = 6
    LeftAxis.Title.Caption = #1047#1072#1087#1080#1089#1080
    TabOrder = 0
    DefaultCanvas = 'TGDIPlusCanvas'
    ColorPaletteIndex = 13
    object Series3: TLineSeries
      Title = #1084#1077#1090#1086#1076' '#1089#1084#1077#1097#1077#1085#1080#1103
      Brush.BackColor = clDefault
      Pointer.InflateMargins = True
      Pointer.Style = psRectangle
      XValues.Name = 'X'
      XValues.Order = loAscending
      YValues.Name = 'Y'
      YValues.Order = loNone
    end
    object Series4: TLineSeries
      Title = #1084#1077#1090#1086#1076' '#1087#1088#1077#1086#1073#1088'. '#1089'.'#1089'.'
      Brush.BackColor = clDefault
      Pointer.InflateMargins = True
      Pointer.Style = psRectangle
      XValues.Name = 'X'
      XValues.Order = loAscending
      YValues.Name = 'Y'
      YValues.Order = loNone
    end
  end
  object Chart1: TChart
    Left = 0
    Top = -10
    Width = 1329
    Height = 340
    Title.Text.Strings = (
      #1053#1072#1080#1084#1077#1085#1100#1096#1072#1103' '#1087#1083#1086#1090#1085#1086#1089#1090#1100' '#1088#1072#1079#1084#1077#1097#1077#1085#1080#1103' '#1087#1072#1082#1077#1090#1072)
    BottomAxis.Title.Caption = #1055#1072#1082#1077#1090#1099
    Chart3DPercent = 5
    LeftAxis.Increment = 5.000000000000000000
    LeftAxis.Title.Caption = #1055#1083#1086#1090#1085#1086#1089#1090#1100', %'
    TabOrder = 1
    DefaultCanvas = 'TGDIPlusCanvas'
    ColorPaletteIndex = 13
    object Series1: TLineSeries
      Title = #1052#1077#1090#1086#1076' '#1089#1084#1077#1097#1077#1085#1080#1103
      Brush.BackColor = clDefault
      Pointer.InflateMargins = True
      Pointer.Style = psRectangle
      XValues.Name = 'X'
      XValues.Order = loAscending
      YValues.Name = 'Y'
      YValues.Order = loNone
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
