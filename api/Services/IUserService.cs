using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Helper;
using api.Model;

namespace api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> getUserAsync(PaginationFilter filter);
        Task<User> getByIdAsync(string id);
        Task UpdateAsync(string id, User updateUser);
        Task DeleteAsync(string id);

        Task<User> getByUserNameAsync(string userName);

        Task<bool> isEmailExitAsync(string email);

        Task<int> CountAsync();
    }
}