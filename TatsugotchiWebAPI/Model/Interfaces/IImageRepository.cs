namespace TatsugotchiWebAPI.Model.Interfaces
{
    public interface IImageRepository
    {
        void RemoveImage(Image i);
        void SaveChanges();
    }
}
