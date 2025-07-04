using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Infrastructure.SqlServer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.SqlServer.Repository
{
    public class PlanningRepository : IPlanningRepository
    {
        private readonly PlanningServiceDbContext _context;
        public PlanningRepository(PlanningServiceDbContext context)
        {
            _context = context;
        }

        public async Task<Planning> Accept(Guid planningId, Guid userId)
        {
            var planning = await _context.Plannings.FindAsync(planningId);

            if (planning == null)
                throw new Exception("Không tìm thấy kế hoạch.");
            planning.CustomerId = userId;
            planning.Status = PlanningStatus.Confirmed;
            planning.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return planning;
        }


        public Task<Guid> AddService(SesstionService sesstionService)
        {
            _context.SesstionServices.Add(sesstionService);
            return _context.SaveChangesAsync().ContinueWith(t => sesstionService.Id);
        }

        public Task<Planning> CreateStep1Async(Planning planning, Guid userId)
        {
            _context.Plannings.Add(planning);
            return _context.SaveChangesAsync().ContinueWith(t => planning);
        }

        public async Task<Planning> CreateStep2Async(Planning planning)
        {
            planning.UpdatedAt = DateTime.Now;
            _context.Plannings.Attach(planning);
            _context.Entry(planning).Property(x => x.UpdatedAt).IsModified = true;
            _context.Entry(planning).Property(x => x.Name).IsModified = true;
            _context.Entry(planning).Property(x => x.Description).IsModified = true;
            _context.Entry(planning).Property(x => x.Location).IsModified = true;
            _context.Entry(planning).Property(x => x.AboutNumberPeople).IsModified = true;
            _context.Entry(planning).Property(x => x.TypeOfEvent).IsModified = true;
            _context.Entry(planning).Property(x => x.Budget).IsModified = true;
            _context.Entry(planning).Property(x => x.MainColor).IsModified = true;
            _context.Entry(planning).Property(x => x.DateOfEvent).IsModified = true;
            await _context.SaveChangesAsync();
            return planning;
        }


        public async Task<bool> DeletePlanAsync(Guid planningId)
        {
            var planning = await GetPlanByIdAsync(planningId);
            if (planning == null)
            {
                return false;
            }

            planning.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteService(Guid sesstionId)
        {
            var sesstionService = await GetSessionByIdAsync(sesstionId);
            if (sesstionService == null)
            {
                return false;
            }

            sesstionService.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<Planning> Items, int TotalCount)> GetAllPlansAsync(int page, int size, PlanningStatus status, string? keyword, Guid userId)
        {
            var query = _context.Plannings.Include(p => p.SesstionServices.Where(s => !s.IsDeleted))
       .Where(p => !p.IsDeleted && p.CustomerId == userId);

            if (Enum.IsDefined(typeof(PlanningStatus), status))
            {
                query = query.Where(p => p.Status == status);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Name.Contains(keyword));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync();

            return (items, totalCount);
        }



        public async Task<int> GetNumberPlanningAsync(Guid userId) => await _context.Plannings.CountAsync(p => p.CustomerId == userId && !p.IsDeleted).ContinueWith(t => t.Result);

        public async Task<Planning?> GetPlanByIdAsync(Guid planningId) => await _context.Plannings.FirstOrDefaultAsync(p => p.Id == planningId && !p.IsDeleted);
        public async Task<SesstionService?> GetSessionByIdAsync(Guid sessionServiceId) => await _context.SesstionServices.FirstOrDefaultAsync(p => p.Id == sessionServiceId && !p.IsDeleted);
    }
}
