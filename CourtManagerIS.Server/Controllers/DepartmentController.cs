using CourtManagerApi.Attributes;
using CourtManagerApi.Dtos.Department;
using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CourtManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRep;
        private readonly IPositionRepository _positionRep;
        private readonly IEmployeeRepository _employeeRep;

        public DepartmentController(IDepartmentRepository departmentRepository, IPositionRepository positionRepository, IEmployeeRepository employeeRep)
        {
            _departmentRep = departmentRepository;
            _positionRep = positionRepository;
            _employeeRep = employeeRep;
        }

        [HttpGet]
        [Route("getallnames")]
        public async Task<IActionResult> GetAllNames()
        {
            try
            {
                List<GetAllNamesResultDto> result = new List<GetAllNamesResultDto> ();
                foreach (var department in await _departmentRep.GetAll())
                {
                    result.Add(new GetAllNamesResultDto
                    {
                        Id = department.Id,
                        Name = department.Name,
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
        [Route("getpage")]
        [JwtAuth]
        public async Task<IActionResult> GetPage(int count = 6, int page = 1)
        {
            try
            {
                List<Dtos.Department.GetListResultDto> result = new List<Dtos.Department.GetListResultDto>();

                foreach (var d in await _departmentRep.GetList(count, page, "Supervisor.Person"))
                {
                    var positions = await _positionRep.Where(p => p.DepartmentId == d.Id, "Employees");
                    var positionsName = "";
                    int employeeCount = 0;
                    foreach (var item in positions)
                    {
                        employeeCount += item.Employees?.Count ?? 0;
                        positionsName += $"{item.Name}, ";
                    }

                    result.Add(new GetListResultDto
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Description = d.Description,
                        SupervisorId = d.SupervisorId.HasValue ? d.SupervisorId.Value : 0,
                        Supervisor = d.Supervisor?.Person.GetFIO() ?? "",
                        PositionsCount = positions.Count,
                        Positions = positionsName,
                        EmployeesCount = employeeCount
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
        public async Task<IActionResult> Add(Dtos.Department.AddDto dto)
        {
            try
            {
                var department = await _departmentRep.Add(new Models.Department(dto.Name, dto.Description));

                if (department == null)
                    return StatusCode(500);

                if (dto.SupervisorId.HasValue)
                {
                    var supervisor = await _employeeRep.Get(dto.SupervisorId.Value);
                    department.Supervisor = supervisor;
                    await _departmentRep.SaveChanges();
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
        [Route("get")]
        [JwtAuth]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Department? department = await _departmentRep.Get(id);

                if (department == null)
                    return BadRequest();

                return Ok(department);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500, "Ошибка сервера");
            }
        }

        [HttpPatch]
        [Route("update")]
        public async Task<IActionResult> Update(Dtos.Department.UpdateDepartmentDto dto)
        {
            try
            {
                Department? department = await _departmentRep.Get(dto.Id);
                if (department == null)
                    return BadRequest();

                if (dto.Description != null)
                {
                    department.Description = dto.Description;
                }

                if (dto.Name != null)
                {
                    if (_departmentRep.IsExist(dto.Name) && department.Name != dto.Name)
                        return BadRequest("Такой отдел уже существует !");
                    department.Name = dto.Name;
                }

                

                await _departmentRep.SaveChanges();

                return Ok();
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500, "Ошибка сервера");
            }
        }

        [HttpPatch]
        [Route("setsupervisor")]
        public async Task<IActionResult> SetSupervisor(SetSupervisorDto dto)
        {
            try
            {
                Department? department = await _departmentRep.Get(dto.Id);
                if (department == null)
                    return BadRequest();

                Employee? employee = await _employeeRep.Get(dto.SupervisorLogin);
                if (employee == null)
                    return BadRequest("Пользователь не найден");

                if ((await _departmentRep.Where(d => d.SupervisorId == employee.Id)).Count > 0)
                    return BadRequest("Пользователь уже является руководителем другого отдела");

                Position? position = await _positionRep.FirstWhere(p => p.DepartmentId == department.Id && p.Privilege == Privilege.DIRECTOR);
                if (position == null)
                    return StatusCode(500);

                if (department.Supervisor != null)
                    this.DeleteSupervisor(new DeleteSupervisorDto { Id = dto.Id });

                department.SupervisorId = employee.Id;
                department.Supervisor = employee;
                employee.Position = position;

                await _departmentRep.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPatch]
        [Route("deletesupervisor")]
        public async Task<IActionResult> DeleteSupervisor(DeleteSupervisorDto dto)
        {
            try
            {
                Department? department = await _departmentRep.Get(dto.Id);
                if (department == null)
                    return BadRequest();
                department.Supervisor.PositionId = (await _positionRep.FirstWhere(p => p.Privilege == Privilege.EMPLOYEE && p.DepartmentId == dto.Id))?.Id ?? department.Supervisor.PositionId;
                department.SupervisorId = null;
                department.Supervisor = null;
                await _departmentRep.SaveChanges();

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
