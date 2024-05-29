﻿import DDTWork  

def startDSA():
  TestedApps.Dsa.Run(1)

def stopDSA():
  TestedApps.Dsa.Close()

def test_parameters_sign(excel_sheet, alies, file=False):
  if file:
    res = DDTWork.get_values(excel_sheet, ["p", "q", "h", "x", "k", "file"])
  else:
    res = DDTWork.get_values(excel_sheet, ["p", "q", "h", "x", "k"])
  
  DSA = alies
  frmDSA = DSA.frmDsa
  frmDSA.rbSign.Select()
  tb = frmDSA.tbP
  tb.Text = res["p"] 
  tb = frmDSA.tbQ
  tb.Text = res["q"] 
  tb = frmDSA.tbH
  tb.Text = res["h"]
  tb = frmDSA.tbX
  tb.Text = res["x"]
  tb = frmDSA.tbK
  tb.Text = res["k"]
  frmDSA.gbRadiobtnSign.btnChoice.Click()
  if file:
    filepath = aqFileSystem.ExpandFileName("..\Resources\TestFiles\\"+res["file"])
    test_choose_file(filepath, alies)

def test_parameters_check_sign(excel_sheet, alies, file_s=False):  
  if file_s:
    res = DDTWork.get_values(excel_sheet, ["p", "q", "h", "y", "file_s"])
  else:
    res = DDTWork.get_values(excel_sheet, ["p", "q", "h", "y"])
  
  DSA = alies
  frmDSA = DSA.frmDsa
  frmDSA.rbCheckSign.Select()
  tb = frmDSA.tbP
  tb.Text = res["p"] 
  tb = frmDSA.tbQ
  tb.Text = res["q"] 
  tb = frmDSA.tbH
  tb.Text = res["h"]
  tb = frmDSA.tbY
  tb.Text = res["y"]
  frmDSA.gbRadiobtnSign.btnChoice.Click()
  
  if file_s:
      filepath = aqFileSystem.ExpandFileName("..\Resources\TestFiles\\"+res["file_s"])
      test_choose_file(filepath, alies)
  
def test_dlg_warning(static, static_wnd_caption, alies):
  DSA = alies
  dlg = DSA.dlg_
  code = aqObject.CheckProperty(dlg, "WndCaption", cmpEqual, "Предупреждение")
  
 # NameMapping.​Sys.​Dsa.​dlg_.​btn_.ClickButton()
  Aliases.Dsa.dlg_.Window("Button", "OK", 1).ClickButton()
  
def test_choose_file(filepath, alies):
  dlg = alies.dlg_2
  #dlg.cbx_2.ComboBox.Edit.SetText(filepath)
  Aliases.Dsa.Window("#32770", "Open", 1).Window("ComboBoxEx32", "", 1).Window("ComboBox", "", 1).SetText(filepath)
  #dlg.btn_.ClickButton()
  Aliases.Dsa.Window("#32770", "Open", 1).Window("Button", "&Open", 1).ClickButton()
  
def test_sign_values(sheet_excel, alies):
  DSA = alies
  values_check = DDTWork.get_values(sheet_excel, ["y", "g", "hash", "r", "s"])
  res = aqString.Compare(DSA.frmDsa.tbY.Text, values_check["y"], True)
  if res != 0:
    Log.Error("y is incorrect")
  res = aqString.Compare(DSA.frmDsa.tbG.Text, values_check["g"], True)
  if res != 0:
    Log.Error("g is incorrect")
  res = aqString.Compare(DSA.frmDsa.tbR.Text, values_check["r"], True)
  if res != 0:
    Log.Error("r is incorrect")
  res = aqString.Compare(DSA.frmDsa.tbS.Text, values_check["s"], True)
  if res != 0:
    Log.Error("s is incorrect")
  res = aqString.Compare(DSA.wndDSA.gbSignResult.tbHM.Text, values_check["hash"], True)
  if res != 0:
    Log.Error("H(M) is incorrect")
  
def test_check_sign_values(sheet_excel, alies):
  DSA = alies
  values_check = DDTWork.get_values(sheet_excel, ["g", "hash", "r", "s", "w", "v"]) 
  res = aqString.Compare(DSA.frmDsa.tbG.Text, values_check["g"], True)
  if res != 0:
    Log.Error("g is incorrect")
  res = aqString.Compare(DSA.frmDsa.tbHMC.Text, values_check["hash"], True)
  if res != 0:
    Log.Error("H(M') is incorrect")
  res = aqString.Compare(DSA.frmDsa.tbRC.Text, values_check["r"], True)
  if res != 0:
    Log.Error("r is incorrect")
  res = aqString.Compare(DSA.frmDsa.tbSC.Text, values_check["s"], True)
  if res != 0:
    Log.Error("s is incorrect")
  res = aqString.Compare(DSA.frmDsa.tbW.Text, values_check["w"], True)
  if res != 0:
    Log.Error("w is incorrect")
  res = aqString.Compare(DSA.frmDsa.tbV.Text, values_check["v"], True)
  if res != 0:
    Log.Error("v is incorrect")