using api.Model;
using api.DTOs;
namespace api.Services{
    public interface IOnlineService{
        public Task<List<MemberDTO>> getOnlineUsers();
        public  Task< bool> updateOnlineStatus(string key);

        
    }
}