using FMoneAPI.DTOs;
using FMoneAPI.Models;

namespace FMoneAPI.Repositories.BannerRepository
{
    public interface IBannerRepository
    {
        Task<IEnumerable<Banner>> GetBanners(bool sortDescending = false);
        Task<Banner> GetBannerById(int id);
        Task<Banner> AddBanner(Banner banner);
        Task<bool> DeleteBanner(int id);
        Task<Banner> UpdateBanner(int id, Banner banner);
        Task UpdateSortOrderAsync(List<BannerSortOrderDto> banners);
        Task<int> GetTotalBannerAsync();
        Task<int> UpdateBannerStatus(int id, bool isActive);
        Task<int> UpdateBannerCTR(int id);
    }
}
