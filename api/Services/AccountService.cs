using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Database;
using api.DTOs;
using api.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<UserTokens> _userTokensCollection;

        public AccountService(IOptions<ApiDataBaseSetttings> userNameStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                userNameStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                userNameStoreDatabaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(
                userNameStoreDatabaseSettings.Value.CollectionName);
            _userTokensCollection = mongoDatabase.GetCollection<UserTokens> (
                userNameStoreDatabaseSettings.Value.UserTokenCollectionName
                );
        }
        
        public async Task AddUserAsync(User user)
        {
            await _usersCollection.InsertOneAsync(user);
        }

        public async Task AddUserTokenAsync(UserTokens userTokens)
        {
            await _userTokensCollection.InsertOneAsync(userTokens); 
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _usersCollection.Find(x => x.UserName == userName).FirstOrDefaultAsync();
        }

        public async Task<UserTokens> GetUserTokenByTokenId(string id)
        {
            return await _userTokensCollection.Find(x => x.id == id).FirstOrDefaultAsync();
        }

        public async Task<UserTokens> GetUserTokensByNameAsync(string userName)
        {
            var result = await _userTokensCollection.Find(x => x.UserName == userName).FirstOrDefaultAsync();
            return result;
        }

        public async Task<bool> isEmailExitAsync(string email)
        {
            return await _usersCollection.Find( _=> _.Email == email).AnyAsync();
        }

        public async Task<bool> isValidRefershKey(string key)
        {
            return await _userTokensCollection.Find(x => x.RefreshToken == key).AnyAsync();
        }

        public async Task UpdateUserTokensAsync(string key, UserTokens userTokens)
        {
            await _userTokensCollection.ReplaceOneAsync(x=>x.RefreshToken == key, userTokens);
        }
    }
}