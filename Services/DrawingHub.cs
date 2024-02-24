using Microsoft.AspNetCore.SignalR;

namespace Task_6.Services
{
    public class DrawingHub : Hub
    {
        public async Task SendDrawing(string drawingData)
        {
            await Clients.All.SendAsync("ReceiveDrawing", drawingData);
        }
    }
}
