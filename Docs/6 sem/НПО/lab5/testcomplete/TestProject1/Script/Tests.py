import TestSamples
  
def test_no_sequence_sign():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_sign("KeyNone", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static, "Введите", DSA)
  
def test_no_parameters_check_sign():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_check_sign("KeyNone", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static, "Введите", DSA)
  
def test_chars_sign():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_sign("Chars", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static2, "не числовое значение", DSA)

def test_chars_check_sign():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_check_sign("Chars", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static2, "не числовое значение", DSA)

def test_sign_p_not_prime():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_sign("p_not_prime", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static3, "p не простое число", DSA)
  
def test_check_sign_p_not_prime():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_check_sign("p_not_prime", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static3, "p не простое число", DSA)
  
def test_sign_q_not_prime():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_sign("q_not_prime", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static4, "q не простое число", DSA)

def test_check_sign_q_not_prime():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_check_sign("q_not_prime", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static4, "q не простое число", DSA)

def test_sign_q_not_p_1():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_sign("q_p-1", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static5, "q не является делителем (p - 1)", DSA)
  
def test_check_sign_q_not_p_1():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_check_sign("q_p-1", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static5, "q не является делителем (p - 1)", DSA)
  
def test_sign_h_not_interval():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_sign("h_interval", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static6, "h не в интервале (1, p - 1)", DSA)
  
def test_check_sign_h_not_interval():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_check_sign("h_interval", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static6, "h не в интервале (1, p - 1)", DSA)

def test_sign_x_not_interval():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_sign("x_interval", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static7, "x не в интервале (0, q)", DSA)

def test_sign_k_not_interval():
  DSA = Aliases.Dsa
  TestSamples.test_parameters_sign("k_interval", DSA)
  TestSamples.test_dlg_warning(DSA.dlg_.Static8, "k не в интервале (0, q)", DSA)
