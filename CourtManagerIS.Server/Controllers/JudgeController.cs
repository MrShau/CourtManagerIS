using CourtManagerApi.Attributes;
using CourtManagerApi.Dtos.Employee;
using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using CourtManagerApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourtManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JudgeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRep;
        private readonly IJudgeRepository _judgeRep;
        private readonly IPositionRepository _positionRep;
        private readonly IPersonRepository _personRep;
        private readonly JwtService _jwtService;

        public JudgeController(IJudgeRepository judgeRepository, IEmployeeRepository employeeRepository, IPositionRepository positionRepository, IPersonRepository personRepository, JwtService jwtService)
        {
            _judgeRep = judgeRepository;
            _employeeRep = employeeRepository;
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

        [HttpGet]
        [Route("getpage")]
        [JwtAuth]
        public async Task<IActionResult> GetPage()
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return Forbid();

                List<Dtos.Employee.GetListResultDto> result = new List<GetListResultDto>();

                var employees = await _judgeRep.GetList("Person", "Position");

                foreach (var employee in employees)
                {
                    result.Add(new GetListResultDto
                    {
                        Id = employee.Id,
                        FirstName = employee.Person.FirstName,
                        LastName = employee.Person.LastName,
                        Patronymic = employee.Person.Patronymic,
                        DateOfAppointment = employee.DateOfAppointment,
                        DateOfBirth = employee.Person.DateOfBirth,
                        EducationType = employee.Person.GetEducationType(),
                        EmployeeId = employee.Id,
                        Gender = employee.Person.GetGender(),
                        IsBlocked = employee.IsBlocked,
                        Login = employee.Login,
                        NumberPassport = employee.Person.NumberPassport,
                        SeriePassport = employee.Person.SeriePassport,
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

        [HttpPost]
        [Route("add")]
        [JwtAuth]
        public async Task<IActionResult> Add(Dtos.Employee.SIgnUpDto dto)
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return Forbid("Недостаточно прав");

                Position? position = await _positionRep.FirstWhere(p => p.Privilege == Privilege.JUDGE);
                if (position == null)
                    return StatusCode(500, "Ошибка сервера");

                Person? person = await _personRep.Add(new Person(dto.FirstName, dto.LastName, dto.Patronymic, dto.SeriePassport, dto.NumberPassport, dto.DateOfBirth, dto.Gender, dto.EducationType));
                if (person == null)
                    return StatusCode(500, "Ошибка сервера");

                Employee? judge = await _employeeRep.Add(new Employee(dto.Login, dto.Password, person, position));
                if (judge == null)
                    return StatusCode(500, "Ошибка сервера");

                await _employeeRep.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

    }
}
