def get_values(sheet_name, values):
  DDT.ExcelDriver(".\Resources\TestSequence.xlsx", sheet_name)
  result = {}
  for value in values:
    result[value] = DDT.CurrentDriver.Value[value]
  DDT.CloseDriver(DDT.CurrentDriver.Name)
  return result
  

def getWindow():
  process = Sys.Process("TI-4")
  window = process.Child(0)
  for i in range(process.ChildCount):
    if process.Child(i).Visible == True:
      window = process.Child(i)
  return window