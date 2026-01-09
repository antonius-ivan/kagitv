// WisataService.cs

using AIRMDataManager.Library.Modules.Traveloka.Wisata.Models;

namespace Kagitv.Server.Services.Traveloka.Wisata
{
    public interface IWisataService
    {
        Task<bool> DeleteWisataAsync(int id);
        Task<List<AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata>> GetAllWisatasAsync();
        Task<AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata?> GetWisataByIdAsync(int id);
        Task<int> InsertWisataAsync(AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata wisata);
        Task<int> UpdateWisataAsync(AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata wisata);
    }
}
