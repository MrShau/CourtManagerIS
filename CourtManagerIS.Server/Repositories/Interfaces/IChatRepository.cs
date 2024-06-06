using CourtManagerApi.Models;

namespace Api.Repositories.Interfaces
{
    public interface IChatRepository
    {
        public Task<ChatMessage?> Add(ChatMessage message);
        public Task<List<ChatMessage>> GetAll(params string[] includes);
    }
}
