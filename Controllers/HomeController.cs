using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Task_6.Models;
using Task_6.Services;
using Task_6.Services.Interfaces;

namespace Task_6.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPictureService service;
        private readonly IHubContext<DrawingHub> hubContext;
        public HomeController(IPictureService service, IHubContext<DrawingHub> hubContext)
        {
            this.service = service;
            this.hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveDrawing([FromBody] JsonArray layerData)
        {
            try
            {
                string data = layerData.ToString();
               // service.SavePicture(data);
                await hubContext.Clients.All.SendAsync("ReceiveDrawing",data);
                return Json(new { success = true, message = "Drawing saved successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error saving drawing: {ex.Message}" });
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult GetDrawing(int id)
        {
            var drawing = service.GetPicture(id);

            if (drawing == null)
            {
                return NotFound();
            }

            return Json(drawing);
        }
    }
}
