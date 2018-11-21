using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TAC.Domain.Infrastructure;
using TAC.Domain.Infrastructure.Repositories;
using Xunit;

namespace TAC.Domain.Tests
{
    public class CustomerRepositoryUnitTests : IDisposable
    {
        private Mock<DbSet<Customer>> mockSet;
        private Mock<AppDbContext> mockContext;

        public CustomerRepositoryUnitTests()
        {
            mockSet = new Mock<DbSet<Customer>>();
            mockContext = new Mock<AppDbContext>();
        }

        [Fact]
        public void CustomerRepository_Create_ShouldBeAbleToCreateCustomer()
        {
            mockContext.Setup(m => m.Customers).Returns(mockSet.Object);
            ICustomerRepository repository = new CustomerRepository(mockContext.Object);
            var newCustomer = new Customer { Id = 1, Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje" };
            repository.Create(newCustomer);
            mockSet.Verify(m => m.Add(It.IsAny<Customer>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default(System.Threading.CancellationToken)), Times.Once);
        }

        [Fact]
        public void CustomerRepository_Delete_ShouldBeAbleToDeleteCustomer()
        {
            int idToDelete = 1;
            mockContext.Setup(m => m.Customers).Returns(mockSet.Object);
            ICustomerRepository _customerRepository = new CustomerRepository(mockContext.Object);
            _customerRepository.Delete(idToDelete);
            mockSet.Verify(m => m.FindAsync(idToDelete), Times.Once);
            mockSet.Verify(m => m.Remove(It.IsAny<Customer>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default(System.Threading.CancellationToken)), Times.Once);
        }

        [Fact]
        public void CustomerRepository_Update_ShouldBeAbleToUpdateCustomer()
        {
            int idToUpdate = 1;
            var newCustomer = new Customer { Id = 1, Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje" };
            mockContext.Setup(m => m.Customers).Returns(mockSet.Object);
            ICustomerRepository _customerRepository = new CustomerRepository(mockContext.Object);
            _customerRepository.Update(idToUpdate, newCustomer);
            mockSet.Verify(m => m.Update(It.IsAny<Customer>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default(System.Threading.CancellationToken)), Times.Once);
        }

        [Fact]
        public void CustomerRepository_GetById_ShouldBeAbleToReturnRecordByID()
        {
            IQueryable<Customer> customers = new List<Customer> { new Customer { Id = 1, Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje" } }.AsQueryable();
            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);
            ICustomerRepository _customerRepository = new CustomerRepository(mockContext.Object);
            var actual = _customerRepository.GetById(1).Result;
            actual.Name.Should().Be("Kalles Grustransporter AB");
        }

        [Fact]
        public void CustomerRepository_GetAll_ShouldBeAbleToReturnRecords()
        {
            IQueryable<Customer> customers = new List<Customer>
            {
                new Customer {  Id = 1, Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje" },
                new Customer {   Id = 2, Name = "Johans Bulk AB", Address = "Balkvägen 12, 222 22 Stockholm"  }
            }.AsQueryable();

            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);
            ICustomerRepository _customerRepository = new CustomerRepository(mockContext.Object);
            var actual = _customerRepository.GetAll().ToListAsync().Result;
            actual.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CustomerRepository_ListByFilter_ShouldBeAbleToReturnRecordsByFilter()
        {
            IQueryable<Customer> customers = new List<Customer>
            {
                new Customer {  Id = 1, Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje" },
                new Customer {   Id = 2, Name = "Johans Bulk AB", Address = "Balkvägen 12, 222 22 Stockholm"  }
            }.AsQueryable();
            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);
            ICustomerRepository _customerRepository = new CustomerRepository(mockContext.Object);
            var actual = _customerRepository.ListByFilter(x => x.Id == 1).Result;
            actual.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void CustomerRepository_GetByFilter_ShouldBeAbleToReturnRecordByFilter()
        {
            IQueryable<Customer> customers = new List<Customer>
            {
                new Customer {  Id = 1, Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje" },
                new Customer {   Id = 2, Name = "Johans Bulk AB", Address = "Balkvägen 12, 222 22 Stockholm"  }
            }.AsQueryable();
            var mockContext = new Mock<AppDbContext>();
            mockContext.Setup(c => c.Customers).ReturnsDbSet(customers);
            ICustomerRepository _customerRepository = new CustomerRepository(mockContext.Object);
            var actual = _customerRepository.GetByFilter(x => x.Id == 1).Result;
            actual.Name.Should().Be("Kalles Grustransporter AB");
        }

        public void Dispose()
        {
            mockSet.Reset();
            mockContext.Reset();
        }
    }
}
