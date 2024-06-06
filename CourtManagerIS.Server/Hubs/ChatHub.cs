using Api.Repositories.Interfaces;
using CourtManagerApi.Models;
using CourtManagerApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace Api.Hubs
{
    [EnableCors]
    public class ChatHub : Hub
    {
        public IChatRepository _chatRep;
        public IEmployeeRepository _employeeRep;

        public ChatHub(IChatRepository chatRep, IEmployeeRepository employeeRepository)
        {
            _employeeRep = employeeRepository;
            _chatRep = chatRep;
        }

        public async Task Send(int senderId, string message)
        {
            try
            {
                Employee? employee = await _employeeRep.Get(senderId);
                    if (employee == null) return;

                await _chatRep.Add(new ChatMessage(employee, message));

                await this.Clients.All.SendAsync("Receive", message);
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }
    }
}
