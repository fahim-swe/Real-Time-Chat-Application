using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using api.Database;
using api.Model;
using api.Helper;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _usesCollection;
        public UserService(IOptions<ApiDataBaseSetttings> userNameStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                userNameStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                userNameStoreDatabaseSettings.Value.DatabaseName);

            _usesCollection = mongoDatabase.GetCollection<User>(
                userNameStoreDatabaseSettings.Value.CollectionName);
        }
        public async Task AddAsync(User newUser)
        {
            await _usesCollection.InsertOneAsync(newUser);
        }

        public async Task<int> CountAsync()
        {
            return (int)await _usesCollection.EstimatedDocumentCountAsync();
        }

        public async Task DeleteAsync(string id)
        {
            await _usesCollection.DeleteOneAsync(x => x.id == id);
        }

        public async Task<IEnumerable<User>> getUserAsync(PaginationFilter filter)
        {
            return await _usesCollection.Find(_ => true)
                                        .Skip((filter.PageNumber - 1) * filter.PageSize)
                                        .Limit(filter.PageSize)
                                        .ToListAsync();
        }

        public async Task<User> getByIdAsync(string id)
        {
            return await _usesCollection.Find(x => x.id == id).FirstOrDefaultAsync();
        }

        public async Task<User> getByUserNameAsync(string userName)
        {
            return await _usesCollection.Find(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<bool> isEmailExitAsync(string email)
        {
            return await _usesCollection.Find( _=> _.Email == email).AnyAsync();
        }

        public async Task UpdateAsync(string id, User updateUser)
        {
            await _usesCollection.ReplaceOneAsync(x => x.id == id, updateUser);
        }

    }
}