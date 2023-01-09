object frmForm: TfrmForm
  Left = 0
  Top = 0
  BorderStyle = bsToolWindow
  Caption = #1040#1085#1072#1083#1080#1079
  ClientHeight = 279
  ClientWidth = 329
  Color = clBtnFace
  CustomTitleBar.CaptionAlignment = taCenter
  Font.Charset = RUSSIAN_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  OldCreateOrder = False
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object btnCompute: TButton
    Left = 8
    Top = 8
    Width = 313
    Height = 25
    Caption = #1053#1072#1095#1072#1090#1100' '#1088#1072#1079#1084#1077#1097#1077#1085#1080#1103' '#1079#1072#1087#1080#1089#1077#1081
    ParentShowHint = False
    ShowHint = False
    TabOrder = 0
    OnClick = btnComputeClick
  end
  object btnFind: TButton
    Left = 8
    Top = 128
    Width = 313
    Height = 25
    Caption = #1055#1086#1080#1089#1082' '#1079#1072#1087#1080#1089#1077#1081
    TabOrder = 1
    OnClick = btnFindClick
  end
  object btnPlot: TButton
    Left = 8
    Top = 64
    Width = 313
    Height = 25
    Caption = #1055#1086#1089#1090#1088#1086#1080#1090#1100' '#1086#1087#1080#1089#1099#1074#1072#1102#1097#1080#1077' '#1088#1072#1079#1084#1077#1097#1077#1085#1080#1103'  '#1075#1088#1072#1092#1080#1082#1080
    TabOrder = 2
    OnClick = btnPlotClick
  end
  object btnTimePlot: TButton
    Left = 8
    Top = 184
    Width = 313
    Height = 25
    Caption = #1055#1086#1089#1090#1088#1086#1080#1090#1100' '#1086#1087#1080#1089#1099#1074#1072#1102#1097#1080#1077' '#1087#1086#1080#1089#1082'  '#1075#1088#1072#1092#1080#1082#1080
    Font.Charset = RUSSIAN_CHARSET
    Font.Color = clWindowText
    Font.Height = -11
    Font.Name = 'Tahoma'
    Font.Style = []
    ParentFont = False
    TabOrder = 3
    OnClick = btnTimePlotClick
  end
  object test: TButton
    Left = 88
    Top = 232
    Width = 75
    Height = 25
    Caption = 'test'
    TabOrder = 4
    OnClick = testClick
  end
end
