using TDM.Models;

namespace TDM.Interfaces
{
    public  interface IRefreshTokenGeneratorRepository
    {
        Task<string> GenerateToken(long userId);
        Task<string?> GetUserRefreshToken(long userId, string token);
        Task<long> SaveRefreshToken(TblRefreshtoken user);
        Task<long> UpdateRefreshToken(TblRefreshtoken user);
    }
}
