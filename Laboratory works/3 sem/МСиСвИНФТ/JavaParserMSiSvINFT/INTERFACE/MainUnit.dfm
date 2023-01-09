object TMainForm: TTMainForm
  Left = 0
  Top = 0
  Caption = 'TMainForm'
  ClientHeight = 482
  ClientWidth = 1615
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  KeyPreview = True
  OldCreateOrder = False
  OnCreate = Created
  DesignSize = (
    1615
    482)
  PixelsPerInch = 96
  TextHeight = 13
  object CodeInput: TMemo
    Left = 0
    Top = 48
    Width = 458
    Height = 426
    Anchors = [akLeft, akTop, akBottom]
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -13
    Font.Name = 'Tahoma'
    Font.Style = []
    Lines.Strings = (
      '')
    ParentFont = False
    ScrollBars = ssVertical
    TabOrder = 0
    OnKeyDown = KeyDown
    OnKeyPress = KeyPressed
  end
  object ResultGrid: TStringGrid
    Left = 464
    Top = 0
    Width = 1143
    Height = 474
    Anchors = [akLeft, akTop, akRight, akBottom]
    ColCount = 6
    DefaultColWidth = 128
    FixedRows = 2
    TabOrder = 1
  end
  object BOpenFile: TButton
    Left = 0
    Top = 0
    Width = 226
    Height = 42
    Caption = 'Load code file'
    TabOrder = 2
    OnClick = BOpenFileClick
  end
  object BCount: TButton
    Left = 232
    Top = 0
    Width = 226
    Height = 42
    Caption = 'Count result'
    TabOrder = 3
    OnClick = BCountClick
  end
  object LoadCodeFromFile: TOpenTextFileDialog
    Options = [ofHideReadOnly, ofFileMustExist, ofEnableSizing]
    Left = 1016
    Top = 24
  end
end
