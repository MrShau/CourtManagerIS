using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace CourtManagerApi.Repositories
{
    

    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool IsExist(string login)
        {
            try
            {
                return _context.Employees.FirstOrDefault(e => e.Login == login.ToLower()) != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task<Employee?> Add(Employee employee)
        {
            Employee? result = null;
            try
            {
                if (this.IsExist(employee.Login))
                    return result;

                result = (await _context.Employees.AddAsync(employee)).Entity;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return result;
        }

        public async Task<Employee?> Get(string login, params string[] includes)
        {
            try
            {
                return await _context.Employees.IncludeMultiple(includes).FirstOrDefaultAsync(e => e.Login == login.ToLower());
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return null;
        }

        public async Task<Employee?> Get(int id, params string[] includes)
        {
            try
            {
                return await _context.Employees.IncludeMultiple(includes).FirstOrDefaultAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return null;
        }

        public async Task AddActivity(Activity activity)
        {
            try
            {
                await _context.ActivityHistory.AddAsync(activity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        public async Task<List<Employee>> GetList(SortType sortType, EducationType? educationType, int departmentId, Gender? gender, string? loginLike, int count, int page, params string[] includes)
        {
            List<Employee> result = new List<Employee>();
            try
            {
                switch (sortType)
                {
                    case SortType.Descending:
                        result = (await _context.Employees.CountAsync()) > count 
                            ? await _context.Employees.IncludeMultiple(includes)
                                .Where(e => 
                                    (loginLike == null ? true : e.Login.StartsWith(loginLike)) 
                                    && (educationType == null ? true : e.Person.EducationType == educationType) 
                                    && (departmentId < 0 ? true : e.Position.DepartmentId == departmentId) 
                                    && (gender == null ? true : e.Person.Gender == gender))
                                .OrderByDescending(e => e.Id)
                                .Skip((page * count) - count)
                                .Take(count)
                                .ToListAsync() 
                            : await _context.Employees.IncludeMultiple(includes)
                                .Where(e => 
                                    (loginLike == null ? true : e.Login.StartsWith(loginLike)) 
                                    && (educationType == null ? true : e.Person.EducationType == educationType) 
                                    && (departmentId < 0 ? true : e.Position.DepartmentId == departmentId) 
                                    && (gender == null ? true : e.Person.Gender == gender))
                                .OrderByDescending(e => e.Id)
                                .ToListAsync();
                        break;
                    case SortType.Ascending:
                    default:
                        result = (await _context.Employees.CountAsync()) > count
                            ? await _context.Employees.IncludeMultiple(includes).Where(e => (loginLike == null ? true : e.Login.StartsWith(loginLike)) && (educationType == null ? true : e.Person.EducationType == educationType) && (departmentId < 0 ? true : e.Position.DepartmentId == departmentId) && (gender == null ? true : e.Person.Gender == gender)).Skip((page * count) - count).Take(count).ToListAsync()
                            : await _context.Employees.IncludeMultiple(includes).Where(e => (loginLike == null ? true : e.Login.StartsWith(loginLike)) && (educationType == null ? true : e.Person.EducationType == educationType) && (departmentId < 0 ? true : e.Position.DepartmentId == departmentId) && (gender == null ? true : e.Person.Gender == gender)).ToListAsync();
                        break;
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return result;
        }

        public async Task<List<Employee>> GetList(int departmentId, SortType sortType, EducationType? educationType, Gender? gender, string? loginLike, int count, int page, params string[] includes)
        {
            List<Employee> result = new List<Employee>();
            try
            {
                switch (sortType)
                {
                    case SortType.Descending:
                        result = (await _context.Employees.CountAsync()) > count
                        ? await _context.Employees.IncludeMultiple(includes).Where(e => (loginLike == null ? true : e.Login.StartsWith(loginLike)) && (educationType == null ? true : e.Person.EducationType == educationType) &&  e.Position.DepartmentId == departmentId && (gender == null ? true : e.Person.Gender == gender)).OrderByDescending(e => e.Id).Skip((page * count) - count).Take(count).ToListAsync()
                        : await _context.Employees.IncludeMultiple(includes).Where(e => (loginLike == null ? true : e.Login.StartsWith(loginLike)) && (educationType == null ? true : e.Person.EducationType == educationType) &&  e.Position.DepartmentId == departmentId && (gender == null ? true : e.Person.Gender == gender)).OrderByDescending(e => e.Id).ToListAsync();
                        break;
                    case SortType.Ascending:
                    default:
                        result = (await _context.Employees.CountAsync()) > count
                        ? await _context.Employees.IncludeMultiple(includes).Where(e => (loginLike == null ? true : e.Login.StartsWith(loginLike)) && (educationType == null ? true : e.Person.EducationType == educationType) && e.Position.DepartmentId == departmentId && (gender == null ? true : e.Person.Gender == gender)).Skip((page * count) - count).Take(count).ToListAsync()
                        : await _context.Employees.IncludeMultiple(includes).Where(e => (loginLike == null ? true : e.Login.StartsWith(loginLike)) && (educationType == null ? true : e.Person.EducationType == educationType) && e.Position.DepartmentId == departmentId && (gender == null ? true : e.Person.Gender == gender)).ToListAsync();
                        break;
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
            return result;
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

        public async Task<bool> IsAdminOrDirector(string login)
        {
            try
            {
                Privilege? privilege = (await this.Get(login, "Position"))?.Position?.Privilege;
                if (!privilege.HasValue)
                    return false;
                return privilege.Value == Privilege.ADMIN || privilege.Value == Privilege.DIRECTOR;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }

        public async Task<bool> IsDirector(string login)
        {
            try
            {
                Privilege? privilege = (await this.Get(login, "Position"))?.Position?.Privilege;
                if (!privilege.HasValue)
                    return false;
                return privilege.Value == Privilege.DIRECTOR;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }

        public async Task<bool> IsJudge(string login)
        {
            try
            {
                Privilege? privilege = (await this.Get(login, "Position"))?.Position?.Privilege;
                if (!privilege.HasValue)
                    return false;
                return privilege.Value == Privilege.JUDGE;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return false;
            }
        }

        public async Task<List<Employee>> Where(Expression<Func<Employee, bool>> expression, params string[] includes)
        {
            try
            {
                return await _context.Employees.IncludeMultiple(includes).Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new List<Employee> { };
            }
        }
    }
}
