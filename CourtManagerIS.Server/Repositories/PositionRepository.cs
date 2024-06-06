using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourtManagerApi.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly AppDbContext _context;

        public PositionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Position?> Add(Position position)
        {
            try
            {
                Position? result = (await _context.Positions.AddAsync(position))?.Entity;
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }

        public async Task<Position?> Get(int id, params string[] includes)
        {
            try
            {
                return await _context.Positions.IncludeMultiple(includes).FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }

        public async Task<Position?> Get(string name)
        {
            try
            {
                return await _context.Positions.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }

        public async Task<List<Position>> GetAll(params string[] includes)
        {
            try
            {
                return await _context.Positions.IncludeMultiple(includes).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new List<Position>();
            }
        }

        public async Task<List<Position>> Where(Expression<Func<Position, bool>> expression, params string[] includes)
        {
            try
            {
                return await _context.Positions.IncludeMultiple(includes).Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new List<Position> { };
            }
        }

        public async Task<Position?> FirstWhere(Expression<Func<Position, bool>> expression, params string[] includes)
        {
            try
            {
                return await _context.Positions.IncludeMultiple(includes).FirstOrDefaultAsync(expression);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }
    }
}
