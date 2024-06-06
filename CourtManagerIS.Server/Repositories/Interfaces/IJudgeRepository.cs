using CourtManagerApi.Models;

namespace CourtManagerApi.Repositories.Interfaces
{
    public interface IJudgeRepository
    {
        public Task<List<Employee>> GetList(params string[] includes);
    }
}
