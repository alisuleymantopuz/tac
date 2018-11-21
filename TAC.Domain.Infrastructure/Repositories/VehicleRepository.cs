using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TAC.Domain.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _dbContext;
        public VehicleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Vehicle> GetAll()
        {
            return _dbContext.Vehicles;
        }

        public async Task<Vehicle> GetById(int id)
        {
            return await _dbContext.Vehicles.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Create(Vehicle entity)
        {
            _dbContext.Vehicles.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, Vehicle entity)
        {
            _dbContext.Vehicles.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _dbContext.Vehicles.FindAsync(id);
            _dbContext.Vehicles.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Vehicle> GetByFilter(Expression<Func<Vehicle, bool>> filter)
        {
            return await _dbContext.Vehicles.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<IList<Vehicle>> ListByFilter(Expression<Func<Vehicle, bool>> filter)
        {
            return await _dbContext.Vehicles.Where(filter).ToListAsync();
        }
    }
}
