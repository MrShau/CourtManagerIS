using Api.Repositories.Interfaces;
using CourtManagerApi;
using CourtManagerApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly AppDbContext _context;

        public CandidateRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Candidate?> Add(Candidate canditate)
        {
            try
            {
                var candidate = (await _context.Candidates.AddAsync(canditate))?.Entity;
                await _context.SaveChangesAsync();
                return candidate;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }

        public async Task<List<Candidate>> GetList(SortType sortType, EducationType? educationType, int departmentId, Gender? gender, int count, int page, params string[] includes)
        {
            try
            {
                switch (sortType)
                {
                    case SortType.Descending:
                        return (await _context.Candidates.CountAsync()) > count
                            ? await _context.Candidates.IncludeMultiple(includes)
                                .Where(e => (educationType == null ? true : e.Person.EducationType == educationType) && (departmentId < 0 ? true : e.DesiredPosition.DepartmentId == departmentId) && (gender == null ? true : e.Person.Gender == gender))
                                .OrderByDescending(e => e.Id)
                                .Skip((page * count) - count)
                                .Take(count)
                                .ToListAsync()
                            : await _context.Candidates.IncludeMultiple(includes)
                                .Where(e => (educationType == null ? true : e.Person.EducationType == educationType) && (departmentId < 0 ? true : e.DesiredPosition.DepartmentId == departmentId) && (gender == null ? true : e.Person.Gender == gender))
                                .OrderByDescending(e => e.Id)
                                .ToListAsync();
                    case SortType.Ascending:
                    default:
                        return (await _context.Candidates.CountAsync()) > count
                            ? await _context.Candidates.IncludeMultiple(includes).Where(e => (educationType == null ? true : e.Person.EducationType == educationType) && (departmentId < 0 ? true : e.DesiredPosition.DepartmentId == departmentId) && (gender == null ? true : e.Person.Gender == gender)).Skip((page * count) - count).Take(count).ToListAsync()
                            : await _context.Candidates.IncludeMultiple(includes).Where(e => (educationType == null ? true : e.Person.EducationType == educationType) && (departmentId < 0 ? true : e.DesiredPosition.DepartmentId == departmentId) && (gender == null ? true : e.Person.Gender == gender)).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new List<Candidate>();
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

        public async Task<List<Candidate>> Where(Expression<Func<Candidate, bool>> expression, params string[] includes)
        {
            try
            {
                return await _context.Candidates.IncludeMultiple(includes).Where(expression).ToListAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return new List<Candidate>();
            }
        }
    }
}
