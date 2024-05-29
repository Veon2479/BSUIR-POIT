import TestSamples
  
def test_no_sequence_sign():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_sign("KeyNone", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static, "Введите", TI_4)
  
def test_no_parameters_check_sign():  
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_check_sign("KeyNone", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static, "Введите", TI_4)
  
def test_chars_sign():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_sign("Chars", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static2, "не числовое значение", TI_4)

def test_chars_check_sign():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_check_sign("Chars", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static2, "не числовое значение", TI_4)

def test_sign_p_not_prime():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_sign("p_not_prime", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static3, "p не простое число", TI_4)
  
def test_check_sign_p_not_prime():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_check_sign("p_not_prime", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static3, "p не простое число", TI_4)
  
def test_sign_q_not_prime():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_sign("q_not_prime", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static4, "q не простое число", TI_4)

def test_check_sign_q_not_prime():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_check_sign("q_not_prime", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static4, "q не простое число", TI_4)

def test_sign_q_not_p_1():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_sign("q_p-1", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static5, "q не является делителем (p - 1)", TI_4)  

def test_check_sign_q_not_p_1():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_check_sign("q_p-1", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static5, "q не является делителем (p - 1)", TI_4)  
def test_sign_h_not_interval():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_sign("h_interval", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static6, "h не в интервале (1, p - 1)", TI_4)  
def test_check_sign_h_not_interval():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_check_sign("h_interval", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static6, "h не в интервале (1, p - 1)", TI_4)
def test_sign_x_not_interval():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_sign("x_interval", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static7, "x не в интервале (0, q)", TI_4)
def test_sign_k_not_interval():
  TI_4 = Aliases.TI_4
  TestSamples.test_parameters_sign("k_interval", TI_4)
  TestSamples.test_dlg_warning(TI_4.dlg_.Static8, "k не в интервале (0, q)", TI_4)