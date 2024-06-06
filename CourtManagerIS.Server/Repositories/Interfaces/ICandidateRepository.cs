using CourtManagerApi;
using CourtManagerApi.Models;
using System.Linq.Expressions;

namespace Api.Repositories.Interfaces
{
    public interface ICandidateRepository
    {
        public Task<Candidate?> Add(Candidate canditate);

        public Task<List<Candidate>> GetList(SortType sortType, EducationType? educationType, int departmentId, Gender? gender, int count, int page, params string[] includes);

        public Task<List<Candidate>> Where(Expression<Func<Candidate, bool>> expression, params string[] includes);
        public Task SaveChanges();
    }
}
