using System.Net.WebSockets;

namespace Web_Chat.Abstractions
{
    public interface IChatService
    {
        Task StartReceiving(WebSocket socket, Action<WebSocketReceiveResult, byte[]> messageHandler);

        Task BroadcastMessage(string message, List<WebSocket> sockets);
    }
}
