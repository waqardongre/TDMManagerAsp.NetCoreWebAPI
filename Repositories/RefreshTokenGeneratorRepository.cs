using System.Data;
using System.Security.Cryptography;
using Dapper;
using TDM.Interfaces;
using TDM.Models;

namespace TDM.Repositories
{
    public class RefreshTokenGeneratorRepository : IRefreshTokenGeneratorRepository
    {
        private readonly DapperContext _context;
        public RefreshTokenGeneratorRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<string> GenerateToken(long userId)
        {
            string refreshToken = "";
            var randomnumber = new byte[32];
            using (var randomnumbergenerator = RandomNumberGenerator.Create())
            {
                randomnumbergenerator.GetBytes(randomnumber);
                refreshToken = Convert.ToBase64String(randomnumber);

                var _user = await IsRefreshTokenExists(userId);
                if (_user != null)
                {
                    TblRefreshtoken tblRefreshtoken = new TblRefreshtoken()
                    {
                        UserId = userId,
                        RefreshToken= refreshToken,
                    };
                    _user.RefreshToken = refreshToken;
                    tblRefreshtoken.RefreshTokenId = await UpdateRefreshToken(tblRefreshtoken);
                }
                else
                {
                    TblRefreshtoken tblRefreshtoken = new TblRefreshtoken()
                    {
                        UserId=userId,
                        Token=new Random().Next().ToString(),
                        RefreshToken= refreshToken,
                    };
                    tblRefreshtoken.RefreshTokenId = await SaveRefreshToken(tblRefreshtoken);

                }
                return refreshToken;
            }
        }
        public async Task<string?> GetUserRefreshToken(long UserId, string RefreshToken)
        {
            string? refreshToken = null;
            var query = "Exec PR_getUserRefreshToken @UserId = @UserId, @RefreshToken = @RefreshToken";
            using (var connection = _context.CreateConnection())
            {
                UserLoginModel userLoginObj = await connection.QueryFirstOrDefaultAsync<UserLoginModel>(query, new { UserId, RefreshToken });
                if (userLoginObj.RefreshToken != null)
                    refreshToken = userLoginObj.RefreshToken;
            }
            return refreshToken;
        }
        public async Task<TblRefreshtoken> IsRefreshTokenExists(long userId)
        {
            var query = "SELECT * FROM RefreshToken WHERE UserId = @userId";
            using (var connection = _context.CreateConnection())
            {
                TblRefreshtoken tblRefreshtoken = await connection.QueryFirstOrDefaultAsync<TblRefreshtoken>(query, new { userId});
                return tblRefreshtoken;
            }
        }
        public async Task<long> SaveRefreshToken(TblRefreshtoken user)
        {
            long id = -1;
            var query = "INSERT INTO RefreshToken (UserId, Token, RefreshToken)"
            + " VALUES (@UserId, @Token, @RefreshToken)"
            + " SELECT CAST(SCOPE_IDENTITY() as bigint)";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", user.UserId, DbType.Int64);
            parameters.Add("Token", user.Token, DbType.String);
            parameters.Add("RefreshToken", user.RefreshToken, DbType.String);
            using (var connection = _context.CreateConnection())
            {
                id = await connection.QuerySingleAsync<long>(query, parameters);
            };        
            return id;
        }
        public async Task<long> UpdateRefreshToken(TblRefreshtoken user)
        {
            long id = -1;
            var query = "UPDATE RefreshToken SET RefreshToken = @RefreshToken WHERE UserId = @UserId" 
            + " SELECT CAST(SCOPE_IDENTITY() as bigint)";
            var parameters = new DynamicParameters();
            parameters.Add("UserId", user.UserId, DbType.Int64);
            parameters.Add("RefreshToken", user.RefreshToken, DbType.String);
            using (var connection = _context.CreateConnection())
            {
                id = await connection.ExecuteAsync(query, parameters);
            }
            return id;
        }
    }
}
