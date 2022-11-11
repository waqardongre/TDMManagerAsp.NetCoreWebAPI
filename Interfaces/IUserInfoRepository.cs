using TDM.Models;

namespace TDM.Interfaces
{
    public interface IUserInfoRepository
    {
        public Task<UserRegisterModel> UserRegister(UserRegisterModel userRegister);
        public Task<UserLoginModel> UserLogin(UserLoginModel userInfo);
        public Task<bool> IsEmailExist(string email);
        public Task<UserLoginModel> GetUserDetails(long userId);
        public Task<bool> IsUserNameExist(string userName);
    }
}