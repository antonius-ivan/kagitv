using AIRMDataManager.Library.Common.DataAccess;
using AIRMDataManager.Library.SystemCoreDataAccess;
using AIRMDataManager.Library.Modules.Traveloka.Wisata.Models;
using AIRMDataManager.Library.Modules.Traveloka.Wisata.DataAccess;

namespace AIRMDataManager.Library.Modules.Tourney.Wisata.DataAccess
{
    public class MsSqlWisataRepository : IWisataRepository
    {
        private readonly IDatabaseConnectionFactory _connectionFactory;
        private readonly ISqlDataAccess _sqlDataAccess;

        public MsSqlWisataRepository(IDatabaseConnectionFactory connectionFactory, ISqlDataAccess sqlDataAccess)
        {
            _connectionFactory = connectionFactory;
            _sqlDataAccess = sqlDataAccess;
        }

        public async Task<List<AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata>> GetAllWisatasAsync()
        {
            // Example: Use _connectionFactory.CreateConnection() to get a DB connection,
            // then execute your query asynchronously.
            // For demonstration, we return a dummy list:
            //await Task.CompletedTask;
            //return new List<WisataModel>
            //{
            //    new WisataModel { Id = 1, PlaceName = "Champion Trophy", WisataAmount = 1000 },
            //    new WisataModel { Id = 2, PlaceName = "Runner-Up Medal", WisataAmount = 500 }
            //};

            // Assuming you have a stored procedure named 'spGetCatalogBrands'
            var storedProcedure = "sp_Wisata_GetAll";
            var databaseCode = "DefaultConnection"; // Change this to your actual database code

            var brands = await _sqlDataAccess.LoadDataStoredProcedureAsync<AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata, dynamic>(storedProcedure, new { }, databaseCode);
            return brands;
        }

        public async Task<AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata?> GetWisataByIdAsync(int id)
        {
            const string storedProcedure = "sp_Wisata_GetById";
            const string databaseCode = "DefaultConnection";

            var parameters = new { WisataId = id };

            var Wisatas = await _sqlDataAccess.LoadDataStoredProcedureAsync<AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata, dynamic>(storedProcedure, parameters, databaseCode);

            return Wisatas.FirstOrDefault();
        }


        public async Task<int> InsertWisataAsync(AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata Wisata)
        {
            const string storedProcedure = "sp_Wisata_Insert";
            const string databaseCode = "DefaultConnection";

            var parameters = new
            {
                Wisata.Nama,
                Wisata.Kota,
                Wisata.Harga,
                //cre_dttm = DateTime.Now, // Use current timestamp for creation
                //cre_by = "SYS" // Replace this with actual user context
            };

            // Execute the stored procedure using the provided SaveDataCodeProcedureAsync method
            await _sqlDataAccess.SaveDataStoredProcedureAsync(storedProcedure, parameters, databaseCode);

            // Assuming you want to return the newly created ID, you may need an additional query or output parameter.
            // Here, it's assumed the stored procedure handles returning the ID separately.
            return 0; // Placeholder; adjust as needed if ID retrieval is implemented.
        }

        public async Task<int> UpdateWisataAsync(AIRMDataManager.Library.Modules.Traveloka.Wisata.Models.Wisata Wisata)
        {
            const string storedProcedure = "sp_Wisata_Update";
            const string databaseCode = "DefaultConnection";

            var parameters = new
            {
                Wisata.WisataID,
                Wisata.Nama,
                Wisata.Kota,
                Wisata.Harga
                //,
                //upd_dttm = DateTime.Now, // Use current timestamp for update
                //upd_by = "SYS" // Replace this with actual user context
            };

            await _sqlDataAccess.SaveDataStoredProcedureAsync(storedProcedure, parameters, databaseCode);

            return Wisata.WisataID; // Returning the updated Wisata ID
        }

        public async Task<bool> DeleteWisataAsync(int id)
        {
            const string storedProcedure = "sp_Wisata_Delete";
            const string databaseCode = "DefaultConnection";

            var parameters = new
            {
                WisataId = id
            };

            try
            {
                await _sqlDataAccess.SaveDataStoredProcedureAsync(storedProcedure, parameters, databaseCode);
                return true;
            }
            catch (Exception)
            {
                // You can log the exception if needed
                return false;
            }
        }
    }
}
