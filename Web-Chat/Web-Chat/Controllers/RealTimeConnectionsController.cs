using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using Web_Chat.Abstractions;

namespace Web_Chat.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RealTimeConnectionsController : Controller
    {
        private readonly IChatService _chatService;

        private List<WebSocket> _connectons;

        public RealTimeConnectionsController(IChatService chatService)
        {
            _chatService = chatService;
            _connectons = new List<WebSocket>();
        }

        [Route("BaseChat")]
        public async Task<IActionResult> BaseChat([FromQuery]string userName)
        {
            using var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            _connectons.Add(socket);

            await _chatService.BroadcastMessage(userName + "joined", _connectons);
            await _chatService.StartReceiving(socket, async (result, buffer) =>
            {
                var message = Encoding.UTF8.GetString(buffer);
                await _chatService.BroadcastMessage(userName + message, _connectons);
            });

            return Ok();
        }
    }
}
