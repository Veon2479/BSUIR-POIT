using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Dynamic;

namespace DynamicObjects
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateAndUseDynamicObject();
            CreateAndUseDynamicWriter();
            //CreateExcelDocument();
            Console.ReadLine();
        }

        private static void CreateAndUseDynamicObject()
        {
            using (StreamWriter writer = new StreamWriter(
                @"P:\My\БГУИР\Code.MyLectures.DotNet\DynamicObjects\Test.txt"))
            {
                UseDynamicObject(writer);
            }
            try
            {
                UseDynamicObject(new FileStream(
                    @"P:\My\БГУИР\Code.MyLectures.DotNet\DynamicObjects\Test2.txt", 
                    FileMode.Create));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void UseDynamicObject(dynamic dynObj)
        {
            dynObj.WriteLine("Отчет");
        }

        private static void CreateAndUseDynamicWriter()
        {
            DynamicWriter writer = new DynamicWriter();
            UseDynamicObject(writer);
        }

        private static void CreateExcelDocument()
        {
            Microsoft.Office.Interop.Excel.Application excel = new Application();
            excel.Visible = true;
            Workbook workbook = excel.Workbooks.Add();
            dynamic cell = excel.Cells[1, 1];
            cell.Value = "Отчет";
            workbook.SaveAs(Filename: @"P:\My\БГУИР\Code.MyLectures.DotNet\DynamicObjects\Test.xlsx");
        }
    }

    public class DynamicWriter : DynamicObject
    {
        private void DoWriteLine(object arg)
        {
            Console.WriteLine(arg);
        }

        public void WriteLine(object arg)
        {
            Console.WriteLine("Вызван WriteLine");
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder,
            object[] args, out object result)
        {
            bool memberFound = false;
            result = null;
            if (binder.Name == "WriteLine")
            {
                Console.WriteLine("Вызван TryInvokeMember");
                DoWriteLine(args[0]);
                memberFound = true;
            }
            return memberFound;
        }
    }
}
