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
        public string GetPicture(int id)
        {
            var picture = dbContext.Pictures.Where(p => p.Id == id).FirstOrDefault();
            if (picture != null)
            {
                return picture!.JsonData;
            }

            return null;

        }

        public void SavePicture(string pictureJson)
        {
            if (pictureJson != null)
            {
                PictureModel picture = new PictureModel
                {
                    JsonData = pictureJson
                };

                dbContext.Add(picture);
                dbContext.SaveChanges();
            }
            
        }
    }
}
