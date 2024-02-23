using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json.Nodes;
using Task_6.Models;
using Task_6.Services.Interfaces;

namespace Task_6.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPictureService service;
        public HomeController(IPictureService service)
        {
            this.service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveDrawing([FromBody] JsonArray layerData)
        {
            try
            {
                string data = layerData.ToString();
               // service.SavePicture(data);
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
