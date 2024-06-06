using CourtManagerApi.Models;
using System.Linq.Expressions;

namespace CourtManagerApi.Repositories.Interfaces
{
    public interface IPositionRepository
    {
        public Task<Position?> Add(Position position);
        public Task<Position?> Get(int id, params string[] includes);
        public Task<Position?> Get(string name);
        public Task<List<Position>> GetAll(params string[] includes);
        public Task<List<Position>> Where(Expression<Func<Position, bool>> expression, params string[] includes);
        public Task<Position?> FirstWhere(Expression<Func<Position, bool>> expression, params string[] includes);
    }
}
