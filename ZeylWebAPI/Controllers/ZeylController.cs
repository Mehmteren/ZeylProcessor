using Microsoft.AspNetCore.Mvc;
using ZeylAPI.Services.Interfaces;

namespace ZeylAPI.Controllers
{
    [ApiController]
    [Route("api/zeyil")]
    public class ZeylController : ControllerBase
    {
        private readonly IExcelService _excelService;
        private readonly IZeylService _zeylService;
        private readonly IFileStorageService _fileStorage;

        public ZeylController(
            IExcelService excelService,
            IZeylService zeylService,
            IFileStorageService fileStorage)
        {
            _excelService = excelService;
            _zeylService = zeylService;
            _fileStorage = fileStorage;
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest(new { message = "Dosya seçilmedi" });

                if (!IsExcelFile(file))
                    return BadRequest(new { message = "Geçersiz dosya formatı" });

                using var stream = file.OpenReadStream();
                var records = await _excelService.ReadAsync(stream);

                if (!records.Any())
                    return BadRequest(new { message = "Dosyada veri bulunamadı" });

                _zeylService.ProcessAltGrupNumbers(records);

                var fileData = await _excelService.CreateAsync(records);

                var fileName = $"Processed_{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                var downloadId = await _fileStorage.StoreAsync(fileData, fileName);

                return Ok(new
                {
                    success = true,                    
                    message = "Başarıyla işlendi",     
                    processedCount = records.Count,   
                    downloadId = downloadId            
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet("download/{id}")]
        public async Task<IActionResult> Download(string id)
        {
            var file = await _fileStorage.GetAsync(id);

            if (file == null)
                return NotFound(new { message = "Dosya bulunamadı" });

            return File(file.Value.data,                                            
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", 
                file.Value.fileName);                                              
        }

        private static bool IsExcelFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return extension is ".xlsx" or ".xls";
        }
    }
}