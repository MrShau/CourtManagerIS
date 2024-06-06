using CourtManagerApi.Models;

namespace CourtManagerApi.Repositories.Interfaces
{
    public interface IPersonRepository
    {
        public Task<Person?> Add(Person person);
        public Task<Person?> Get(int id);
        public Task<Person?> Get(string seriePassport, string numberPassport);
    }
}
