import TestSamples

def test_sign_empty_file():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_sign("emptyfile", TI_4, True)
  TestSamples.test_sign_values("emptyfile", TI_4)
  TI_4.frmTI_4.gbRadiobtnSign.btnSave.Click()
  filepath = aqFileSystem.ExpandFileName("..\Resources\TestFiles\empty_s.txt")
  TestSamples.test_choose_file(filepath, TI_4)
  
def test_check_sign_empty_file():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_check_sign("emptyfile", TI_4, True)
  TestSamples.test_check_sign_values("emptyfile", TI_4)
  aqObject.CheckProperty(TI_4.frmTI_4.lblResult, "Text", cmpContains, "Подпись верна")
  
def test_sign_BSUIR():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_sign("BSUIR", TI_4, True)
  TestSamples.test_sign_values("BSUIR", TI_4)
  
  TI_4.frmTI_4.gbRadiobtnSign.btnSave.Click()
  filepath = aqFileSystem.ExpandFileName("..\Resources\TestFiles\BSUIR_s.txt")
  TestSamples.test_choose_file(filepath, TI_4)
  
def test_check_sign_BSUIR():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_check_sign("BSUIR", TI_4, True)
  TestSamples.test_check_sign_values("BSUIR", TI_4)
  aqObject.CheckProperty(TI_4.frmTI_4.lblResult, "Text", cmpContains, "Подпись верна")
  
def test_wrong_format_check_sign():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_check_sign("wrong_format", TI_4, True)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static9, "Подпись не может быть проверена", TI_4)
  
def test_wrong_signature():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_check_sign("wrong_signature", TI_4, True)
  aqObject.CheckProperty(TI_4.frmTI_4.lblResult, "Text", cmpContains, "Подпись не верна")