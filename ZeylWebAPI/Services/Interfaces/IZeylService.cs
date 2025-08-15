using ZeylAPI.Models.Entities;

namespace ZeylAPI.Services.Interfaces
{
    public interface IZeylService
    {
        void ProcessAltGrupNumbers(List<ZeylRecord> records);
    }
}
