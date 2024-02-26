using Task_6.Models;

namespace Task_6.Services.Interfaces
{
    public interface IPictureService
    {

        public Task<PictureModel?> GetPictureAsync(int id);

        public Task<IEnumerable<PictureModel>> GetPicturesAsync();

        public Task AddNewPictureAsync();

        public Task<string> GetPictureDataAsync(int id);
        public Task SavePictureAsync(string pictureJson, int id);

        public Task DeletePictureAsync(int id);
    }
}
