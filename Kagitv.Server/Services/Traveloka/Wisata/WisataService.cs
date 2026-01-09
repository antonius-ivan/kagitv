// WisataService.cs

using AIRMDataManager.Library.Modules.Traveloka.Wisata.DataAccess;
using AIRMDataManager.Library.Modules.Traveloka.Wisata.Models;

namespace Kagitv.Server.Services.Traveloka.Wisata
{
    public class WisataService : IWisataService
    {
        private readonly IWisataRepository _wisataRepository;

        public WisataService(IWisataRepository wisataRepository)
        {
            _wisataRepository = wisataRepository;
        }

        public Task<List<AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata>> GetAllWisatasAsync()
            => _wisataRepository.GetAllWisatasAsync();

        public async Task<AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata?> GetWisataByIdAsync(int id)
        {
            // return null if not found
            return await _wisataRepository.GetWisataByIdAsync(id);
        }

        public async Task<int> InsertWisataAsync(AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata wisata)
            => await _wisataRepository.InsertWisataAsync(wisata);

        public async Task<int> UpdateWisataAsync(AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata wisata)
            => await _wisataRepository.UpdateWisataAsync(wisata);

        public async Task<bool> DeleteWisataAsync(int id)
            => await _wisataRepository.DeleteWisataAsync(id);
    }
}
