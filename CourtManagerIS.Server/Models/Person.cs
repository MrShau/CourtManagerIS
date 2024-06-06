using System.ComponentModel.DataAnnotations;

namespace CourtManagerApi.Models
{
    public enum Gender
    {
        Female = 0,
        Male = 1
    }

    public enum EducationType
    {
        VocationalSecondary,
        Bachelor,
        Master,
        Postgraduate
    }

    public class Person
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Patronymic { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        public string SeriePassport { get; set; }
        
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string NumberPassport { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public EducationType EducationType { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        Person() => CreatedAt = DateTime.Now;
        public Person(string firstName, string lastName, string patronymic, string seriePassport, string numberPassport, DateTime dateOfBirth, Gender gender, EducationType educationType) : this()
        {
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            SeriePassport = seriePassport;
            NumberPassport = numberPassport;
            DateOfBirth = dateOfBirth;
            this.Gender = gender;
            this.EducationType = educationType;
        }

        public string GetEducationType()
        {
            switch (EducationType)
            {
                case EducationType.VocationalSecondary:
                    return "Среднее профессиональное";
                case EducationType.Bachelor:
                    return "Бакалавриат";
                case EducationType.Master:
                    return "Магистратура";
                case EducationType.Postgraduate:
                    return "Аспирантура";
                default:
                    return "null";
            }
        }

        public string GetGender() => Gender == Gender.Female ? "Женский" : "Мужской";
        public string GetFIO() => $"{LastName} {FirstName} {Patronymic}";
    }
}
