using Api.Dtos.Employee;
using CourtManagerApi.Attributes;
using CourtManagerApi.Dtos.Employee;
using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using CourtManagerApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourtManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRep;
        private readonly IPersonRepository _personRep;
        private readonly IPositionRepository _positionRep;
        private readonly IDepartmentRepository _departmentRep;
        private readonly AppDbContext _context;

        private readonly JwtService _jwtService;

        public EmployeeController(
            IEmployeeRepository employeeRep, 
            IPersonRepository personRepository, 
            IPositionRepository positionRepository,
            IDepartmentRepository departmentRepository,
            JwtService jwtService,
            AppDbContext appDbContext
            )
        {
            _employeeRep = employeeRep;
            _personRep = personRepository;
            _positionRep = positionRepository;
            _departmentRep = departmentRepository;
            _jwtService = jwtService;
            _context = appDbContext;
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
        [JwtAuth]
        [Route("signup")]
        public async Task<IActionResult> SignUp(Dtos.Employee.SIgnUpDto dto)
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return StatusCode(StatusCodes.Status403Forbidden);

                if (_employeeRep.IsExist(dto.Login))
                    return Unauthorized("Логин занят");

                Person? person = await _personRep.Add(new Person(dto.FirstName, dto.LastName, dto.Patronymic, dto.SeriePassport, dto.NumberPassport, dto.DateOfBirth, dto.Gender, dto.EducationType));
                Position? position = await _positionRep.Get(dto.PositionId);

                if (person == null) return StatusCode(500);
                if (position == null) return BadRequest("Должность была указана неправильно");

                Employee? employee = await _employeeRep.Add(new Employee(dto.Login, dto.Password, person, position));

                if (employee == null)
                    return StatusCode(500);

                Employee? creater = await _employeeRep.Get(this.GetRequestLogin(), "ActivityHistory");
                if (creater != null) 
                {
                    creater.ActivityHistory.Last().Description += $"Добавил нового сотрудника (ИД: {employee.Id}) ";
                    await _employeeRep.SaveChanges();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("signin")]
        public async Task<IActionResult> SignIn(Dtos.Employee.SignInDto dto)
        {
            try
            {
                if (!_employeeRep.IsExist(dto.Login)) return Unauthorized("Логин не зарегистрирован");

                Employee? employee = await _employeeRep.Get(dto.Login, "Position", "Person");
                if (employee == null) return StatusCode(500);
                if (employee.IsBlocked) return Unauthorized("Вы заблокированы !");
                if (!employee.VerifyPassword(dto.Password))
                {
                    employee.PasswordAttempts++;
                    await _employeeRep.SaveChanges();
                    return Unauthorized("Неверный пароль");
                }

                employee.PasswordAttempts = 0;
                await _employeeRep.SaveChanges();
                string token = _jwtService.Generate(employee.Id, employee.Login);

                await _employeeRep.AddActivity(new Activity(employee));

                return Ok(token);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("auth")]
        [JwtAuth]
        public async Task<IActionResult> Auth()
        {
            try
            {
                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var employee = _jwtService.Verify(token ?? "");
                if (employee == null || employee.Claims?.FirstOrDefault(c => c.Type == "login") == null)
                    return Unauthorized("Токен недействителен !");

                var resultUser = await _employeeRep.Get(employee.Claims.First(c => c.Type == "login").Value, "Person", "Position");

                if (resultUser == null)
                    return Unauthorized("Токен недействителен !" );

                return Ok(new
                {
                    id = resultUser.Id,
                    login = resultUser.Login,
                    firstName = resultUser.Person.FirstName,
                    lastName = resultUser.Person.LastName,
                    position = resultUser.Position.Name,
                    imagePath = resultUser.ImagePath,
                    privilege = resultUser.Position.Privilege.ToString(),
                });
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                return StatusCode(500, new
                {
                    error = "Ошибка сервера"
                });
            }
        }

        [HttpGet]
        [Route("getpage")]
        [JwtAuth]
        public async Task<IActionResult> GetPage(string loginLike = "", SortType sortType = SortType.Ascending, int educationType = -1, int departmentId = -1, int gender = -1, int count = 6, int page = 1)
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return Forbid();

                List<Dtos.Employee.GetListResultDto> result = new List<GetListResultDto>();

                var employees = new List<Employee>();
                var user = await _employeeRep.Get(this.GetRequestLogin(), "Position");

                if (user == null || user.Position == null)
                    return StatusCode(500);

                if (user.Position.Privilege == Privilege.ADMIN)
                    employees = await _employeeRep.GetList(sortType, (educationType < 0 || educationType > 3 ? null : (EducationType)educationType), departmentId, (gender < 0 || gender > 1 ? null : (Gender)gender), loginLike, count, page, "Person", "Position.Department");
                else
                    employees = await _employeeRep.GetList(user.Position.DepartmentId, sortType, (educationType < 0 || educationType > 3 ? null : (EducationType)educationType), (gender < 0 || gender > 1 ? null : (Gender)gender), loginLike, count, page, "Person", "Position.Department");

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
                        Position = employee.Position.Name,
                        Department = employee.Position.Department.Name
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
        [Route("activityend")]
        [JwtAuth]
        public async Task<IActionResult> ActivityEnd()
        {
            try
            {
                Employee? employee = await _employeeRep.Get(this.GetRequestLogin(), "ActivityHistory");
                
                if (employee == null) return StatusCode(500);

                employee.ActivityHistory.Last().SignOutTime = DateTime.Now;

                await _employeeRep.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("get")]
        [JwtAuth]
        public async Task<IActionResult> Get(int id = 0, string login = "")
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return Forbid();

                Employee? e = id > 0 ? await _employeeRep.Get(id, "Position", "Person") : await _employeeRep.Get(login, "Position", "Person");

                if (e == null) return StatusCode(500);

                return Ok(new
                {
                    FirstName = e.Person.FirstName,
                    LastName = e.Person.LastName,
                    Patronymic = e.Person.Patronymic,
                    SeriePassport = e.Person.SeriePassport,
                    NumberPassport = e.Person.NumberPassport,
                    DateOfBirth = e.Person.DateOfBirth,
                    DateOfAppointment = e.DateOfAppointment,
                    Gender = e.Person.Gender,
                    EducationType = e.Person.EducationType,
                    DepartmentId = e.Position.DepartmentId,
                    PositionId = e.PositionId,
                    Position = e.Position.Name,
                    Login = e.Login,
                    IsBlocked = e.IsBlocked,
                    Id = e.Id,
                });
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPatch]
        [Route("update")]
        [JwtAuth]
        public async Task<IActionResult> Update(Dtos.Employee.UpdateDto dto)
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return Forbid();

                Employee? employee = await _employeeRep.Get(dto.Id, "Position", "Person");
                if (employee == null) return StatusCode(500);
                if (dto.Login != null)
                {
                    if (_employeeRep.IsExist(dto.Login))
                        return BadRequest("Логин занят");
                    if (dto.Login.Length < 3)
                        return BadRequest("Слишком короткий логин");
                    employee.Login = dto.Login;
                }

                
                if (dto.FirstName != null)
                {
                    if (dto.FirstName.Length < 2)
                        return BadRequest("Слишком короткое имя");
                    if (dto.FirstName.Length > 80)
                        return BadRequest("Сликшом длинное имя");
                    employee.Person.FirstName = dto.FirstName;
                }
                if (dto.LastName != null)
                {
                    if (dto.LastName.Length < 2)
                        return BadRequest("Слишком короткая фамилия");
                    if (dto.LastName.Length > 80)
                        return BadRequest("Сликшом длинная фамилия");
                    employee.Person.LastName = dto.LastName;
                }
                if (dto.Patronymic != null)
                {
                    if (dto.Patronymic.Length < 2)
                        return BadRequest("Слишком короткое отчество");
                    if (dto.Patronymic.Length > 80)
                        return BadRequest("Сликшом длинное отчество");
                    employee.Person.Patronymic = dto.Patronymic;
                }
                if (dto.SeriePassport != null && dto.SeriePassport.Length == 4)
                    employee.Person.SeriePassport = dto.SeriePassport;
                if (dto.NumberPassport != null && dto.NumberPassport.Length == 6)
                    employee.Person.NumberPassport = dto.NumberPassport;
                if (dto.DateOfBirth != null)
                    employee.Person.DateOfBirth = dto.DateOfBirth.Value;
                if (dto.Gender != null)
                    employee.Person.Gender = (Gender)dto.Gender;

                await _employeeRep.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPatch]
        [Route("updatedepartment")]
        [JwtAuth]
        public async Task<IActionResult> UpdateDepartament(UpdateDepartment dto)
        {
            try
            {
                Employee? employee = await _employeeRep.Get(dto.Id, "Position");
                if (employee == null)
                    return StatusCode(500, "Ошибка сервера");

                Position? position = await _positionRep.Get(dto.PositionId, "Department", "Employees");
                if (position == null)
                    return StatusCode(500, "Ошибка сервера");

                if (position.Privilege == Privilege.DIRECTOR)
                {
                    if (position.Employees.Count >= 1)
                        return Conflict("Руководитель может быть только один !");
                }

                employee.Position = position;
                await _employeeRep.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500, "Ошибка сервера");
            }
        }

        [HttpPatch]
        [Route("changepassword")]
        [JwtAuth]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return Forbid();

                Employee? employee = await _employeeRep.Get(dto.Id);
                if (employee == null)
                    return StatusCode(500);

                employee.ChangePassowrd(dto.Password);
                await _employeeRep.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPatch]
        [Route("block")]
        [JwtAuth]
        public async Task<IActionResult> Block(BlockDto dto)
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return Forbid();

                Employee? employee = await _employeeRep.Get(dto.Id);
                if (employee == null)
                    return StatusCode(500);

                employee.Status = !dto.Value;
                employee.IsBlocked = dto.Value;

                await _employeeRep.SaveChanges();

                Employee? creater = await _employeeRep.Get(this.GetRequestLogin(), "ActivityHistory");
                if (creater != null)
                {
                    if (dto.Value)
                        creater.ActivityHistory.Last().Description += $"Заблокировал сотрудника (ИД: {employee.Id}) ";
                    else
                        creater.ActivityHistory.Last().Description += $"Разблокировал сотрудника (ИД: {employee.Id}) ";
                    await _employeeRep.SaveChanges();
                }

                return Ok();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getactivityhistory")]
        [JwtAuth]
        public async Task<IActionResult> GetActivityHistory(int id)
        {
            try
            {
                Employee? employee = await _employeeRep.Get(id, "ActivityHistory");
                if (employee == null) 
                    return StatusCode(500);

                List<GetActivityHistoryResultDto> result = new List<GetActivityHistoryResultDto>();

                foreach(var item in employee.ActivityHistory)
                {
                    TimeSpan d = item.SignOutTime - item.SignInTime;
                    string duration = ((int)d.TotalMinutes).ToString() + " минут";
                    result.Add(new GetActivityHistoryResultDto
                    {
                        Id = item.Id,
                        StartDate = item.SignInTime.ToString("dd.MM.yyyy HH:mm:ss"),
                        EndDate = item.SignOutTime.ToString("dd.MM.yyyy HH:mm:ss"),
                        Duration = duration,
                        Description = item.Description
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

        [HttpGet]
        [Route("getchart")]
        [JwtAuth]
        public async Task<IActionResult> GetChart()
        {
            try
            {
                int[] result = new int[12];
                for (int i = 0; i < 12; i++)
                    result[i] = await _context.ActivityHistory.Where(a => a.SignOutTime.Month == i + 1).CountAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getcount")]
        [JwtAuth]
        public async Task<IActionResult> GetCount()
        {
            try
            {
                Employee? employee = await _employeeRep.Get(this.GetRequestLogin(), "Position");
                if (employee == null)
                    return BadRequest();
                int count = employee.Position.Privilege == Privilege.DIRECTOR ? (await _employeeRep.Where(e => e.PositionId == employee.PositionId)).Count : (await _employeeRep.Where(e => true)).Count;

                return Ok(count);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }
    }

    
}
