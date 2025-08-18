using Europa.Resources;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

/**
 * @Author Thalles Batista 
 */
namespace Europa.Extensions
{
    public class ExcelUtil
    {
        private byte[] FileDownload { get; set; }
        private ExcelPackage ExcelPackage { get; set; }
        private int? DefaultColWidth { get; set; }

        private static string _author = "Europa";
        private static string _title = "Europa";
        private static string _comments = "Grupo Europa Empreendimentos e Negócios";
        private static string FormatMoney = "_-\"R$\"* #,##0.00_-;\\-\"R$\"* #,##0.00_-;_-\"R$\"* \"-\"??_-;_-@_-";
        private static string FormatDate = "dd/MM/yyyy";

        private Dictionary<int, ICollection<string>> _headers;

        private int IdxPage { get; set; }
        private int IdxCell { get; set; }
        private int IdxRow { get; set; }
        private int IdxCurrentRankSheet { get; set; }
        private Dictionary<string, int> RankSheets { get; set; }
        private Dictionary<string, string> RankFormulas { get; set; }

        private ExcelUtil(int? defaultColWidth, Stream stream)
        {
            ExcelPackage = new ExcelPackage(stream ?? new MemoryStream());
            ExcelPackage.Workbook.Properties.Author = _author;
            ExcelPackage.Workbook.Properties.Title = _title;
            ExcelPackage.Workbook.Properties.Comments = _comments;
            _headers = new Dictionary<int, ICollection<string>>();
            RankSheets = new Dictionary<string, int>();
            RankFormulas = new Dictionary<string, string>();
            IdxCurrentRankSheet = 1;
            DefaultColWidth = defaultColWidth;
        }

        public static ExcelUtil NewInstance(int? defaultColWidth, Stream stream)
        {
            return new ExcelUtil(defaultColWidth, stream);
        }

        public static ExcelUtil NewInstance(int? defaultColWidth)
        {
            return new ExcelUtil(defaultColWidth, null);
        }

        public static ExcelUtil NewInstance(Stream stream)
        {
            return new ExcelUtil(null, stream);
        }

        public static ExcelUtil NewInstance()
        {
            return new ExcelUtil(null, null);
        }

        public ExcelUtil NewSheet(string name)
        {
            ExcelPackage.Workbook.Worksheets.Add(name);
            IdxPage++;
            IdxRow = 0;
            IdxCell = 0;
            return this;
        }

        public ExcelUtil WithHeader(params string[] header)
        {
            var workSheet = CurrentExcelWorksheet();

            if (DefaultColWidth.HasValue && DefaultColWidth.Value > 0)
            {
                workSheet.DefaultColWidth = DefaultColWidth.Value;
            }
            workSheet.Cells.Style.WrapText = false;

            _headers[IdxPage] = header;

            new List<string>(header).ForEach(x => CreateCellValue(x));
            return this;
        }

        public ExcelUtil WithHeaderCustom(params string[] header)
        {
            var workSheet = CurrentExcelWorksheet();

            if (DefaultColWidth.HasValue && DefaultColWidth.Value > 0)
            {
                workSheet.DefaultColWidth = DefaultColWidth.Value;
            }


            _headers[IdxPage] = header;
            workSheet.Cells[1, 1, 1, header.Length].AutoFilter = true;
            new List<string>(header).ForEach(x => CreateCellValueCustom(x, true));
            return this;
        }

        public ExcelUtil CreateCellValueCustom(dynamic value, bool isHeader = false)
        {
            var workSheet = CurrentExcelWorksheet();
            if (IdxCell == _headers[IdxPage].Count)
            {
                IdxCell = 0;
                IdxRow++;
            }
            if (value is DateTime && ((DateTime?)value) == DateTime.MinValue)
            {
                value = null;
            }
            else if (value is bool)
            {
                value = value ? GlobalMessages.Sim : GlobalMessages.Nao;
            }
            else if (value is Enum)
            {
                value = value > 0 ? ((Enum)value).AsString() : "";
            }
            var cell = workSheet.Cells[IdxRow + 1, ++IdxCell];
            cell.Style.Font.Bold = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            if (isHeader)
            {
                cell.Style.Font.Bold = true;
                cell.Style.Font.Color.SetColor(Color.DarkBlue);
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
            }
            cell.Value = value;
            return this;
        }

        public ExcelUtil CreateCellValueLink(dynamic value, bool isHeader = false, string url = "")
        {
            var workSheet = CurrentExcelWorksheet();
            if (IdxCell == _headers[IdxPage].Count)
            {
                IdxCell = 0;
                IdxRow++;
            }
            if (value is DateTime && ((DateTime?)value) == DateTime.MinValue)
            {
                value = null;
            }
            else if (value is bool)
            {
                value = value ? GlobalMessages.Sim : GlobalMessages.Nao;
            }
            else if (value is Enum)
            {
                value = value > 0 ? ((Enum)value).AsString() : "";
            }
            var cell = workSheet.Cells[IdxRow + 1, ++IdxCell];
            cell.Style.Font.Bold = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            if (isHeader)
            {
                cell.Style.Font.Bold = true;
                cell.Style.Font.Color.SetColor(Color.DarkBlue);
                cell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cell.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
            }
            else if (!url.IsEmpty())
            {
                cell.Hyperlink = new Uri(url);
            }
            cell.Value = value;
            return this;
        }

        public ExcelUtil CreateCellValue(dynamic value)
        {
            var workSheet = CurrentExcelWorksheet();
            if (IdxCell == _headers[IdxPage].Count)
            {
                IdxCell = 0;
                IdxRow++;
            }
            if (value is DateTime && ((DateTime?)value) == DateTime.MinValue)
            {
                value = null;
            }
            else if (value is bool)
            {
                value = value ? GlobalMessages.Sim : GlobalMessages.Nao;
            }
            else if (value is Enum)
            {
                value = value > 0 ? ((Enum)value).AsString() : "";
            }
            workSheet.Cells[IdxRow + 1, ++IdxCell].Value = value;
            return this;
        }

        public ExcelUtil AddRankValidation(string formula, int startCell, int endCell, int colIndex)
        {
            //"=Resultados!$A$1:$A$3"
            var workSheet = CurrentExcelWorksheet();
            var range = ExcelCellBase.GetAddress(startCell, colIndex, endCell, colIndex);
            var val = workSheet.DataValidations.AddListValidation(range);
            val.Formula.ExcelFormula = formula;
            return this;
        }

        public ExcelUtil CreateDateTimeCell(DateTime? value)
        {
            ExcelRange currentCell = CurrentExcelWorksheet().Cells[IdxRow + 1, ++IdxCell];
            currentCell.Style.Numberformat.Format = FormatDate;

            if (value == null || value.Value == DateTime.MinValue)
            {
                currentCell.Value = null;
            }
            else
            {
                currentCell.Value = value.Value;
            }
            return this;
        }

        public ExcelUtil CreateMoneyCell(decimal? value)
        {
            ExcelRange currentCell = CurrentExcelWorksheet().Cells[IdxRow + 1, ++IdxCell];
            currentCell.Style.Numberformat.Format = FormatMoney;

            if (value == null)
            {
                currentCell.Value = null;
            }
            else
            {
                currentCell.Value = value.Value;
            }
            return this;
        }

        public ExcelUtil Format(string pattern)
        {
            var workSheet = CurrentExcelWorksheet();
            workSheet.Cells[IdxRow + 1, IdxCell].Style.Numberformat.Format = pattern;
            return this;
        }

        public ExcelUtil FormatToCurrency(string currency)
        {
            var workSheet = CurrentExcelWorksheet();
            var value = workSheet.Cells[IdxRow + 1, IdxCell].GetValue<decimal>();
            var pattern = currency + " #.###,";
            if (value == 0)
            {
                pattern = currency + " 0,00";
            }
            else if (value % 1 == 0)
            {
                pattern = pattern + "00";
            }
            else if (value % .1M == 0)
            {
                pattern = pattern + "#0";
            }
            else
            {
                pattern = pattern + "##";
            }
            workSheet.Cells[IdxRow + 1, IdxCell].Style.Numberformat.Format = pattern;
            return this;
        }

        public ExcelUtil Width(int width)
        {
            var workSheet = CurrentExcelWorksheet();
            workSheet.Cells[IdxRow + 1, IdxCell].AutoFitColumns(width);
            return this;
        }

        public void SaveAs(string name)
        {
            ExcelPackage.SaveAs(new FileInfo("C:\\Users\\Julio Effgen\\Documents\\teste_" + DateTime.Now.ToString("yyyyMMddHHmmss")));
        }

        public ExcelUtil Close()
        {
            ExcelPackage.Save();
            var stream = ExcelPackage.Stream as MemoryStream;
            if (stream != null)
            {
                FileDownload = stream.ToArray();
            }
            return this;
        }

        public ExcelWorksheet CurrentExcelWorksheet()
        {
            return ExcelPackage.Workbook.Worksheets[IdxPage];
        }

        public byte[] DownloadFile()
        {
            return FileDownload;
        }

        public ExcelUtil LockSheet()
        {
            var workSheet = CurrentExcelWorksheet();
            workSheet.Protection.IsProtected = true;
            return this;
        }

        public ExcelUtil AddComment(string comment)
        {
            var workSheet = CurrentExcelWorksheet();
            workSheet.Cells[IdxRow + 1, IdxCell].AddComment(comment, "SUAT");
            return this;
        }

        public string CreateRankSheet(string name, string[] values)
        {
            var ddList = ExcelPackage.Workbook.Worksheets.Add(name);
            for (var index = 1; index <= values.Length; index++)
            {
                ddList.Cells[index, 1].Value = values[index - 1];
            }
            IdxCurrentRankSheet++;
            RankSheets.Add(name, IdxCurrentRankSheet);
            var formula = $"={name}!$A$1:$A${values.Length}";
            RankFormulas.Add(name, formula);

            return formula;
        }

        public ExcelUtil HideColumnRange(int[] columns)
        {
            var workSheet = CurrentExcelWorksheet();
            for (int i = 0; i < columns.Length; i++)
            {
                workSheet.Column(columns[i]).Hidden = true;

            }
            return this;
        }
    }
}
