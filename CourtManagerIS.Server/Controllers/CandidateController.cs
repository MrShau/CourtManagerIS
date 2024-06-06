using Api.Repositories.Interfaces;
using CourtManagerApi;
using CourtManagerApi.Attributes;
using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using CourtManagerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRep;
        private readonly IPositionRepository _positionRep;
        private readonly ICandidateRepository _candidateRep;
        private readonly IPersonRepository _personRep;
        private readonly JwtService _jwtService;

        public CandidateController(IEmployeeRepository employeeRep, ICandidateRepository candidateRep, IPersonRepository personRepository, IPositionRepository positionRepository, JwtService jwtService)
        {
            _employeeRep = employeeRep;
            _candidateRep = candidateRep;
            _positionRep = positionRepository;
            _personRep = personRepository;
            _jwtService = jwtService;
        }

        private string GetRequestLogin()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var user = _jwtService.Verify(token ?? "");

            if (user?.Claims?.FirstOrDefault(c => c.Type == "login") == null)
                return "";

            return user.Claims.First(c => c.Type == "login").Value;
        }

        [HttpPost]
        [Route("add")]
        [JwtAuth]
        public async Task<IActionResult> Add(Dtos.Candidate.AddCandidateDto dto)
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return Forbid("Недостаточно прав");

                Position? position = await _positionRep.Get(dto.DesiredPositionId);
                if (position == null) return BadRequest("Должность не найдена");

                Person? person = await _personRep.Add(new Person(dto.FirstName, dto.LastName, dto.Patronymic, dto.SeriePassport, dto.NumberPassport, dto.DateOfBirth, dto.Gender, dto.EducationType));
                if (person == null) return StatusCode(500);

                Candidate? canditate = await _candidateRep.Add(new Candidate(position, person, dto.WorkExperience));

                return Ok();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getpage")]
        [JwtAuth]
        public async Task<IActionResult> GetPage(SortType sortType = SortType.Ascending, int educationType = -1, int departmentId = -1, int gender = -1, int count = 6, int page = 1)
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return Forbid();

                List<Dtos.Candidate.GetCandidatePageDto> result = new List<Dtos.Candidate.GetCandidatePageDto>();

                var candidates = new List<Candidate>();
                var user = await _employeeRep.Get(this.GetRequestLogin(), "Position");

                if (user == null || user.Position == null)
                    return StatusCode(500);

                candidates = await _candidateRep.GetList(sortType, (educationType < 0 || educationType > 3 ? null : (EducationType)educationType), (user.Position.Privilege == Privilege.ADMIN) ? departmentId : user.Position.DepartmentId, (gender < 0 || gender > 1 ? null : (Gender)gender), count, page, "Person", "DesiredPosition.Department");
               
                foreach ( var candidate in candidates )
                {
                    result.Add(new Dtos.Candidate.GetCandidatePageDto()
                    {
                        Id = candidate.Id,
                        FIO = candidate.Person.GetFIO(),
                        DesiredPositionId = candidate.DesiredPositionId,
                        DesiredPosition = candidate.DesiredPosition.Name,
                        Gender = candidate.Person.GetGender(),
                        WorkExperience = candidate.WorkExperience,
                        DateOfBirth = candidate.Person.DateOfBirth
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
