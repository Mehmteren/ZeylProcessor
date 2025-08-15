namespace ZeylAPI.Services.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> StoreAsync(byte[] data, string fileName);
        Task<(byte[] data, string fileName)?> GetAsync(string id);
    }
}