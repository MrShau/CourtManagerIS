
using CourtManagerApi.Attributes;
using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using CourtManagerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [JwtAuth]
    public class PositionController : Controller
    {
        private readonly IPositionRepository _positionRep;
        private readonly IEmployeeRepository _employeeRep;
        private readonly JwtService _jwtService;

        public PositionController(IPositionRepository positionRep, IEmployeeRepository employeeRep, JwtService jwtService)
        {
            _positionRep = positionRep;
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

        [HttpGet]
        [Route("getallnames")]
        [JwtAuth]
        public async Task<IActionResult> GetAllNames(int departmentId = 0)
        {
            try
            {
                List<Position> positions =  departmentId == 0 ? await _positionRep.GetAll() : await _positionRep.Where(p => p.DepartmentId == departmentId);
                var result = new List<Dtos.Position.GetAllNamesResultDto>();

                foreach(var position in positions)
                {
                    if (await _employeeRep.IsDirector(this.GetRequestLogin()) && position.Privilege == Privilege.ADMIN || position.Name == "Руководитель") {
                        continue;
                    }

                    result.Add(new Dtos.Position.GetAllNamesResultDto()
                    {
                        Id = position.Id,
                        Name = position.Name,
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
