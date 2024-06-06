using Api.Dtos.Chat;
using Api.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository _chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        [HttpGet]
        [Route("getmessages")]
        public async Task<IActionResult> GetMessages()
        {
            try
            {
                List<GetMessagesDto> result = new List<GetMessagesDto> { };
                foreach(var item in (await _chatRepository.GetAll("Sender")).Take(100))
                {
                    result.Add(new GetMessagesDto
                    {
                        Id = item.Id,
                        Login = item.Sender.Login,
                        DateTime = item.CreatedAt.ToString("dd.MM HH:mm"),
                        Message = item.Content
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
