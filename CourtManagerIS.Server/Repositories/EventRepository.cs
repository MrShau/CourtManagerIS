using Api.Repositories.Interfaces;
using CourtManagerApi;
using CourtManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;
        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Event?> Add(Event e)
        {
            try
            {
                Event ev = (await _context.Events.AddAsync(e))?.Entity; ;
                await _context.SaveChangesAsync();
                return ev;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }

        public async Task<List<Event>> Where(Expression<Func<Event, bool>> expression, params string[] includes)
        {
            try
            {
                return await _context.Events.IncludeMultiple(includes).Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new List<Event> { };
            }
        }
    }
}
