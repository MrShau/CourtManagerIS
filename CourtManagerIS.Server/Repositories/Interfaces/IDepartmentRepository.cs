using CourtManagerApi.Models;
using System.Linq.Expressions;

namespace CourtManagerApi.Repositories.Interfaces
{
    public interface IDepartmentRepository
    {
        public bool IsExist(string name);

        public Task<Department?> Add(Department department);
        public Task<Department?> Get(int id, params string[] includes);
        public Task<List<Department>> GetAll(params string[] includes);
        public Task<List<Department>> GetList(int count, int page, params string[] includes);
        public Task<List<Department>> Where(Expression<Func<Department, bool>> expression, params string[] includes);
        public Task SaveChanges();
    }
}
