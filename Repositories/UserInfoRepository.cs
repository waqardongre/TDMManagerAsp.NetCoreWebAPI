using System.Data;
using Dapper;
using TDM.Interfaces;
using TDM.Models;

namespace TDM.Repositories {
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly DapperContext _context;
        public UserInfoRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<UserRegisterModel> UserRegister(UserRegisterModel UserRegister)
        {
            var query = "INSERT INTO UserInfo (UserName,DisplayName,Email,Password,CreatedDate,UpdatedDate)"
            + " VALUES (@UserName,@DisplayName,@Email,@Password,@CreatedDate,@UpdatedDate)"
            + " SELECT * from UserInfo where UserId = CAST(SCOPE_IDENTITY() as bigint)";
            var parameters = new DynamicParameters();
            parameters.Add("UserName", UserRegister.UserName, DbType.String);
            parameters.Add("DisplayName", UserRegister.DisplayName, DbType.String);
            parameters.Add("Email", UserRegister.Email, DbType.String);
            parameters.Add("Password", UserRegister.Password, DbType.String);
            parameters.Add("CreatedDate", DateTime.Now, DbType.DateTime2);
            parameters.Add("UpdatedDate", DateTime.Now, DbType.DateTime2);
            using (var connection = _context.CreateConnection())
            {
                UserRegisterModel userRegisterObj = await connection.QueryFirstOrDefaultAsync<UserRegisterModel>(query, parameters);
                return userRegisterObj;
            }
        }

        public async Task<UserLoginModel> UserLogin(UserLoginModel userLogin)
        {
            var query = "SELECT * FROM UserInfo WHERE Email = @Email and Password = @Password";
            using (var connection = _context.CreateConnection())
            {
                string? Email = userLogin.Email;
                string Password = userLogin.Password;
                UserLoginModel userInfoObj = await connection.QueryFirstOrDefaultAsync<UserLoginModel>(query, new { Email, Password });
                return userInfoObj;
            }
        }

        public async Task<UserLoginModel> GetUserDetails(long userId)
        {
            var query = "SELECT * FROM UserInfo WHERE UserId = @UserId";
            using (var connection = _context.CreateConnection())
            {
                UserLoginModel userInfoObj = await connection.QueryFirstOrDefaultAsync<UserLoginModel>(query, new { userId });
                return userInfoObj;
            }
        }

        public async Task<bool> IsEmailExist(string email)
        {
            bool result = true;
            var query = "SELECT Email FROM UserInfo WHERE Email = @Email";
            using (var connection = _context.CreateConnection())
            {
                string Email = email;
                UserRegisterModel userInfoObj = await connection.QueryFirstOrDefaultAsync<UserRegisterModel>(query, new { Email});
                if (userInfoObj == null) {
                    result = false;
                }
                return result;
            }
        }
        public async Task<bool> IsUserNameExist(string userName)
        {
            bool result = true;
            var query = "SELECT * FROM UserInfo WHERE UserName = @UserName";
            using (var connection = _context.CreateConnection())
            {
                UserRegisterModel userInfoObj = await connection.QueryFirstOrDefaultAsync<UserRegisterModel>(query, new { userName });
                if (userInfoObj == null) {
                    result = false;
                }
                return result;
            }
        }
    }
}
