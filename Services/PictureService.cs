using Microsoft.EntityFrameworkCore;
using Task_6.dabases;
using Task_6.Models;
using Task_6.Services.Interfaces;

namespace Task_6.Services
{
    public class PictureService : IPictureService
    {
        private readonly PictureDbContext dbContext;
        public PictureService(PictureDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddNewPictureAsync(string userName)
        {
            await dbContext.AddAsync(new PictureModel
            {
                CreatedBy = userName,
            });
           await dbContext.SaveChangesAsync();
        }

        public async Task DeletePictureAsync(int id)
        {
            var picture = await dbContext.Pictures.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (picture != null)
            {
                dbContext.Pictures.Remove(picture);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<PictureModel?> GetPictureAsync(int id)
        {
            var picture = await dbContext.Pictures.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (picture != null)
            {
                return picture;
            }

            return null;
        }

        public async Task<string> GetPictureDataAsync(int id)
        {
            var picture = await dbContext.Pictures.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (picture != null)
            {
                return picture.JsonData;
            }

            return string.Empty;
        }

        public async Task<IEnumerable<PictureModel>> GetPicturesAsync()
        {
            var pictures = await dbContext.Pictures.ToListAsync();
            return pictures;
        }

        public async Task SavePictureAsync(string pictureJson, int id)
        {
            var picture = dbContext.Pictures.Where(p => p.Id == id).FirstOrDefault();
            if (picture != null)
            {
                picture.JsonData = pictureJson;
                dbContext.Update(picture);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
