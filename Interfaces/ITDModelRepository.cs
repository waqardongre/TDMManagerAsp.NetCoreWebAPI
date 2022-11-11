using TDM.DTOs;
using TDM.Models;

namespace TDM.Interfaces
{
    public interface ITDModelRepository
    {
        public Task<IEnumerable<TDModel>> GetTDModels(long UserId);
        public Task<TDModel> GetTDModel(long id);
        public Task UpdateTDModel(long id, TDModelDTO tDModelDTO);
        public Task<TDModelDTO> CreateTDModel(TDModelDTO tDModel);
        public Task DeleteTDModel(long id);
        public byte[] GetBinaryModel(IFormFile model);
    }
}