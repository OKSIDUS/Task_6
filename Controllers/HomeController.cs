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

        public async Task<IActionResult> Index()
        {
            var pictures = await service.GetPicturesAsync();
            return View(pictures);
        }

        [HttpPost]
        public async Task<IActionResult> SaveDrawing([FromBody] JsonArray layerData)
        {
            try
            {
                string data = layerData.ToString();
                //service.SavePicture(data);
                await hubContext.Clients.All.SendAsync("ReceiveDrawing",data);
                return Json(new { success = true, message = "Drawing saved successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error saving drawing: {ex.Message}" });
            }
        }

        public async Task<IActionResult> DrawingPage(int id)
        {
            TempData["id"] = id;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDrawing(int id)
        {
            var drawing = await service.GetPictureDataAsync(id);

            if (drawing == null)
            {
                return NotFound();
            }

            return Json(drawing);
        }
    }
}
