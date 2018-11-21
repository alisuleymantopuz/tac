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
    public class VehicleRepositoryUnitTests : IDisposable
    {
        private Mock<DbSet<Vehicle>> mockSet;
        private Mock<AppDbContext> mockContext;

        public VehicleRepositoryUnitTests()
        {
            mockSet = new Mock<DbSet<Vehicle>>();
            mockContext = new Mock<AppDbContext>();
        }

        [Fact]
        public void VehicleRepository_Create_ShouldBeAbleToCreateVehicle()
        {
            var newVehicle = new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown };
            mockContext.Setup(m => m.Vehicles).Returns(mockSet.Object);
            IVehicleRepository repository = new VehicleRepository(mockContext.Object);
            repository.Create(newVehicle);
            mockSet.Verify(m => m.Add(It.IsAny<Vehicle>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default(System.Threading.CancellationToken)), Times.Once);
        }


        [Fact]
        public void VehicleRepository_Delete_ShouldBeAbleToDeleteVehicle()
        {
            int idToDelete = 1;
            mockContext.Setup(m => m.Vehicles).Returns(mockSet.Object);
            IVehicleRepository _VehicleRepository = new VehicleRepository(mockContext.Object);
            _VehicleRepository.Delete(idToDelete);
            mockSet.Verify(m => m.FindAsync(idToDelete), Times.Once);
            mockSet.Verify(m => m.Remove(It.IsAny<Vehicle>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default(System.Threading.CancellationToken)), Times.Once);
        }

        [Fact]
        public void VehicleRepository_Update_ShouldBeAbleToUpdateVehicle()
        {
            var newVehicle = new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown };
            int idToUpdate = 1;
            mockContext.Setup(m => m.Vehicles).Returns(mockSet.Object);
            IVehicleRepository _VehicleRepository = new VehicleRepository(mockContext.Object);
            _VehicleRepository.Update(idToUpdate, newVehicle);
            mockSet.Verify(m => m.Update(It.IsAny<Vehicle>()), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default(System.Threading.CancellationToken)), Times.Once);
        }

        [Fact]
        public void VehicleRepository_GetById_ShouldBeAbleToReturnRecordByID()
        {
            IQueryable<Vehicle> vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
            }.AsQueryable();

            mockContext.Setup(c => c.Vehicles).ReturnsDbSet(vehicles);
            IVehicleRepository _VehicleRepository = new VehicleRepository(mockContext.Object);
            var actual = _VehicleRepository.GetById(1).Result;
            actual.VIN.Should().Be("YS2R4X20005399401");
        }


        [Fact]
        public void VehicleRepository_GetAll_ShouldBeAbleToReturnRecords()
        {
            IQueryable<Vehicle> vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
                new Vehicle { Id = 2, VIN = "VLUR4X20009093588", RegistrationNumber = "DEF456", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown }
            }.AsQueryable();

            mockContext.Setup(c => c.Vehicles).ReturnsDbSet(vehicles);
            IVehicleRepository _VehicleRepository = new VehicleRepository(mockContext.Object);
            var actual = _VehicleRepository.GetAll().ToListAsync().Result;
            actual.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void VehicleRepository_ListByFilter_ShouldBeAbleToReturnRecordsByFilter()
        {
            IQueryable<Vehicle> vehicles = new List<Vehicle>
            {
               new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
                new Vehicle { Id = 2, VIN = "VLUR4X20009093588", RegistrationNumber = "DEF456", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown }
            }.AsQueryable();
            mockContext.Setup(c => c.Vehicles).ReturnsDbSet(vehicles);
            IVehicleRepository _vehicleRepository = new VehicleRepository(mockContext.Object);
            var actual = _vehicleRepository.ListByFilter(x => x.Id == 1).Result;
            actual.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public void VehicleRepository_GetByFilter_ShouldBeAbleToReturnRecordByFilter()
        {
            IQueryable<Vehicle> vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, VIN = "YS2R4X20005399401", RegistrationNumber = "ABC123", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown },
                new Vehicle { Id = 2, VIN = "VLUR4X20009093588", RegistrationNumber = "DEF456", CustomerId = 1, LastConnectedOn = null, ConnectedOn = null, VehicleStatus = VehicleStatus.Unknown }
            }.AsQueryable();
            mockContext.Setup(c => c.Vehicles).ReturnsDbSet(vehicles);
            IVehicleRepository _vehicleRepository = new VehicleRepository(mockContext.Object);
            var actual = _vehicleRepository.GetByFilter(x => x.Id == 1).Result;
            actual.VIN.Should().Be("YS2R4X20005399401");
        }

        public void Dispose()
        {
            mockSet.Reset();
            mockContext.Reset();
        }
    }
}
