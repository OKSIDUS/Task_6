using Task_6.Models;

namespace Task_6.Services.Interfaces
{
    public interface IPictureService
    {
        public string GetPicture(int id);
        public void SavePicture(string pictureJson);
    }
}
