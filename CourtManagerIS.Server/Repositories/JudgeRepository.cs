using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourtManagerApi.Repositories
{
    public class JudgeRepository : IJudgeRepository
    {
        private readonly AppDbContext _context;

        public JudgeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetList(params string[] includes)
        {
            var list = new List<Employee>();
            try
            {
                list = await _context.Employees.IncludeMultiple(includes).Where(e => e.Position.Privilege == Privilege.JUDGE).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return list;
        }
    }
}
