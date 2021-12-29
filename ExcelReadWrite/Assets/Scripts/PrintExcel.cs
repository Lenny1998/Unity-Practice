using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using OfficeOpenXml;
using System.IO;

public class PrintExcel : MonoBehaviour {
    public List<DepenceTableData> listdata;
    void Start () {
        //读
        //Text T = GameObject.Find("Canvas/Text").GetComponent<Text>();
        //T.text = "";//清空一开始的文本
        //listdata = DoExcel.Load(Application.streamingAssetsPath + "/Test2007.xlsx");

        //foreach (var listing in listdata)
        //{
        //    print(listing.instruct + "     " + listing.word);
        //    T.text += (listing.instruct + "     " + listing.word + "\n").ToString();
        //}

        //写
        WriteExcel(Application.streamingAssetsPath + "/Test2007.xlsx");

    }


    public static void WriteExcel(string outputDir)
    {
        //string outputDir = EditorUtility.SaveFilePanel("Save Excel", "", "New Resource", "xlsx");
        FileInfo newFile = new FileInfo(outputDir);
        if (newFile.Exists)
        {
            newFile.Delete();  // ensures we create a new workbook
            newFile = new FileInfo(outputDir);
        }
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            // add a new worksheet to the empty workbook
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
            //Add the headers
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Product";
            worksheet.Cells[1, 3].Value = "Quantity";
            worksheet.Cells[1, 4].Value = "Price";
            worksheet.Cells[1, 5].Value = "Value";

            //Add some items...
            worksheet.Cells["A2"].Value = 12001;
            worksheet.Cells["B2"].Value = "Nails";
            worksheet.Cells["C2"].Value = 37;
            worksheet.Cells["D2"].Value = 3.99;

            worksheet.Cells["A3"].Value = 12002;
            worksheet.Cells["B3"].Value = "Hammer";
            worksheet.Cells["C3"].Value = 5;
            worksheet.Cells["D3"].Value = 12.10;

            worksheet.Cells["A4"].Value = 12003;
            worksheet.Cells["B4"].Value = "Saw";
            worksheet.Cells["C4"].Value = 12;
            worksheet.Cells["D4"].Value = 15.37;

            //save our new workbook and we are done!
            package.Save();
        }
    }


}
