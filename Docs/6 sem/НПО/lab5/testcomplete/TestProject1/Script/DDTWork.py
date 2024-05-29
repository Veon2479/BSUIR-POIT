def get_values(sheet_name, values):
  DDT.ExcelDriver("..\Resources\TestSequence.xlsx", sheet_name)
  result = {}
  for value in values:
    result[value] = DDT.CurrentDriver.Value[value]
  DDT.CloseDriver(DDT.CurrentDriver.Name)
  return result
  