using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Nodes;
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
        public async Task<IActionResult> SaveDrawing([FromBody] JsonArray layerData, int id)
        {
            try
            {
                TempData.TryGetValue("id", out var idPicture);
                string data = layerData.ToString();
                await service.SavePictureAsync(data, (int)idPicture!);
                await hubContext.Clients.All.SendAsync("ReceiveDrawing",data);
                TempData["id"] = idPicture;
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
            TempData["id"] = id;
            if (drawing == null)
            {
                return NotFound();
            }

            return Json(drawing);
        }

        public async Task<IActionResult> Create()
        {
            await service.AddNewPictureAsync();
            var pictures = await service.GetPicturesAsync();
            var picture = pictures.LastOrDefault();
            if(picture != null)
            {
                TempData["id"] = picture.Id;
                return RedirectToAction("DrawingPage", new { id = picture.Id });
            }

            return RedirectToAction("Index");

        }


        public async Task<IActionResult> Delete(int id)
        {
            await service.DeletePictureAsync(id);

            return RedirectToAction("Index");
        }
    }
}
