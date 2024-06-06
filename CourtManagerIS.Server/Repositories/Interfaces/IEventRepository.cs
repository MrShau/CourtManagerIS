using CourtManagerApi.Models;
using System.Linq.Expressions;

namespace Api.Repositories.Interfaces
{
    public interface IEventRepository
    {
        public Task<Event?> Add(Event e);
        public Task<List<Event>> Where(Expression<Func<Event, bool>> expression, params string[] includes);
    }
}
