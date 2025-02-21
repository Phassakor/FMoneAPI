using FMoneAPI.Models;
using FMoneAPI.Repositories.BannerRepository;
using FMoneAPI.Repositories.UserRepository;

namespace FMoneAPI.Services.BannerService
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public Task<IEnumerable<Banner>> GetBanners()
        {
            return _bannerRepository.GetBanners();
        }
        public Task<Banner> GetBannerById(int id)
        {
            return _bannerRepository.GetBannerById(id);
        }
        public Task<Banner> AddBanner(Banner banner)
        {
            return _bannerRepository.AddBanner(banner);
        }
        public Task<bool> DeleteBanner(int id)
        {
            return _bannerRepository.DeleteBanner(id);  
        }
        public Task<Banner> UpdateBanner(int id, Banner banner)
        {
            return _bannerRepository.UpdateBanner(id, banner);
        }
    }
}
