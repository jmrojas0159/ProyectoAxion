using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using System;
using System.IO;
using System.Linq;

namespace Crosscutting.Common.Tools.ExcelGenerator
{
    public sealed class ExcelGenerator : IDisposable
    {
        private readonly ReportDto _data;
        private HSSFWorkbook _hssfworkbook;

        public ExcelGenerator(ReportDto data)
        {
            _data = data;
        }

        public void Dispose()
        {
            _hssfworkbook = null;
        }

        private void InitializeWorkbook()
        {
            _hssfworkbook = new HSSFWorkbook();

            // create a entry of DocumentSummaryInformation
            var dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "Axion";
            _hssfworkbook.DocumentSummaryInformation = dsi;

            // create a entry of SummaryInformation
            var si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "Report";
            _hssfworkbook.SummaryInformation = si;
        }

        public MemoryStream WriteExcelToStream()
        {
            // Write the stream data of workbook to the root directory
            WriteReportWithData();

            using (var file = new MemoryStream())
            {
                _hssfworkbook.Write(file);
                return file;
            }
        }

        private void WriteReportWithData()
        {
            var reportdata = _data.ReportData.ToList().AsParallel();

            InitializeWorkbook();
            var sheet = _hssfworkbook.CreateSheet("request");
            _hssfworkbook.CreateSheet("hidden");

            var style = _hssfworkbook.CreateCellStyle();
            style.FillForegroundColor = HSSFColor.DarkBlue.Index;
            style.FillPattern = FillPattern.SolidForeground;
            var font = _hssfworkbook.CreateFont();
            font.Color = HSSFColor.White.Index;
            style.SetFont(font);

            var rows = reportdata.Count() + 1;

            for (var j = 0; j < rows; j++)
            {
                var row = sheet.CreateRow(j);

                for (var i = 0; i < _data.Columns.Count; i++)
                {
                    var header = _data.Columns.ElementAt(i);
                    if (j == 0)
                    {
                        var cell = row.CreateCell(i);
                        cell.SetCellValue(header);
                        cell.CellStyle = style;
                    }
                    else
                    {
                        var dictionary = reportdata.ElementAt(j - 1);
                        row.CreateCell(i).SetCellValue(dictionary[header]);
                    }
                }
                sheet.AutoSizeColumn(j);
            }
            sheet.SetColumnHidden(0, true);
            var namedcell = _hssfworkbook.CreateName();
            namedcell.NameName = "hidden";
            var constraint = DVConstraint.CreateFormulaListConstraint("hidden");
            var addressList = new CellRangeAddressList(1, reportdata.Count(), _data.Columns.Count,
                _data.Columns.Count);
            var validation = new HSSFDataValidation(addressList, constraint);
            _hssfworkbook.SetSheetHidden(1, true);
        }
    }
}