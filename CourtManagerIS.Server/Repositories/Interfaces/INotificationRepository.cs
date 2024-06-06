using CourtManagerApi.Models;
using System.Linq.Expressions;

namespace CourtManagerApi.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        public Task<Notification?> Add(Notification notification);
        public Task<List<Notification>> Where(Expression<Func<Notification, bool>> expression, params string[] includes);

        public Task SaveChanges();
    }
}
