using Api.Repositories.Interfaces;
using CourtManagerApi.Attributes;
using CourtManagerApi.Repositories.Interfaces;
using CourtManagerApi.Services;
using CourtManagerIS.Server.Dtos.Event;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventRepository _eventRep;
        private readonly IEmployeeRepository _employeeRep;
        private readonly JwtService _jwtService;

        public EventController(IEventRepository eventRep, IEmployeeRepository employeeRep, JwtService jwtService)
        {
            _eventRep = eventRep;
            _employeeRep = employeeRep;
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
        public async Task<IActionResult> Add(AddEventDto dto)
        {
            try
            {
                if (!await _employeeRep.IsAdminOrDirector(this.GetRequestLogin()))
                    return Forbid("Недостаточно прав");

                if (dto.StartDate < DateTime.Now || dto.EndDate < DateTime.Now || dto.EndDate < dto.StartDate)
                    return BadRequest("Некорректная дата");

                await _eventRep.Add(new CourtManagerApi.Models.Event(dto.Title, dto.Description, dto.StartDate, dto.EndDate, await _employeeRep.Get(this.GetRequestLogin())));

                return Ok();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getactives")]
        [JwtAuth]
        public async Task<IActionResult> GetActives(int count = 0)
        {
            try
            {
                List<GetEventDto> result = new List<GetEventDto>();
                var evs = await _eventRep.Where(e => e.StartDate >= DateTime.Now && e.EndDate >= DateTime.Now, "CreatedEmployee.Person");
                if (count > 0)
                    evs = evs.Take(count).ToList();
                foreach (var item in evs)
                    result.Add(new GetEventDto
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        EndDate = item.EndDate.ToString("dd.mm.yy HH:mm"),
                        StartDate = item.StartDate.ToString("dd.mm.yy HH:mm"),
                        CreaterFIO = item.CreatedEmployee.Person.GetFIO()
                    });

                return Ok(result);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("getcompleted")]
        [JwtAuth]
        public async Task<IActionResult> GetCompleted(int count = 0)
        {
            try
            {
                List<GetEventDto> result = new List<GetEventDto>();
                var evs = await _eventRep.Where(e => e.StartDate < DateTime.Now && e.EndDate < DateTime.Now, "CreatedEmployee.Person");
                if (count > 0)
                    evs = evs.Take(count).ToList();
                foreach (var item in evs)
                    result.Add(new GetEventDto
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        EndDate = item.EndDate.ToString("dd.mm.yy HH:mm"),
                        StartDate = item.StartDate.ToString("dd.mm.yy HH:mm"),
                        CreaterFIO = item.CreatedEmployee.Person.GetFIO()
                    });

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
