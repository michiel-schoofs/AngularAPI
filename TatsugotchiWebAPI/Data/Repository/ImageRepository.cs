using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TatsugotchiWebAPI.Model;
using TatsugotchiWebAPI.Model.Interfaces;

namespace TatsugotchiWebAPI.Data.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<Image> _images;

        public ImageRepository(ApplicationDBContext context){
            _context = context;
            _images = context.Images;
        }

        public void RemoveImage(Image i){
            _images.Remove(i);
        }

        public void SaveChanges(){
            _context.SaveChanges();
        }
    }
}
