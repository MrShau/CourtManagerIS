using CourtManagerApi.Models;
using System.Linq.Expressions;

namespace CourtManagerApi.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        public bool IsExist(string login);

        public Task<Employee?> Add(Employee employee);
        public Task<Employee?> Get(string login, params string[] includes);
        public Task<Employee?> Get(int id, params string[] includes);
        public Task AddActivity(Activity activity);

        public Task<bool> IsAdminOrDirector(string login);
        public Task<bool> IsDirector(string login);
        public Task<bool> IsJudge(string login);
        public Task<List<Employee>> Where(Expression<Func<Employee, bool>> expression, params string[] includes);

        public Task<List<Employee>> GetList(SortType sortType, EducationType? educationType, int departmentId, Gender? gender, string? loginLike, int count, int page, params string[] includes);
        public Task<List<Employee>> GetList(int departmentId, SortType sortType, EducationType? educationType, Gender? gender, string? loginLike, int count, int page, params string[] includes);
        public Task SaveChanges();
    }
}
