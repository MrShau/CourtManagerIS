using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourtManagerApi.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Person?> Add(Person person)
        {
            try
            {
                Person? result = (await _context.People.AddAsync(person))?.Entity;
                await _context.SaveChangesAsync();
                return result;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }

        public async Task<Person?> Get(int id)
        {
            try
            {
                return await  _context.People.FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }

        public async Task<Person?> Get(string seriePassport, string numberPassport)
        {
            try
            {
                return await _context.People.FirstOrDefaultAsync(p => p.SeriePassport == seriePassport && p.NumberPassport == numberPassport);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return null;
            }
        }
    }
}
