

namespace AIRMDataManager.Library.Modules.Traveloka.Wisata.DataAccess
{
    public interface IWisataRepository
    {
        Task<bool> DeleteWisataAsync(int id);
        Task<List<AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata>> GetAllWisatasAsync();
        Task<AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata?> GetWisataByIdAsync(int id);
        Task<int> InsertWisataAsync(AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata Wisata);
        Task<int> UpdateWisataAsync(AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata Wisata);
    }
}
