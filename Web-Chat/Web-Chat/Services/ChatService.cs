using System.Net.WebSockets;
using System.Text;
using Web_Chat.Abstractions;

namespace Web_Chat.Services
{
    public class ChatService : IChatService
    {
        public async Task BroadcastMessage(string message, List<WebSocket> sockets)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            foreach (var socket in sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(bytes, WebSocketMessageType.Text, true, default);
                }
            }
        }

        public async Task StartReceiving(WebSocket socket, Action<WebSocketReceiveResult, byte[]> messageHandler)
        {
            var buffer = new byte[1024];
            while (true) 
            {
                if (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(buffer, default);
                    messageHandler(result, buffer);
                }
            }
        }
    }
}
