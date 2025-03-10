using AutoMapper;
using FMoneAPI.Data;
using FMoneAPI.DTOs;
using FMoneAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FMoneAPI.Repositories.BannerRepository
{
    public class BannerRepository : IBannerRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public BannerRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Banner>> GetBanners(bool sortDescending = false)
        {
            return sortDescending
                ? await _context.Banner.OrderByDescending(b => b.SortOrder).ToListAsync()
                : await _context.Banner.OrderBy(b => b.SortOrder).ToListAsync();
        }
        public async Task<Banner> GetBannerById(int id) => await _context.Banner.FindAsync(id);

        public async Task<Banner> AddBanner(Banner banner)
        {
            _context.Banner.Add(banner);
            await _context.SaveChangesAsync();
            return banner;
        }

        public async Task<bool> DeleteBanner(int id)
        {
            var banner = await _context.Banner.FindAsync(id);
            if (banner == null) return false;

            _context.Banner.Remove(banner);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Banner> UpdateBanner(int id, Banner banner)
        {
            var existingBanner = await _context.Banner.FindAsync(id);
            if (existingBanner == null) return null;

            existingBanner.Title = banner.Title;
            existingBanner.ImageUrl = banner.ImageUrl;
            await _context.SaveChangesAsync();
            return existingBanner;
        }
        public async Task UpdateSortOrderAsync(List<BannerSortOrderDto> banners)
        {
            var bannerIds = banners.Select(b => b.Id).ToList();
            var bannerEntities = await _context.Banner.Where(b => bannerIds.Contains(b.ID)).ToListAsync();

            foreach (var banner in bannerEntities)
            {
                var newSortOrder = banners.FirstOrDefault(b => b.Id == banner.ID)?.SortOrder;
                if (newSortOrder.HasValue)
                {
                    banner.SortOrder = newSortOrder.Value;
                }
            }

            await _context.SaveChangesAsync();
        }
        public async Task<int> GetTotalBannerAsync()
        {
            return await _context.Banner.CountAsync();
        }
    }
}
