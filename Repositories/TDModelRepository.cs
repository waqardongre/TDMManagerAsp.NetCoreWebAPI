using System.Data;
using Dapper;
using TDM.DTOs;
using TDM.Interfaces;
using TDM.Models;

namespace TDM.Repositories {
    public class TDModelRepository : ITDModelRepository
    {
        private readonly DapperContext _context;
        public TDModelRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TDModel>> GetTDModels(long userId)
        {
            string query = "exec PR_GETTDMODELS @USERID = @USERID";    
            using (var connection = _context.CreateConnection())
            {
                var tDModels = await connection.QueryAsync<TDModel>(query, new { userId }, commandTimeout: 0);
                return tDModels.ToList();
            }
        }
        public async Task<TDModel> GetTDModel(long id)
        {
            var query = "SELECT * FROM TDModels WHERE Id = @id";
            using (var connection = _context.CreateConnection())
            {
                var tDModels = await connection.QuerySingleOrDefaultAsync<TDModel>(query, new { id }, commandTimeout: 0);
                return tDModels;
            }
        }

        public async Task<TDModelDTO> CreateTDModel(TDModelDTO tDModelDTO)
        {
            var query = "INSERT INTO TDModels (UserId, Email, ModelName, Model, CreatedDate, UpdatedDate)"
            + " VALUES (@UserId, @Email, @ModelName, @Model, @CreatedDate, @UpdatedDate)"
            + " SELECT CAST(SCOPE_IDENTITY() as bigint)";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", tDModelDTO.UserId, DbType.String);
            parameters.Add("Email", tDModelDTO.Email, DbType.String);
            parameters.Add("ModelName", tDModelDTO.ModelName, DbType.String);
            if (tDModelDTO.ModelIFormFile != null) {
                tDModelDTO.Model = GetBinaryModel(tDModelDTO.ModelIFormFile);
                parameters.Add("Model", tDModelDTO.Model, DbType.Binary);
            }
            parameters.Add("CreatedDate", tDModelDTO.CreatedDate, DbType.DateTime2);
            parameters.Add("UpdatedDate", tDModelDTO.UpdatedDate, DbType.DateTime2);
            using (var connection = _context.CreateConnection())
            {
                long id = await connection.QuerySingleAsync<long>(query, parameters, commandTimeout: 0);
                return new TDModelDTO() {
                    Id = id
                };
            }
        }

        public async Task UpdateTDModel(long id, TDModelDTO tDModelDTO)
        {
            var query = "";
            var parameters = new DynamicParameters();
            tDModelDTO.UpdatedDate = DateTime.Now;
            
            if (tDModelDTO.ModelIFormFile != null) {
                tDModelDTO.Model = GetBinaryModel(tDModelDTO.ModelIFormFile);
                parameters.Add("Model", tDModelDTO.Model, DbType.Binary);
            }

            parameters.Add("Id", id, DbType.Int64);
            parameters.Add("UpdatedDate", tDModelDTO.UpdatedDate, DbType.DateTime2);
            if (tDModelDTO.ModelIFormFile != null && tDModelDTO.ModelName != null) {
                query = "UPDATE TDModels SET ModelName = @ModelName, Model = @Model, UpdatedDate = @UpdatedDate WHERE Id = @Id";
                parameters.Add("ModelName", tDModelDTO.ModelName, DbType.String);
            }
            else if (tDModelDTO.ModelIFormFile == null && tDModelDTO.ModelName != null) {
                tDModelDTO.ModelName = tDModelDTO.ModelName;
                query = "UPDATE TDModels SET ModelName = @ModelName, UpdatedDate = @UpdatedDate WHERE Id = @Id";
                parameters.Add("ModelName", tDModelDTO.ModelName, DbType.String);
            }
            else if (tDModelDTO.ModelIFormFile != null && tDModelDTO.ModelName == null) {
                query = "UPDATE TDModels SET Model = @Model, UpdatedDate = @UpdatedDate WHERE Id = @Id";
            }

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters, commandTimeout: 0);
            }
        }

        public async Task DeleteTDModel(long id)
        {
            var query = "DELETE FROM TDModels WHERE Id = @id";
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, new { id }, commandTimeout: 0);
            }
        }

        public byte[] GetBinaryModel(IFormFile model) {
            long length = model.Length;
            byte[] binaryModel = new byte[length];
            using var fileStream = model.OpenReadStream();
            fileStream.Read(binaryModel, 0, (int)length);
            return binaryModel;
        }
    }
}