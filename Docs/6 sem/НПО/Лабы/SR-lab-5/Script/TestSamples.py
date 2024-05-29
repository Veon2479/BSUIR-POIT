﻿import DDTWork  

def startTI_4():
  TestedApps.TI_4.Run(1)

def stopTI_4():
  TestedApps.TI_4.Close()

def test_parameters_sign(excel_sheet, alies, file=False):
  if file:
    res = DDTWork.get_values(excel_sheet, ["p", "q", "h", "x", "k", "file"])
  else:
    res = DDTWork.get_values(excel_sheet, ["p", "q", "h", "x", "k"])
  
  TI_4 = alies
  
  window = getWindow()
  window.
  
  frmTI_4 = TI_4.frmTI_4
  frmTI_4.rbSign.Select()
  tb = frmTI_4.tbP
  tb.Text = res["p"] 
  tb = frmTI_4.tbQ
  tb.Text = res["q"] 
  tb = frmTI_4.tbH
  tb.Text = res["h"]
  tb = frmTI_4.tbX
  tb.Text = res["x"]
  tb = frmTI_4.tbK
  tb.Text = res["k"]
  frmTI_4.gbRadiobtnSign.btnChoice.Click()
  if file:
    filepath = aqFileSystem.ExpandFileName("..\Resources\TestFiles\\"+res["file"])
    test_choose_file(filepath, alies)

def test_parameters_check_sign(excel_sheet, alies, file_s=False):  
  if file_s:
    res = DDTWork.get_values(excel_sheet, ["p", "q", "h", "y", "file_s"])
  else:
    res = DDTWork.get_values(excel_sheet, ["p", "q", "h", "y"])
  
  TI_4 = alies
  frmTI_4 = TI_4.frmTI_4
  frmTI_4.rbCheckSign.Select()
  tb = frmTI_4.tbP
  tb.Text = res["p"] 
  tb = frmTI_4.tbQ
  tb.Text = res["q"] 
  tb = frmTI_4.tbH
  tb.Text = res["h"]
  tb = frmTI_4.tbY
  tb.Text = res["y"]
  frmTI_4.gbRadiobtnSign.btnChoice.Click()
  if file_s:
      filepath = aqFileSystem.ExpandFileName("..\Resources\TestFiles\\"+res["file_s"])
      test_choose_file(filepath, alies)
  
# def test_dlg_warning(static, static_wnd_caption, alies):
  
def test_choose_file(filepath, alies):
  dlg = alies.dlg_2
  dlg.cbx_2.ComboBox.Edit.SetText(filepath)
  dlg.btn_.ClickButton()
  
def test_sign_values(sheet_excel, alies):
  TI_4 = alies
  values_check = DDTWork.get_values(sheet_excel, ["y", "g", "hash", "r", "s"])
  res = aqString.Compare(TI_4.frmTI_4.tbY.Text, values_check["y"], True)
  if res != 0:
    Log.Error("y is incorrect")
  res = aqString.Compare(TI_4.frmTI_4.tbG.Text, values_check["g"], True)
  if res != 0:
    Log.Error("g is incorrect")
  res = aqString.Compare(TI_4.frmTI_4.tbR.Text, values_check["r"], True)
  if res != 0:
    Log.Error("r is incorrect")
  res = aqString.Compare(TI_4.frmTI_4.tbS.Text, values_check["s"], True)
  if res != 0:
    Log.Error("s is incorrect")
  res = aqString.Compare(TI_4.wndTI_4.gbSignResult.tbHM.Text, values_check["hash"], True)
  if res != 0:
    Log.Error("H(M) is incorrect") 
  
def test_check_sign_values(sheet_excel, alies):
  TI_4 = alies
  values_check = DDTWork.get_values(sheet_excel, ["g", "hash", "r", "s", "w", "v"]) 
  res = aqString.Compare(TI_4.frmTI_4.tbG.Text, values_check["g"], True)
  if res != 0:
    Log.Error("g is incorrect")
  res = aqString.Compare(TI_4.frmTI_4.tbHMC.Text, values_check["hash"], True)
  if res != 0:
    Log.Error("H(M') is incorrect")
  res = aqString.Compare(TI_4.frmTI_4.tbRC.Text, values_check["r"], True)
  if res != 0:
    Log.Error("r is incorrect")
  res = aqString.Compare(TI_4.frmTI_4.tbSC.Text, values_check["s"], True)
  if res != 0:
    Log.Error("s is incorrect")
  res = aqString.Compare(TI_4.frmTI_4.tbW.Text, values_check["w"], True)
  if res != 0:
    Log.Error("w is incorrect")
  res = aqString.Compare(TI_4.frmTI_4.tbV.Text, values_check["v"], True)
  if res != 0:
    Log.Error("v is incorrect")
