using OfficeOpenXml;
using ZeylAPI.Models.Entities;
using ZeylAPI.Services.Interfaces;

namespace ZeylAPI.Services
{
    public class ExcelService : IExcelService
    {
        public Task<List<ZeylRecord>> ReadAsync(Stream stream)
        {
            var records = new List<ZeylRecord>();

            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets.FirstOrDefault();

            if (worksheet?.Dimension == null)
                return Task.FromResult(records);

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var zeylNo = worksheet.Cells[row, 1].Text?.Trim();
                if (string.IsNullOrEmpty(zeylNo)) continue;

                records.Add(new ZeylRecord
                {
                    ZeylNo = zeylNo,                                                   
                    AnaGrup = worksheet.Cells[row, 2].Text?.Trim() ?? string.Empty,    
                    Sigortali = worksheet.Cells[row, 4].Text?.Trim() ?? string.Empty  
                });
            }
            return Task.FromResult(records);
        }

        public Task<byte[]> CreateAsync(List<ZeylRecord> records)
        {
            using var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Zeyil");

            SetHeaders(worksheet);
            WriteRecords(worksheet, records);

            worksheet.Cells.AutoFitColumns();
            return Task.FromResult(package.GetAsByteArray());
        }

        private static void SetHeaders(ExcelWorksheet worksheet)
        {
            worksheet.Cells[1, 1].Value = "Zeyil No";          
            worksheet.Cells[1, 2].Value = "Ana Grup";           
            worksheet.Cells[1, 3].Value = "ALT GRUP ZEYİL NO";  
            worksheet.Cells[1, 4].Value = "Sigortalı";          
        }

        private static void WriteRecords(ExcelWorksheet worksheet, List<ZeylRecord> records)
        {
            for (int i = 0; i < records.Count; i++)
            {
                var record = records[i];       
                var row = i + 2;               

                worksheet.Cells[row, 1].Value = record.ZeylNo;        
                worksheet.Cells[row, 2].Value = record.AnaGrup;       
                worksheet.Cells[row, 3].Value = record.AltGrupZeylNo; 
                worksheet.Cells[row, 4].Value = record.Sigortali;    
            }
        }
    }
}