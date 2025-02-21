using FMoneAPI.Models;

namespace FMoneAPI.Services.BannerService
{
    public interface IBannerService
    {
        Task<IEnumerable<Banner>> GetBanners();
        Task<Banner> GetBannerById(int id);
        Task<Banner> AddBanner(Banner banner);
        Task<bool> DeleteBanner(int id);
        Task<Banner> UpdateBanner(int id, Banner banner);
    }
}
