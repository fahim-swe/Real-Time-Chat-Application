
using api.Model;

namespace api.Services
{
    public interface IAccountService
    {
       Task AddUserAsync(User user);
       Task<User> GetByUserNameAsync(String userName);
       Task<Boolean> isEmailExitAsync(String email);

       Task AddUserTokenAsync(UserTokens userTokens);
       Task<UserTokens> GetUserTokensByNameAsync(String userName);
       Task UpdateUserTokensAsync(String key, UserTokens userTokens);

       Task<UserTokens> GetUserTokenByTokenId(string id);

       Task<Boolean> isValidRefershKey(string key);
    }
}