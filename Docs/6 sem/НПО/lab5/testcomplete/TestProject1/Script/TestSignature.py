import TestSamples

def test_sign_empty_file():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_sign("emptyfile", DSA, True)
  TestSamples.test_sign_values("emptyfile", DSA)
  DSA.frmDsa.gbRadiobtnSign.btnSave.Click()
  filepath = aqFileSystem.ExpandFileName("..\Resources\TestFiles\empty_s.txt")
  TestSamples.test_choose_file(filepath, DSA)
  
def test_check_sign_empty_file():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_check_sign("emptyfile", DSA, True)
  TestSamples.test_check_sign_values("emptyfile", DSA)
  aqObject.CheckProperty(DSA.frmDsa.lblResult, "Text", cmpContains, "Подпись верна")
  
def test_sign_BSUIR():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_sign("BSUIR", DSA, True)
  TestSamples.test_sign_values("BSUIR", DSA)
  
  DSA.frmDsa.gbRadiobtnSign.btnSave.Click()
  filepath = aqFileSystem.ExpandFileName("..\Resources\TestFiles\BSUIR_s.txt")
  TestSamples.test_choose_file(filepath, DSA)
  
def test_check_sign_BSUIR():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_check_sign("BSUIR", DSA, True)
  TestSamples.test_check_sign_values("BSUIR", DSA)
  aqObject.CheckProperty(DSA.frmDsa.lblResult, "Text", cmpContains, "Подпись верна")
  
def test_wrong_format_check_sign():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_check_sign("wrong_format", DSA, True)
  TestSamples.test_dlg_warning(DSA.dlg_.Static9, "Подпись не может быть проверена", DSA)
  
def test_wrong_signature():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_check_sign("wrong_signature", DSA, True)
  aqObject.CheckProperty(DSA.frmDsa.lblResult, "Text", cmpContains, "Подпись не верна")
  