using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CourtManagerApi.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool IsExist(string name)
        {
            return _context.Departments.FirstOrDefault(d => d.Name == name) != null;
        }

        public async Task<Department?> Add(Department department)
        {
            try
            {
                var result = (await _context.Departments.AddAsync(department))?.Entity;
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return null;
        }

        public async Task<Department?> Get(int id, params string[] includes)
        {
            try
            {
                return await _context.Departments.IncludeMultiple(includes).FirstOrDefaultAsync(d => d.Id == id);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return null;
        }

        public async Task<List<Department>> GetAll(params string[] includes)
        {
            var result = new List<Department>();
            try
            {
                result = await _context.Departments.IncludeMultiple(includes).ToListAsync();
            }
            catch  (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return result;
        }

        public async Task<List<Department>> GetList(int count, int page, params string[] includes)
        {
            List<Department> result = new List<Department>();
            try
            {
                result = (await _context.Departments.CountAsync()) > count ? await _context.Departments.IncludeMultiple(includes).Skip((page * count) - count).Take(count).ToListAsync() : await _context.Departments.IncludeMultiple(includes).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return result;
        }

        public async Task<List<Department>> Where(Expression<Func<Department, bool>> expression,  params string[] includes)
        {
            try
            {
                return await _context.Departments.IncludeMultiple(includes).Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new List<Department>();
            }
        }
        public async Task SaveChanges()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}
