using Api.Repositories.Interfaces;
using CourtManagerApi;
using CourtManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _context;

        public ChatRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ChatMessage?> Add(ChatMessage message)
        {
            try
            {
                ChatMessage c = (await _context.ChatMessages.AddAsync(message))?.Entity;
                await _context.SaveChangesAsync();
                return c;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }

        public async Task<List<ChatMessage>> GetAll(params string[] includes)
        {
            try
            {
                return await _context.ChatMessages.IncludeMultiple(includes).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new List<ChatMessage> { };
            }
        }
    }
}
