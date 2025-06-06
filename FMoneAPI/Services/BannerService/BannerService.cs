﻿using FMoneAPI.DTOs;
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
        public async Task UpdateSortOrderAsync(BannerSortRequestDTO request)
        {
            if (request.Banners == null || request.Banners.Count == 0)
            {
                throw new ArgumentException("Banners list cannot be empty");
            }

            await _bannerRepository.UpdateSortOrderAsync(request.Banners);
        }
        public async Task<int> UpdateBannerStatus(int id, bool isActive)
        {
            return await _bannerRepository.UpdateBannerStatus(id, isActive);
        }
        public async Task<int> UpdateBannerCTR(int id)
        {
            return await (_bannerRepository.UpdateBannerCTR(id));
        }
    }
}
