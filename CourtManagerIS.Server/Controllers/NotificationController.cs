using CourtManagerApi.Attributes;
using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using CourtManagerApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CourtManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRep;
        private readonly INotificationRepository _notificationRep;
        private readonly JwtService _jwtService;

        public NotificationController(IEmployeeRepository employeeRepository, INotificationRepository notificationRepository, JwtService jwtService)
        {
            _employeeRep = employeeRepository;
            _notificationRep = notificationRepository;
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
        [Route("send")]
        [JwtAuth]
        public async Task<IActionResult> Send(Dtos.Notification.SendDto dto)
        {
            try
            {
                Employee? sender = await _employeeRep.Get(this.GetRequestLogin());
                if (sender == null) return StatusCode(500);

                Employee? recipient = await _employeeRep.Get(dto.RecipientId);
                if (recipient == null) return StatusCode(500);

                await _notificationRep.Add(new Models.Notification(sender, recipient, dto.Title, dto.Content));

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
        public async Task<IActionResult> Get()
        {
            try
            {
                Employee? employee = await _employeeRep.Get(this.GetRequestLogin(), "Notifications");
                if (employee == null) return StatusCode(500);

                List<Dtos.Notification.GetResultDto> result = new List<Dtos.Notification.GetResultDto>();

                foreach(var item in employee.Notifications) 
                {
                    result.Add(new Dtos.Notification.GetResultDto
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Content = item.Content,
                        SenderId = item.SenderId,
                        SenderFIO = (await _employeeRep.Get(item.SenderId, "Person"))?.Person?.GetFIO() ?? "null",
                        RecipientId = item.RecipientId,
                        RecipientFIO = (await _employeeRep.Get(item.RecipientId, "Person"))?.Person?.GetFIO() ?? "null",
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
