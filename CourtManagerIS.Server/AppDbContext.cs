using CourtManagerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CourtManagerApi
{
    internal static class DataAccessExtensions
    {
        internal static IQueryable<T> IncludeMultiple<T>(this IQueryable<T> query,
            params string[] includes) where T : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            return query;
        }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Activity> ActivityHistory { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<CourtCase> CourtCases { get; set; }
        public DbSet<Participant> Participants { get; set; }

        public void Initialize()
        {
            try
            {
                if (Departments.Count() == 0)
                {
                    Departments.Add(new Department("Управление судебного департамента"));
                    Departments.Add(new Department("Отдел финансов, бухгалтерского учета и отчетности"));
                    Departments.Add(new Department("Отдел государственной службы, кадров и противодействия коррупции"));
                    Departments.Add(new Department("Отдел управления недвижимостью, капитального строительства и материально-технического обеспечения"));
                    SaveChanges();
                    Positions.Add(new Position("Администратор", Departments.FirstOrDefault(), Privilege.ADMIN));
                    var person = People.Add(new Person("Ислам", "Нагоев", "Асланович", "1111", "666666", new DateTime(2004, 11, 27), Gender.Male, EducationType.VocationalSecondary)).Entity;
                    SaveChanges();
                    var employee = Employees.Add(new Employee("admin", "27112004nag", person, Positions.FirstOrDefault())).Entity;
                    SaveChanges();
                    foreach (var department in Departments.ToList())
                    {
                        Positions.Add(new Position("Руководитель", department, Privilege.DIRECTOR));
                        department.Supervisor = employee;
                    }
                    var judgeDepartment = Departments.Add(new Department("Отдел организационно-правового обеспечения деятельности судов")).Entity;
                    SaveChanges();

                    if (judgeDepartment != null)
                    {
                        Positions.Add(new Position("Судья", judgeDepartment, Privilege.JUDGE));
                        SaveChanges();
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Initialize();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>()
                .Property(p => p.Privilege)
                .HasMaxLength(50)
                .HasConversion(v => v.ToString(), v => (Privilege)Enum.Parse(typeof(Privilege), v));

            modelBuilder.Entity<Person>()
                .Property(p => p.Gender)
                .HasMaxLength(8)
                .HasConversion(v => v.ToString(), v => (Gender)Enum.Parse(typeof(Gender), v));

            modelBuilder.Entity<Person>()
                .Property(p => p.EducationType)
                .HasMaxLength(50)
                .HasConversion(v => v.ToString(), v => (EducationType)Enum.Parse(typeof(EducationType), v));

            modelBuilder.Entity<Employee>()
                .HasMany<Notification>(e => e.SentNotifications)
                .WithOne(n => n.Sender)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Employee>()
                .HasMany<Notification>(e => e.Notifications)
                .WithOne(e => e.Recipient)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Department>()
                .HasOne<Employee>(d => d.Supervisor)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}
