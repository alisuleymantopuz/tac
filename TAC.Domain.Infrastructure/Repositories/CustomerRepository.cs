using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TAC.Domain.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IQueryable<Customer> GetAll()
        {
            return _dbContext.Customers;
        }

        public async Task<Customer> GetById(int id)
        {
            return await _dbContext.Customers.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task Create(Customer entity)
        {
            _dbContext.Customers.Add(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(int id, Customer entity)
        {
            _dbContext.Customers.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var entity = await _dbContext.Customers.FindAsync(id);
            _dbContext.Customers.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Customer> GetByFilter(Expression<Func<Customer, bool>> filter)
        {
            return await _dbContext.Customers.Where(filter).FirstOrDefaultAsync();
        }

        public async Task<IList<Customer>> ListByFilter(Expression<Func<Customer, bool>> filter)
        {
            return await _dbContext.Customers.Where(filter).ToListAsync();
        }
    }
}
