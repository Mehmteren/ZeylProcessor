using ZeylAPI.Models.Entities;

namespace ZeylAPI.Services.Interfaces
{
    public interface IExcelService
    {
        Task<List<ZeylRecord>> ReadAsync(Stream stream);
        Task<byte[]> CreateAsync(List<ZeylRecord> records);
    }
}